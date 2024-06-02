let global, dataSet, setNumber, graphNumber;
let leftTeamID, rightTeamID;

function getQueryParams() {
    const params = new URLSearchParams(window.location.search);
    const queryParams = {};
    for (const [key, value] of params.entries()) {
        queryParams[key] = parseInt(value);
    }
    return queryParams;
}

document.addEventListener('DOMContentLoaded', () => {
    const queryParams = getQueryParams();
    // 將參數顯示在頁面上

    // 使用 Fetch API 發送請求
    fetch('/graphicData?UserID='+queryParams['UserID']+'&GameID='+queryParams['GameID'])
        .then(response => {
	    
            if (!response.ok) {
                throw new Error('Network response was not ok ' + response.statusText);
            }
            return response.json();
        })
        .then(data => {
            global = data;
            leftTeamID = global.LREachRound.left[0];
            rightTeamID = global.LREachRound.right[0];
            dataSet = global.ballRecords.set1;
            setNumber = 1;
            graphNumber = 1;
            drawGameTable();
            drawDonut(1,global.ballRecords.set1);
        })
        .catch(error => {
            console.error('There has been a problem with your fetch operation:', error);
       
    	//	paramDisplay.textContent = error;
	});
});


let allGameTableData = {
    1: [
        { Team: '0', Win: 0, Game1: 0, Game2: 0, Game3: 0, Game4: 0, Game5: 0},
        { Team: '0', Win: 0, Game1: 0, Game2: 0, Game3: 0, Game4: 0, Game5: 0}
    ],
}

function drawGameTable() {
    let tableData = allGameTableData[1];
    let tableBody = document.getElementById('allGameTableBody');
    tableData[0].Team = global.teamL;
    tableData[1].Team = global.teamR;
    global.scores.forEach((score, index) => {
        if(index == 1 || index == 3) {
            tableData[1][`Game${index+1}`] = score.leftScore;
            tableData[0][`Game${index+1}`] = score.rightScore;
        } else if(index == 0 || index == 2 || index == 4) {
            tableData[0][`Game${index+1}`] = score.leftScore;
            tableData[1][`Game${index+1}`] = score.rightScore;
        }

        if(tableData[0][`Game${index+1}`] > tableData[1][`Game${index+1}`])
            tableData[0][`Win`]++;
        else if(tableData[0][`Game${index+1}`] < tableData[1][`Game${index+1}`])
            tableData[1][`Win`]++;
    });
    
    tableBody.innerHTML = '';
    tableData.forEach(item => {
        let row = document.createElement('tr');
        row.innerHTML = `
            <td>${item.Team}</td>
            <td>${item.Win}</td>
            <td>${item.Game1}</td>
            <td>${item.Game2}</td>
            <td>${item.Game3}</td>
            <td>${item.Game4}</td>
            <td>${item.Game5}</td>
        `;
        tableBody.appendChild(row);
    });
}

function visualizeSet(num) {
    setNumber = num;
    switch(num) {
        case 1:
            dataSet = global.ballRecords.set1;
            break;
        case 2:
            dataSet = global.ballRecords.set2;
            break;
        case 3:
            dataSet = global.ballRecords.set3;
            break;
        case 4:
            dataSet = global.ballRecords.set4;
            break;
        case 5:
            dataSet = global.ballRecords.set5;
            break;
        default:
            console.error("Invalid set number:", num);
            return;
    }

    if(dataSet.length !== 0) {
        if(graphNumber === 1)
            drawDonut(setNumber,dataSet);
        else if(graphNumber === 2)
            playerTableData(setNumber,dataSet);
        else if(graphNumber === 3)
            drawScatter(setNumber,dataSet);
    }
}

function visualizeGraph(num) {
    graphNumber = num;
    if(dataSet.length !== 0) {
        switch(num) {
        case 1:
            drawDonut(setNumber, dataSet);
            break;
        case 2:
            playerTableData(setNumber, dataSet);
            break;
        case 3:
            drawScatter(setNumber, dataSet);
            break;
        default:
            console.error("Invalid set number:", num);
            return;
        }
    }
}

function drawDonut(setNumber,dataSet) {
    const teamScores = {teamL: 0, teamR: 0};
    const teamAttack = {teamL: 0, teamR: 0};
    const teamBlock = {teamL: 0, teamR: 0};

    dataSet.forEach(record => {
        const teamKey = record.TeamID === leftTeamID ? 'teamL' : 'teamR';
        if (record.behavior === 2)
            teamAttack[teamKey]++;
        else if (record.behavior === 3)
            teamBlock[teamKey]++;

        if(record.score === 1)
            teamScores[`teamL`]++;
        else if(record.score === -1)
            teamScores[`teamR`]++;
    });

    cleanContainer();
    let container = d3.select('#donutContainer');
    createDonutChart(container, '球隊得分', setNumber, teamScores);
    createDonutChart(container, '攻擊次數', setNumber, teamAttack);
    createDonutChart(container, '攔網次數', setNumber, teamBlock);
    container = d3.select('#lineContainer');
    drawLine(container, setNumber, dataSet);
}

function createDonutChart(container, title, setNumber, data) {
    let width = 175;
    let height = 175;
    let margin = 20;
    let radius = Math.min(width, height) / 2 - margin;

    let chartContainer = container.append('div').attr('class', 'chart');

    let svg = chartContainer
        .append('svg')
        .attr('width', width)
        .attr('height', height)
        .append('g')
        .attr('transform', `translate(${width / 2}, ${height / 1.8})`);
    
    let leftColor = (global.LREachRound.left[setNumber-1] == leftTeamID) ? "red" : "blue";
    let rightColor = (leftColor == "red") ? "blue" : "red";
    let color = d3.scaleOrdinal()
        .domain("teamL", "teamR")
        .range([leftColor, rightColor]);

    let teamNames = setNumber === 1 || setNumber === 3 || setNumber === 5 ?
        { teamL: global.teamL, teamR: global.teamR } :
        { teamL: global.teamR, teamR: global.teamL };
    let pie = d3.pie().value(d => d.value);
    let data_ready = pie(d3.entries(data));

    svg .selectAll('whatever')
        .data(data_ready)
        .enter()
        .append('path')
        .attr('d', d3.arc()
            .innerRadius(radius * 0.5)
            .outerRadius(radius))
        .attr('fill', d => color(d.data.key))
        .attr("stroke", "white")
        .style("stroke-width", "2px")
        .style("opacity", 0.7);

    svg .selectAll('whatever')
        .data(data_ready)
        .enter()
        .append('text')
        .text(d => teamNames[d.data.key] + ': ' + d.data.value)
        .attr("transform", d => `translate(${d3.arc().innerRadius(radius * 0.5).outerRadius(radius).centroid(d)})`)
        .style("text-anchor", "middle")
        .style("font-size", '14px');

    svg .append('text')
        .attr('x', 0)
        .attr('y', 0 - (height / 2) + margin / 2)
        .attr('text-anchor', 'middle')
        .style('font-size', '16px')
        .style('text-decoration', 'underline')
        .text(title);
}

function drawLine(container, setNumber, dataSet) {
    let teamEachScores = [{ left: 0, right: 0, total: 0 }];
    let count_left = 0;
    let count_right = 0;
    let total = 0;
    dataSet.forEach(function(record) {
        if (record.score !== 0) {
            if (record.score === 1)
                count_left += 1;
            else if (record.score === -1)
                count_right += 1;
            total = count_left + count_right;
            teamEachScores.push({ left: count_left, right: count_right, total: total });
        }
    });

    let width = 700;
    let height = 450;
    let margin = { top: 30, right: 30, bottom: 30, left: 50 };

    let chartContainer = container.append('div').attr('class', 'chart');
    let svg = chartContainer
        .append('svg')
        .attr('width', width + margin.left + margin.right)
        .attr('height', height + margin.top + margin.bottom)
        .append('g')
        .attr('transform', `translate(${margin.left}, ${margin.top + 10})`);

    let leftScores = teamEachScores.map(item => item.left);
    let rightScores = teamEachScores.map(item => item.right);

    let xScale = d3.scaleLinear()
        .domain([0, teamEachScores.length - 1])
        .range([0, width]);

    let yScale = d3.scaleLinear()
        .domain([0, Math.max(teamEachScores[teamEachScores.length - 1].left, teamEachScores[teamEachScores.length - 1].right)])
        .range([height, 0]);

    let lineLeft = d3.line()
        .x((d, i) => xScale(i))
        .y(d => yScale(d));

    let lineRight = d3.line()
        .x((d, i) => xScale(i))
        .y(d => yScale(d));
    
    let leftColor = (global.LREachRound.left[setNumber-1] == leftTeamID) ? "blue" : "red";
    let rightColor = (leftColor == "blue") ? "red" : "blue";

    let tooltip = d3.select('body').append('div')
        .attr('class', 'tooltip')
        .style('position', 'absolute')
        .style('padding', '6px')
        .style('background', 'rgba(0, 0, 0, 0.6)')
        .style('color', '#fff')
        .style('border-radius', '4px')
        .style('font-size', '12px')
        .style('visibility', 'hidden');

    function rightAngleLine(data) {
        let path = '';
        for(let i = 0; i < data.length - 1; i++) {
            const x1 = xScale(i);
            const y1 = yScale(data[i]);
            const x2 = xScale(i + 1);
            const y2 = yScale(data[i + 1]);
            path += `M${x1},${y1}H${x2}V${y2}`;
        }
        return path;
    }

    svg.append('path')
        .datum(leftScores)
        .attr('class', 'line')
        .attr('d', function(d) { return rightAngleLine(d); })
        .attr('fill', 'none')
        .attr('stroke', leftColor)
        .attr('stroke-width', 2)
        .style('opacity', 0.65);

    svg.append('path')
        .datum(rightScores)
        .attr('class', 'line')
        .attr('d', function(d) { return rightAngleLine(d); })
        .attr('fill', 'none')
        .attr('stroke', rightColor)
        .attr('stroke-width', 2)
        .style('opacity', 0.65);

    svg.selectAll('.helper-line-left')
        .data(leftScores)
        .enter().append('line')
        .attr('class', 'helper-line-left')
        .attr('x1', 0)
        .attr('y1', d => yScale(d))
        .attr('x2', width)
        .attr('y2', d => yScale(d))
        .style('stroke', 'lightgray')
        .style('stroke-dasharray', '3,3');

    svg.append('g')
        .attr('transform', `translate(0, ${height})`)
        .call(d3.axisBottom(xScale));

    svg.append('g')
        .call(d3.axisLeft(yScale));

    svg.append('text')
        .attr('x', width / 2)
        .attr('y', -margin.top / 2)
        .attr('text-anchor', 'middle')
        .style('font-size', '18px')
        .text(`第${setNumber}局比分`);

    svg.selectAll('.dot-right')
        .data(rightScores)
        .enter()
        .append('circle')
        .attr('class', 'dot')
        .attr('cx', (d, i) => xScale(i))
        .attr('cy', d => yScale(d))
        .attr('r', 4)
        .attr('fill', rightColor)
        .on('mouseover', (d, i) => {
            tooltip.text(`比分: ${leftScores[i]} : ${d}`);
            tooltip.style('visibility', 'visible');
        })
        .on('mousemove', () => {
            tooltip.style('top', `${d3.event.pageY - 10}px`);
            tooltip.style('left', `${d3.event.pageX + 10}px`);
        })
        .on('mouseout', () => {
            tooltip.style('visibility', 'hidden');
        });

    svg.selectAll('.dot-left')
        .data(leftScores)
        .enter()
        .append('circle')
        .attr('class', 'dot')
        .attr('cx', (d, i) => xScale(i))
        .attr('cy', d => yScale(d))
        .attr('r', 4)
        .attr('fill', leftColor)
        .on('mouseover', (d, i) => {
            tooltip.text(`比分: ${d} : ${rightScores[i]}`);
            tooltip.style('visibility', 'visible');
        })
        .on('mousemove', () => {
            tooltip.style('top', `${d3.event.pageY - 10}px`);
            tooltip.style('left', `${d3.event.pageX + 10}px`);
        })
        .on('mouseout', () => {
            tooltip.style('visibility', 'hidden');
        });
}

function updateCounts(counts, condition, record, player) {
    if(condition && record[player] !== -1) {
        let playerStr = record[player].toString();
        if (!counts[playerStr]) {
            counts[playerStr] = { "-1": 0, "1": 0, "2": 0, "3": 0 };
        }
        counts[playerStr][record.behavior.toString()]++;
    }
}

function playerTableData(setNumber, dataSet) {
    let behaviorCounts = {};
    let successCounts = {};
    let falseCounts = {};
    let numList = {};
    dataSet.forEach(record => {
        ['player1', 'player2', 'player3'].forEach(player => {
            updateCounts(behaviorCounts, true, record, player);
            updateCounts(successCounts, (record.TeamID === 2 && record.score === -1) || (record.TeamID === 1 && record.score === 1), record, player);
            updateCounts(falseCounts, (record.TeamID === 1 && record.score === -1) || (record.TeamID === 2 && record.score === 1), record, player);
            if (player === 'player1' && !numList.hasOwnProperty(record.player1))
                numList[record.player1] = record.player1;
        });
    });

    Object.keys(behaviorCounts).forEach(player => {
        if (!successCounts[player])
            successCounts[player] = { "-1": 0, "1": 0, "2": 0, "3": 0 };
        if (!falseCounts[player])
            falseCounts[player] = { "-1": 0, "1": 0, "2": 0, "3": 0 };
    });

    drawPlayerTable(setNumber, numList, behaviorCounts, successCounts, falseCounts, dataSet);
}

function drawPlayerTable(setNumber, numList, behaviorCounts, successCounts, falseCounts, dataSet) {
    cleanContainer();

    d3.select('#playerDataContainer').append('div')
        .attr('class', 'table-title')
        .style('font-size', '24px')
        .style('text-align', 'center')
        .text(`第${setNumber}局 選手數據圖表`);
    
    let table = d3.select('#playerDataContainer').append('table');
    let thead = table.append('thead');
    let tbody = table.append('tbody');
    let columns = ['Player', '發球', '攻擊', '攔網', '其他'];

    thead.append('tr')
        .selectAll('th')
        .data(columns)
        .enter()
        .append('th')
        .text(d => d)
        .attr('class', 'sortable')
        .on('click', function(event, d) {
            let typeMap = { 'Player': '0', '發球': '-1', '攻擊': '2', '攔網': '3', '其他': '1' };
            let type = typeMap[event];

            if(type == 0)
                playerTableData(setNumber, dataSet);
            else {
                for(let player1 in behaviorCounts) {
                    for(let player2 in behaviorCounts) {
                        if(behaviorCounts[player1][type] > behaviorCounts[player2][type]) {
                            [behaviorCounts[player1], behaviorCounts[player2]] = [behaviorCounts[player2], behaviorCounts[player1]];
                            [successCounts[player1], successCounts[player2]] = [successCounts[player2], successCounts[player1]];
                            [falseCounts[player1], falseCounts[player2]] = [falseCounts[player2], falseCounts[player1]];
                            [numList[player1], numList[player2]] = [numList[player2], numList[player1]];
                        }
                    }
                }
                
                drawPlayerTable(setNumber, numList, behaviorCounts, successCounts, falseCounts, dataSet);
            }
        });

    let maxBehaviorCount = 0;
    let behaviorType = { "-1": 0, "1": 0, "2": 0, "3": 0 };
    for(let player in behaviorCounts) {
        for(let type in behaviorType) {
            if(behaviorCounts[player][type] > maxBehaviorCount)
                maxBehaviorCount = behaviorCounts[player][type];
        }
    }

    let rows = tbody.selectAll('tr')
        .data(Object.entries(behaviorCounts))
        .enter()
        .append('tr');

    let tmpList = {...numList}, playerTeam = [], index = 0;
    rows.selectAll('td')
        .data((d, index) => {
            let playerName;
            while(tmpList[index] === undefined)
                index++;
            let playerNum = tmpList[index];
            tmpList[index] = undefined;
            
            let totalBehavior = d[1];
            let successBehavior = successCounts[index] || { "-1": 0, "1": 0, "2": 0, "3": 0 };

            [...global.players.left, ...global.players.right].forEach(playerObj => {
                if(playerObj.num == playerNum) {
                    playerName = playerObj.name;
                    playerTeam.push(global.players.left.includes(playerObj) ? 'left' : 'right');
                }
            });

            let successRate = type => {
                let total = totalBehavior[type];
                let success = successBehavior[type];
                return total ? `${success}/${total}` : '0/0';
            };

            return [
                `${playerName} (${playerNum})`,
                successRate("-1"),
                successRate("2"),
                successRate("3"),
                successRate("1")
            ];
        })
        .enter()
        .append('td')
        .html((d, i) => {
            if(d.includes('/')) {
                let [success, total] = d.split('/').map(Number);
                let maxTotal = maxBehaviorCount;
                let percentage = total ? (total / maxTotal) * 100 : 0;
                let color = playerTeam[index] === 'left' ? 'lightblue' : 'salmon';
                if(i == 4) index++;
                return `<div style="position: relative; width: 100%; height: 20px; background-color: white;">
                <div style="position: absolute; width: ${percentage}%; height: 100%; background-color: ${color};"></div>
                <div style="position: absolute; width: 100%; text-align: center;">${d}</div>
                        </div>`;
            } else {
                return d;
            }
        });
}

let clickId = 0, before = -1;
function drawScatter(setNumber, dataSet) {
    cleanContainer();
    let container = d3.select('#scatterContainer');
    let svg = container.append('svg')
        .attr('height', 550)
        .attr('width', 800)
        .attr('class', 'chart');
    
    let leftTeamID = global.LREachRound.left[setNumber-1];
    let rightTeamID = global.LREachRound.right[setNumber-1];
    let teamNames = { teamL: global.teamL, teamR: global.teamR };

    const colorScale = d3.scaleOrdinal()
        .domain([leftTeamID, rightTeamID, leftTeamID+2, rightTeamID+2])
        .range(["blue", "red", d3.interpolateBlues(0.2), d3.interpolateReds(0.2)]);

    let x = 45, y = 25, xCount = 0, yCount = 0, maxY = 0;
    dataSet.forEach((record, index) => {
        if(clickId != 0 && dataSet[index].player1 != clickId)
            dataSet[index].TeamID +=2;

        const getTooltipContent = () => {
            let behaviorText, playerName;
            switch (record.behavior) {
                case -1:
                    behaviorText = "發球";
                    break;
                case 1:
                    behaviorText = "接球";
                    break;
                case 2:
                    behaviorText = "進攻";
                    break;
                case 3:
                    behaviorText = "攔網";
                    break;
                default:
                    behaviorText = "其他";
            }
            [...global.players.left, ...global.players.right].forEach(playerObj => {
                if(playerObj.num == record.player1)
                    playerName = playerObj.name;
            });
            return `球員: ${playerName}<br/>
                    背號: ${record.player1}<br/>
                    Behavior: ${behaviorText}`;
        };
        
        const tip = d3.tip()
                      .attr("class", "d3-tip tooltip")
                      .html(getTooltipContent)
                      .offset([-10, 0]);
    
        svg.call(tip);
        svg.append("circle")
            .attr("cx", x)
            .attr("cy", 500 - y)
            .attr("r", 6)
            .attr("fill", colorScale(record.TeamID))
            .on("mouseover", tip.show)
            .on("mouseout", tip.hide)
            .on("click", function() {
                tip.hide();
                clickId = record.player1;
                if(clickId == before) {
                    clickId = 0;
                    before = -1;
                }
                if(dataSet.length !== 0)
                    drawScatter(setNumber, dataSet);
                before = clickId;
            });
    
        if (record.score !== 0) {
            x += 25;
            y = 25;
            xCount++;
            if(yCount > maxY)
                maxY = yCount;
            yCount = 0;
        } else {
            y += 25;
            yCount++;
        }

        if(clickId != 0 && dataSet[index].player1 != clickId)
            dataSet[index].TeamID -=2;
    });

    maxY += 1;
    svg.attr('width', x);
    const xScale = d3.scaleLinear()
        .domain([0, xCount])
        .range([0, x-45]);
    
    const yScale = d3.scaleLinear()
        .domain([0, maxY])
        .range([maxY*25, 0]);

    svg.append("g")
        .attr("transform", `translate(20, 500)`)
        .call(d3.axisBottom(xScale).ticks(Math.floor(x / 25)));

    svg.append("g")
        .attr("transform", `translate(20, ${500-maxY*25})`)
        .call(d3.axisLeft(yScale).ticks(Math.floor(maxY)));
    
    svg.append("text")
        .attr("x", x/2)
        .attr("y", 20)
        .attr("text-anchor", "middle")
        .style("font-size", "24px")
        .text(`第${setNumber}局 球賽歷程圖表`);

    svg.append("circle")
        .attr("cx", x/2+150)
        .attr("cy", 14)
        .attr("r", 6)
        .attr("fill", "blue");

    svg.append("circle")
        .attr("cx", x/2+240)
        .attr("cy", 14)
        .attr("r", 6)
        .attr("fill", "red");

    svg.append("text")
        .attr("x", x/2+160)
        .attr("y", 15)
        .text(`${teamNames.teamL}`)
        .style("font-size", "14px")
        .attr("alignment-baseline","middle");

    svg.append("text")
        .attr("x", x/2+250)
        .attr("y", 15)
        .text(`${teamNames.teamR}`)
        .style("font-size", "14px")
        .attr("alignment-baseline","middle");
}

function cleanContainer() {
    const donutContainer = d3.select('#donutContainer');
    donutContainer.html('');
    const lineContainer = d3.select('#lineContainer');
    lineContainer.html('');
    const playerDataContainer = d3.select('#playerDataContainer');
    playerDataContainer.html('');
    const scatterContainer = d3.select('#scatterContainer');
    scatterContainer.html('');
}

const setButtons = document.querySelectorAll('.setbutton');
const graphButtons = document.querySelectorAll('.graphbutton');
setButtons.forEach(button => {
    button.addEventListener('click', () => {
        setButtons.forEach(btn => btn.classList.remove('selected'));
        button.classList.add('selected');
    });
});
graphButtons.forEach(button => {
    button.addEventListener('click', () => {
        graphButtons.forEach(btn => btn.classList.remove('selected'));
        button.classList.add('selected');
    });
});


