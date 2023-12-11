using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;
public class DataLineChart : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
        var chart = gameObject.GetComponent<BarChart>();
        if (chart == null){
            chart = gameObject.AddComponent<BarChart>();
            chart.Init();
        }
        var title = chart.EnsureChartComponent<Title>();
        title.text = "Simple Bar";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
