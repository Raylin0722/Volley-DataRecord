using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class SystemData : MonoBehaviour
{
    public string formation; // 紀錄當前陣容
    [SerializeField] public GameObject[] leftPlayers; // 左方球員
    [SerializeField] public GameObject[] rightPlayers; // 右方球員
    public int[,] leftLibero;
    public int[,] rightLibero;
    [SerializeField] public GameObject[] changeLeftPlayers;
    [SerializeField] public GameObject[] changeRightPlayers;
    [SerializeField] public GameObject gameView;
    [SerializeField] public GameObject deleteNewData;
    public Vector3[,] leftStartCatchPos = new Vector3[6,6]{{new Vector3(318.7087f, 580.1398f, 0f),  new Vector3(320.0315f, 580.1104f, 0f),  new Vector3(322.7357f, 582.462f, 0f),  new Vector3(321.0015f, 583.9317f, 0f),  new Vector3(319.9433f, 582.9029f, 0f),  new Vector3(320.3842f, 581.3156f, 0f)}, 
                                                           {new Vector3(319.5905f, 580.1839f, 0f),  new Vector3(322.7064f, 580.3015f, 0f),  new Vector3(320.5605f, 583.1821f, 0f),  new Vector3(321.9715f, 583.9758f, 0f),  new Vector3(318.8851f, 582.4473f, 0f),  new Vector3(319.9493f, 581.536f, 0f)},
                                                           {new Vector3(319.7963f, 580.3015f, 0f),  new Vector3(322.3536f, 580.5072f, 0f),  new Vector3(322.2948f, 582.1827f, 0f),  new Vector3(320.3842f, 583.9464f, 0f),  new Vector3(319.5023f, 582.5355f, 0f),  new Vector3(318.7675f, 581.2715f, 0f)},
                                                           {new Vector3(318.8557f, 580.0516f, 0f),  new Vector3(319.7963f, 582.8441f, 0f),  new Vector3(321.7951f, 583.6378f, 0f),  new Vector3(322.8239f, 584.1668f, 0f),  new Vector3(320.3548f, 581.7859f, 0f),  new Vector3(320.0902f, 580.7571f, 0f)},
                                                           {new Vector3(319.2672f, 580.3896f, 0f),  new Vector3(321.266f, 580.3602f, 0f),  new Vector3(320.1784f, 582.8588f, 0f),  new Vector3(322.383f, 584.0052f, 0f),  new Vector3(321.7069f, 583.0352f, 0f),  new Vector3(319.7081f, 581.5654f, 0f)},
                                                           {new Vector3(319.3554f, 580.1545f, 0f),  new Vector3(321.6776f, 580.4778f, 0f),  new Vector3(322.9415f, 581.3597f, 0f),  new Vector3(320.3842f, 583.7994f, 0f),  new Vector3(319.032f, 582.4178f, 0f),  new Vector3(321.6482f, 581.8594f, 0f)}};
    public Vector3[,] rightStartCatchPos = {{new Vector3(328.1149f, 584.0493f, 0f),  new Vector3(326.6452f, 583.9905f, 0f),  new Vector3(324.2349f, 582.4326f, 0f),  new Vector3(325.9397f, 580.2867f, 0f),  new Vector3(327.1743f, 581.2862f, 0f),  new Vector3(327.2331f, 582.7265f, 0f)},
                                            {new Vector3(327.7622f, 583.917f, 0f),  new Vector3(324.5582f, 584.0345f, 0f),  new Vector3(325.9691f, 581.1833f, 0f),  new Vector3(324.47f, 580.1839f, 0f),  new Vector3(327.968f, 581.7124f, 0f),  new Vector3(326.851f, 582.7118f, 0f)},
                                            {new Vector3(327.3801f, 583.8141f, 0f),  new Vector3(324.7346f, 583.9317f, 0f),  new Vector3(323.9997f, 582.991f, 0f),  new Vector3(326.2043f, 580.4044f, 0f),  new Vector3(326.851f, 581.7565f, 0f),  new Vector3(328.0855f, 582.462f, 0f)},
                                            {new Vector3(328.3207f, 583.7406f, 0f),  new Vector3(326.3219f, 581.3008f, 0f),  new Vector3(324.5288f, 581.0951f, 0f),  new Vector3(323.9409f, 580.0075f, 0f),  new Vector3(327.0567f, 582.2121f, 0f),  new Vector3(327.1155f, 583.3291f, 0f)},
                                            {new Vector3(328.1737f, 583.1527f, 0f),  new Vector3(326.1455f, 583.917f, 0f),  new Vector3(326.7628f, 581.1833f, 0f),  new Vector3(323.9703f, 580.1839f, 0f),  new Vector3(325.0579f, 580.566f, 0f),  new Vector3(327.3801f, 582.2415f, 0f)},
                                            {new Vector3(328.1149f, 584.0493f, 0f),  new Vector3(324.8521f, 583.623f, 0f),  new Vector3(323.9115f, 582.6824f, 0f),  new Vector3(326.3219f, 580.0663f, 0f),  new Vector3(326.998f, 581.5948f, 0f),  new Vector3(324.8815f, 582.2415f, 0f)}};
    public Vector3[] leftStartServePos = {new Vector3(317.7997f, 580.4063f, 0f),  new Vector3(322.7207f, 580.9407f, 0f),  new Vector3(322.7429f, 581.9872f, 0f),  new Vector3(322.7652f, 583.0338f, 0f),  new Vector3(319.5365f, 583.5237f, 0f),  new Vector3(320.0041f, 582.0763f, 0f)};
    public Vector3[] rightStartServePos = {new Vector3(328.9554f, 584.058f, 0f),  new Vector3(324.0344f, 583.0338f, 0f),  new Vector3(324.0789f, 582.0095f, 0f),  new Vector3(324.1235f, 581.0075f, 0f),  new Vector3(327.7084f, 580.5399f, 0f),  new Vector3(326.8623f, 581.9427f, 0f)};
    public Vector3[] leftGamePos;
    public Vector3[] rightGamePos;
    public int[] point; // 小分
    public int[] score; // 大分
    public Text leftScoreText; // 左方分數文字
    public Text rightScoreText; // 右方分數文字
    public Text leftPointText;
    public Text rightPointText;
    public Text leftTeamName;
    public Text rightTeamName;
    public int whoWin;
    public int setServe;
    public List<dealDB.Data> saveData; // 資料儲存
    public bool changePosition; //更換位子變數判斷
    public int leftRight;
    public int[] leftTeamNum, rightTeamNum;
    public bool changePlayer;
    public bool Clibero;
    public int[] liberoTarget;
    public bool[] liberoHasChange;
    public int[] CformationTmp;
    public int nameMode;
    public Text[] leftLiberoName;
    public Text[] rightLiberoName;
    public List<string> leftOption;
    public List<string> rightOption;
    public dragPlayer[] leftDrag;
    public dragPlayer[] rightDrag;

        
    void Start(){
        
        score = new int[2];
        point = new int[2];
        leftScoreText.text = "0";
        rightScoreText.text = "0";
        leftPointText.text = "00";
        rightPointText.text = "00";
        saveData = new List<dealDB.Data>();
        leftRight = UserData.Instance.leftRight;
        leftTeamNum = new int[1];
        leftTeamNum[0] = UserData.Instance.leftTeamNum;
        rightTeamNum = new int[1];
        rightTeamNum[0] = UserData.Instance.rightTeamNum;
        leftGamePos = new Vector3[6]{new Vector3(-1f, -1f, -1f), new Vector3(-1f, -1f, -1f), new Vector3(-1f, -1f, -1f), new Vector3(-1f, -1f, -1f), new Vector3(-1f, -1f, -1f), new Vector3(-1f, -1f, -1f)};
        rightGamePos = new Vector3[6]{new Vector3(-1f, -1f, -1f), new Vector3(-1f, -1f, -1f), new Vector3(-1f, -1f, -1f), new Vector3(-1f, -1f, -1f), new Vector3(-1f, -1f, -1f), new Vector3(-1f, -1f, -1f)};
        
        changePosition = false;
        changePlayer = false;
        Clibero = false; // 不自動換
        liberoHasChange = new bool[2]{false, false};
        CformationTmp = new int[2]{-1, -1};
        leftLibero = new int[2, 3];
        rightLibero = new int[2, 3];
        nameMode = 0;

        leftOption = new List<string>();
        rightOption = new List<string>();

        if(leftRight == 0){ // 使用者 left 0 right 1
            leftTeamName.text = UserData.Instance.UserTeamName;
            rightTeamName.text = UserData.Instance.EnemyTeamName;
            for(int i = 0 ; i < 6; i++){
                leftPlayers[i].GetComponent<dragPlayer>().playerName = UserData.Instance.UserPlayerName[i];
                rightPlayers[i].GetComponent<dragPlayer>().playerName = UserData.Instance.EnemyPlayerName[i];
                leftPlayers[i].GetComponent<dragPlayer>().playerNum = UserData.Instance.UserPlayerNumber[i].ToString();
                rightPlayers[i].GetComponent<dragPlayer>().playerNum = UserData.Instance.EnemyPlayerNumber[i].ToString();
                leftPlayers[i].GetComponent<dragPlayer>().playerPlayPos = UserData.Instance.UserPlayerPlayPos[i];
                rightPlayers[i].GetComponent<dragPlayer>().playerPlayPos = UserData.Instance.EnemyPlayerPlayPos[i];

                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().playerName = UserData.Instance.UserPlayerName[i + 6];
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().playerName = UserData.Instance.EnemyPlayerName[i + 6];
                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().playerNum = UserData.Instance.UserPlayerNumber[i + 6].ToString();
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().playerNum = UserData.Instance.EnemyPlayerNumber[i + 6].ToString();
                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().playerPlayPos = UserData.Instance.UserPlayerPlayPos[i + 6];
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().playerPlayPos = UserData.Instance.EnemyPlayerPlayPos[i + 6];
                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().FirstPlayer = false;
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().FirstPlayer = false;
                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().timeOfChange = 1;
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().timeOfChange = 1;
                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().changeTarget = -1;
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().changeTarget = -1;
                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().side = new int[1]{0};
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().side = new int[1]{1};

            }
            int lc= 0, rc = 0;
            for(int i = 0 ; i < 12; i++){
                if(UserData.Instance.UserPlayerPlayPos[i] == 4 && lc < 2){
                    leftLibero[lc, 0] = UserData.Instance.UserPlayerNumber[i];
                    leftLiberoName[lc].text = UserData.Instance.UserPlayerName[i];
                    leftLibero[lc, 1] = 0;
                    leftLibero[lc, 2] = 0;
                    lc++;
                }
                if(UserData.Instance.EnemyPlayerPlayPos[i] == 4 && rc < 2){
                    rightLibero[rc, 0] = UserData.Instance.EnemyPlayerNumber[i];
                    rightLiberoName[rc].text = UserData.Instance.EnemyPlayerName[i];
                    rightLibero[rc, 1] = 0;
                    rightLibero[rc, 2] = 0;
                    rc++;
                }
                if(rc>=2 && lc >=2)
                    break;
            }
            print(lc);
            print(rc);
            for(int i = 1; i >= lc; i--){
                leftLiberoC[2*i].interactable = false;
                leftLiberoC[2*i+1].interactable = false;
                leftLiberoName[i].text = "未設定";
            }
            
            for(int i = 1; i >= rc; i--){
                rightLiberoC[2*i].interactable = false;
                rightLiberoC[2*i+1].interactable = false;
                rightLiberoName[i].text = "未設定";
            }

            leftOption.Clear();
            rightOption.Clear();

            leftOption.Add("未設定");
            rightOption.Add("未設定");

            for(int i = 0; i < 6; i++){
                if(UserData.Instance.UserPlayerPlayPos[i] != 4)
                    leftOption.Add(UserData.Instance.UserPlayerName[i]);
                if(UserData.Instance.EnemyPlayerPlayPos[i] != 4)
                    rightOption.Add(UserData.Instance.EnemyPlayerName[i]);
            }
            
            for(int i = 0; i < 4; i++){
                leftLiberoC[i].ClearOptions();
                rightLiberoC[i].ClearOptions();
                leftLiberoC[i].AddOptions(leftOption);
                rightLiberoC[i].AddOptions(rightOption);
            }
            
        }
        else{
            rightTeamName.text = UserData.Instance.UserTeamName;
            leftTeamName.text = UserData.Instance.EnemyTeamName;
            for(int i = 0 ; i < 6; i++){
                rightPlayers[i].GetComponent<dragPlayer>().playerName = UserData.Instance.UserPlayerName[i];
                leftPlayers[i].GetComponent<dragPlayer>().playerName = UserData.Instance.EnemyPlayerName[i];
                rightPlayers[i].GetComponent<dragPlayer>().playerNum = UserData.Instance.UserPlayerNumber[i].ToString();
                leftPlayers[i].GetComponent<dragPlayer>().playerNum = UserData.Instance.EnemyPlayerNumber[i].ToString();
                rightPlayers[i].GetComponent<dragPlayer>().playerPlayPos = UserData.Instance.UserPlayerPlayPos[i];
                leftPlayers[i].GetComponent<dragPlayer>().playerPlayPos = UserData.Instance.EnemyPlayerPlayPos[i];

                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().playerName = UserData.Instance.EnemyPlayerName[i + 6];
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().playerName = UserData.Instance.UserPlayerName[i + 6];
                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().playerNum = UserData.Instance.EnemyPlayerNumber[i + 6].ToString();
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().playerNum = UserData.Instance.UserPlayerNumber[i + 6].ToString();
                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().playerPlayPos = UserData.Instance.EnemyPlayerPlayPos[i + 6];
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().playerPlayPos = UserData.Instance.UserPlayerPlayPos[i + 6];
                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().FirstPlayer = false;
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().FirstPlayer = false;
                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().timeOfChange = 1;
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().timeOfChange = 1;
                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().changeTarget = -1;
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().changeTarget = -1;
                changeLeftPlayers[i].GetComponent<dragPlayerToChange>().side = new int[1]{0};
                changeRightPlayers[i].GetComponent<dragPlayerToChange>().side = new int[1]{1};
            }
            int lc= 0, rc = 0;
            for(int i = 0 ; i < 12; i++){
                if(UserData.Instance.UserPlayerPlayPos[i] == 4 && rc < 2){
                    rightLibero[rc, 0] = UserData.Instance.UserPlayerNumber[i];
                    rightLiberoName[rc].text = UserData.Instance.UserPlayerName[i];
                    rightLibero[rc, 1] = 0;
                    rightLibero[rc, 2] = 0;
                    rc++;
                }
                if(UserData.Instance.EnemyPlayerPlayPos[i] == 4 && lc < 2){
                    leftLibero[rc, 0] = UserData.Instance.EnemyPlayerNumber[i];
                    leftLiberoName[lc].text = UserData.Instance.EnemyPlayerName[i];
                    leftLibero[lc, 1] = 0;
                    leftLibero[lc, 2] = 0;
                    lc++;
                }
                if(rc>=2 && lc >=2)
                    break;
            }
            for(int i = 1; i >= lc; i--){
                leftLiberoC[2*i].interactable = false;
                leftLiberoC[2*i+1].interactable = false;
                leftLiberoName[i].text = "未設定";
            }
            for(int i = 1; i >= rc; i--){
                rightLiberoC[2*i].interactable = false;
                rightLiberoC[2*i+1].interactable = false;
                rightLiberoName[i].text = "未設定";
            }
            leftOption.Clear();
            rightOption.Clear();
            
            leftOption.Add("未設定");
            rightOption.Add("未設定");

            for(int i = 0; i < 6; i++){
                if(UserData.Instance.UserPlayerPlayPos[i] != 4)
                    rightOption.Add(UserData.Instance.UserPlayerName[i]);
                if(UserData.Instance.EnemyPlayerPlayPos[i] != 4)
                    leftOption.Add(UserData.Instance.EnemyPlayerName[i]);
            }
            for(int i = 0; i < 4; i++){
                leftLiberoC[i].ClearOptions();
                rightLiberoC[i].ClearOptions();
                leftLiberoC[i].AddOptions(leftOption);
                rightLiberoC[i].AddOptions(rightOption);
            }

        }
        leftDrag = new dragPlayer[6];
        rightDrag = new dragPlayer[6];
        for(int i = 0; i < 6; i++){
            leftDrag[i] = leftPlayers[i].GetComponent<dragPlayer>();
            rightDrag[i] = rightPlayers[i].GetComponent<dragPlayer>();
        }
    }

    public void changePlayerPos(){
        if(!changePosition)
            changePosition = true;
        else{
            changePosition = false;
        }
    }

    public void Cformation(){
        if(!changePlayer){
            changePlayer = true;
            for(int i = 0; i < 6; i++){
                changeLeftPlayers[i].SetActive(true);
                changeRightPlayers[i].SetActive(true);
            }
            gameView.SetActive(false);
            deleteNewData.SetActive(false);
        }
        else{
            changePlayer = false;
            for(int i = 0; i < 6; i++){
                changeLeftPlayers[i].SetActive(false);
                changeRightPlayers[i].SetActive(false);
            }
            gameView.SetActive(true);
            deleteNewData.SetActive(true);
        }
    }
    public Text ChangeNameBtn;
    public void changeNameMode(){
        nameMode = 1 - nameMode;
        ChangeNameBtn.text = nameMode == 0 ? "名稱" : "背號";
        for(int i = 0; i < 6; i++){
            leftPlayers[i].GetComponent<dragPlayer>().updata[0] = false;
            rightPlayers[i].GetComponent<dragPlayer>().updata[0] = false;
        }
        return;
    }

    [SerializeField] public GameObject Main;
    [SerializeField] public GameObject Setting;

    public void startSetting(){
        Main.SetActive(false);
        Setting.SetActive(true);
    }
    public void exitSetting(){
        Main.SetActive(true);
        Setting.SetActive(false);
        if(lockingL == true){
            for(int i = 0; i < 4; i++){
                leftLiberoC[i].interactable = false;
            }
        }
        else{
            for(int i = 0; i < 4; i++){
                leftLiberoC[i].interactable = true;
            }
        }

        if(lockingR == true){
            for(int i = 0; i < 4; i++){
                rightLiberoC[i].interactable = false;
            }
        }
        else{
            for(int i = 0; i < 4; i++){
                rightLiberoC[i].interactable = true;
            }
        }
    }
    public Dropdown[] leftLiberoC;
    public Dropdown[] rightLiberoC;

    public void SetChangeLibero(){

        for(int i = 0; i < 2; i++){
            if(leftLiberoC[2*i].options[leftLiberoC[2*i].value].text == "未設定"){
                leftLibero[i, 1] = 0;
            }
            else{
                for(int j = 0; j < 6; j++){
                    if(leftDrag[j].playerName == leftLiberoC[2*i].options[leftLiberoC[2*i].value].text)
                        leftLibero[i, 1] = Int32.Parse(leftDrag[j].playerNum);
                }
            }

            if(leftLiberoC[2*i+1].options[leftLiberoC[2*i+1].value].text == "未設定"){
                leftLibero[i, 2] = 0;
            }
            else{
                for(int j = 0; j < 6; j++){
                    if(leftDrag[j].playerName == leftLiberoC[2*i+1].options[leftLiberoC[2*i+1].value].text)
                        leftLibero[i, 2] = Int32.Parse(leftDrag[j].playerNum);
                }
            }

            if(rightLiberoC[2*i].options[rightLiberoC[2*i].value].text == "未設定"){
                rightLibero[i, 1] = 0;
            }
            else{
                for(int j = 0; j < 6; j++){
                    if(rightDrag[j].playerName == rightLiberoC[2*i].options[rightLiberoC[2*i].value].text)
                        rightLibero[i, 1] = Int32.Parse(rightDrag[j].playerNum);
                }
                
            }

            if(rightLiberoC[2*i+1].options[rightLiberoC[2*i+1].value].text == "未設定"){
                rightLibero[i, 2] = 0;
            }
            else{
                for(int j = 0; j < 6; j++){
                    if(rightDrag[j].playerName == rightLiberoC[2*i+1].options[rightLiberoC[2*i+1].value].text)
                        rightLibero[i, 2] = Int32.Parse(rightDrag[j].playerNum);
                }
            }
        }
    }
    public void changeLiberoChoice(Dropdown[] dropdowns, int leftOrRight){
        string[] select = new string[dropdowns.Length];
        List<string> newOption;
        if(leftOrRight == 0)
            newOption = new List<string>(leftOption);
        else
            newOption = new List<string>(rightOption);
        

        for(int i = 0; i < dropdowns.Length; i++){
            select[i] = dropdowns[i].options[dropdowns[i].value].text;
            if(select[i] != "未設定" && newOption.Contains(select[i])){
                newOption.Remove(select[i]);
            }
        }

        for(int i = 0; i < dropdowns.Length; i++){
            dropdowns[i].ClearOptions();
            List<string> tmpOption = new List<string>(newOption);
            if(select[i] != "未設定"){
                tmpOption.Add(tmpOption[0]);
                tmpOption[0] = select[i];
            }
                
            dropdowns[i].AddOptions(tmpOption);
            dropdowns[i].value = 0;
        }
        
    }
    public void SetLibero(){
        if(leftRight == 0){
            
            leftOption.Clear();
            rightOption.Clear();

            leftOption.Add("未設定");
            rightOption.Add("未設定");

            for(int i = 0; i < 6; i++){
                if(UserData.Instance.UserPlayerPlayPos[i] != 4)
                    leftOption.Add(UserData.Instance.UserPlayerName[i]);
                if(UserData.Instance.EnemyPlayerPlayPos[i] != 4)
                    rightOption.Add(UserData.Instance.EnemyPlayerName[i]);
            }

            print(leftOption.Count);
        }
        else{
            
            leftOption.Clear();
            rightOption.Clear();

            leftOption.Add("未設定");
            rightOption.Add("未設定");

            for(int i = 0; i < 6; i++){
                if(UserData.Instance.UserPlayerPlayPos[i] != 4)
                    rightOption.Add(UserData.Instance.UserPlayerName[i]);
                if(UserData.Instance.EnemyPlayerPlayPos[i] != 4)
                    leftOption.Add(UserData.Instance.EnemyPlayerName[i]);
            }
            print(leftOption.Count);
        }

        print(leftOption.Count);
        changeLiberoChoice(leftLiberoC, 0);
        changeLiberoChoice(rightLiberoC, 1);
        SetChangeLibero();
        AutoCLibero();
    }
    bool lockingL = false, lockingR = false;
    public GameObject Canvas;
    RefreshPoint PointScript;
    public void AutoCLibero(){
        for(int i = 0; i < 2; i++){
            string output1 = String.Format("{0} num: {1} first: {2} sec: {3}\n", "left", leftLibero[i, 0], leftLibero[i, 1], leftLibero[i, 2]);
            string output2 = String.Format("{0} num: {1} first: {2} sec: {3}\n", "right", rightLibero[i, 0], rightLibero[i, 1], rightLibero[i, 2]);
            Debug.Log(output1);
            Debug.Log(output2);
        }

        PointScript = Canvas.GetComponent<RefreshPoint>();        

        for(int i = 0; i < 2; i++){
            
            if(leftLibero[i,1]!=0 || leftLibero[i, 2]!= 0){
                if(Int32.Parse(leftDrag[1].playerNum) == leftLibero[i,0] || 
                   Int32.Parse(leftDrag[2].playerNum) == leftLibero[i,0] || 
                   Int32.Parse(leftDrag[3].playerNum) == leftLibero[i,0]){ // 自由球員在前排
                    string tmpName="", tmpNum="";
                    int tmpPos=0, change=0 ;
                    change = leftLibero[i, 1] < 0 ? 1 : 2; 
                    leftLibero[i, change] *= -1;
                    for(int j = 0; j < 6; j++){
                        if((leftRight == 0 ? UserData.Instance.UserPlayerNumber[j] : UserData.Instance.EnemyPlayerNumber[j]) == leftLibero[i, change]){
                            tmpNum = leftRight == 0 ? UserData.Instance.UserPlayerNumber[j].ToString() : UserData.Instance.EnemyPlayerNumber[j].ToString();
                            tmpName = leftRight == 0 ? UserData.Instance.UserPlayerName[j] : UserData.Instance.EnemyPlayerName[j];
                            tmpPos = leftRight == 0 ? UserData.Instance.UserPlayerPlayPos[j] : UserData.Instance.EnemyPlayerPlayPos[j];
                            break;
                        }
                    }
                    
                    foreach(int j in  new List<int>{1,2,3}){
                        if(Int32.Parse(leftDrag[j].playerNum) == leftLibero[i, 0]){
                            leftDrag[j].playerNum = tmpNum;
                            leftDrag[j].playerPlayPos = tmpPos;
                            leftDrag[j].playerName = tmpName;
                            leftDrag[j].updata[0] = false;
                        }
                    }
                    leftLiberoC[i].interactable = true;
                    lockingL = false;
                }
                else if(Int32.Parse(leftDrag[4].playerNum) == leftLibero[i,0] || 
                        Int32.Parse(leftDrag[5].playerNum) == leftLibero[i,0]){ // 自由球員在後排
                    lockingL = true;
                }
                else{ // 自由球員未在場上
                    if((Int32.Parse(leftDrag[0].playerNum) == leftLibero[i,1] || 
                        Int32.Parse(leftDrag[4].playerNum) == leftLibero[i,1] || 
                        Int32.Parse(leftDrag[5].playerNum) == leftLibero[i,1] ||
                        Int32.Parse(leftDrag[0].playerNum) == leftLibero[i,2] || 
                        Int32.Parse(leftDrag[4].playerNum) == leftLibero[i,2] || 
                        Int32.Parse(leftDrag[5].playerNum) == leftLibero[i,2])&& 
                        PointScript.whoServe == 1){ // 目標球員在後排
                        string tmpName="", tmpNum="";
                        int tmpPos=0;
                        for(int j = 0; j < 12; j++){
                            if((leftRight == 0 ? UserData.Instance.UserPlayerNumber[j] : UserData.Instance.EnemyPlayerNumber[j]) == leftLibero[i, 0]){
                                tmpNum = leftRight == 0 ? UserData.Instance.UserPlayerNumber[j].ToString() : UserData.Instance.EnemyPlayerNumber[j].ToString();
                                tmpName = leftRight == 0 ? UserData.Instance.UserPlayerName[j] : UserData.Instance.EnemyPlayerName[j];
                                tmpPos = leftRight == 0 ? UserData.Instance.UserPlayerPlayPos[j] : UserData.Instance.EnemyPlayerPlayPos[j];
                                break;
                            }
                        }
                        foreach(int j in new List<int>{0, 4, 5}){
                            if(Int32.Parse(leftDrag[j].playerNum) == leftLibero[i,1]){
                                leftDrag[j].playerName = tmpName;
                                leftDrag[j].playerNum = tmpNum;
                                leftDrag[j].playerPlayPos = tmpPos;
                                leftDrag[j].updata[0] = false;
                                leftLibero[i,1] *= -1;
                                break;
                            }
                            else if(Int32.Parse(leftDrag[j].playerNum) == leftLibero[i,2]){
                                leftDrag[j].playerName = tmpName;
                                leftDrag[j].playerNum = tmpNum;
                                leftDrag[j].playerPlayPos = tmpPos;
                                leftDrag[j].updata[0] = false;
                                leftLibero[i,2] *= -1;
                                break;
                            }
                            
                        }

                    }
                    lockingL = true;
                }
            }

            if(rightLibero[i,1]!=0 || rightLibero[i, 2]!=0){
                if(Int32.Parse(rightDrag[1].playerNum) == rightLibero[i,0] || 
                   Int32.Parse(rightDrag[2].playerNum) == rightLibero[i,0] || 
                   Int32.Parse(rightDrag[3].playerNum) == rightLibero[i,0]){ // 自由球員在前排
                    string tmpName="", tmpNum="";
                    int tmpPos=0, change=0 ;
                    change = rightLibero[i, 1] < 0 ? 1 : 2; 
                    rightLibero[i, change] *= -1;
                    for(int j = 0; j < 6; j++){
                        if((leftRight == 1 ? UserData.Instance.UserPlayerNumber[j] : UserData.Instance.EnemyPlayerNumber[j]) == rightLibero[i, change]){
                            tmpNum = leftRight == 1 ? UserData.Instance.UserPlayerNumber[j].ToString() : UserData.Instance.EnemyPlayerNumber[j].ToString();
                            tmpName = leftRight == 1 ? UserData.Instance.UserPlayerName[j] : UserData.Instance.EnemyPlayerName[j];
                            tmpPos = leftRight == 1 ? UserData.Instance.UserPlayerPlayPos[j] : UserData.Instance.EnemyPlayerPlayPos[j];
                            break;
                        }
                    }
                    
                    foreach(int j in  new List<int>{1,2,3}){
                        if(Int32.Parse(rightDrag[j].playerNum) == rightLibero[i, 0]){
                            rightDrag[j].playerNum = tmpNum;
                            rightDrag[j].playerPlayPos = tmpPos;
                            rightDrag[j].playerName = tmpName;
                            rightDrag[j].updata[0] = false;
                        }
                    }
                    rightLiberoC[i].interactable = true;
                }
                else if(Int32.Parse(rightDrag[4].playerNum) == rightLibero[i,0] || 
                        Int32.Parse(rightDrag[5].playerNum) == rightLibero[i,0]){ // 自由球員在後排
                    lockingR = true;
                }
                else{ // 自由球員未在場上
                    if((Int32.Parse(rightDrag[0].playerNum) == rightLibero[i,1] || 
                        Int32.Parse(rightDrag[4].playerNum) == rightLibero[i,1] || 
                        Int32.Parse(rightDrag[5].playerNum) == rightLibero[i,1] ||
                        Int32.Parse(rightDrag[0].playerNum) == rightLibero[i,2] || 
                        Int32.Parse(rightDrag[4].playerNum) == rightLibero[i,2] || 
                        Int32.Parse(rightDrag[5].playerNum) == rightLibero[i,2])&& 
                        PointScript.whoServe == 0){ // 目標球員在後排
                        string tmpName="", tmpNum="";
                        int tmpPos=0;
                        for(int j = 0; j < 12; j++){
                            if((leftRight == 1 ? UserData.Instance.UserPlayerNumber[j] : UserData.Instance.EnemyPlayerNumber[j]) == rightLibero[i, 0]){
                                tmpNum = leftRight == 1 ? UserData.Instance.UserPlayerNumber[j].ToString() : UserData.Instance.EnemyPlayerNumber[j].ToString();
                                tmpName = leftRight == 1 ? UserData.Instance.UserPlayerName[j] : UserData.Instance.EnemyPlayerName[j];
                                tmpPos = leftRight == 1 ? UserData.Instance.UserPlayerPlayPos[j] : UserData.Instance.EnemyPlayerPlayPos[j];
                                break;
                            }
                        }
                        foreach(int j in new List<int>{0, 4, 5}){
                            if(Int32.Parse(rightDrag[j].playerNum) == rightLibero[i,1]){
                                rightDrag[j].playerName = tmpName;
                                rightDrag[j].playerNum = tmpNum;
                                rightDrag[j].playerPlayPos = tmpPos;
                                rightDrag[j].updata[0] = false;
                                rightLibero[i,1] *= -1;
                                break;
                            }
                            else if(Int32.Parse(rightDrag[j].playerNum) == rightLibero[i,2]){
                                rightDrag[j].playerName = tmpName;
                                rightDrag[j].playerNum = tmpNum;
                                rightDrag[j].playerPlayPos = tmpPos;
                                rightDrag[j].updata[0] = false;
                                rightLibero[i,2] *= -1;
                                break;
                            }
                            
                        }

                    }
                    
                    lockingR = true;
                }
            }

            
        }
    }

}
