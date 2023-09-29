using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RefreshPoint : MonoBehaviour
{
    [SerializeField] int Self_Point;
    [SerializeField] Text Self_Point_Text;
    [SerializeField] int Enemy_Point;
    [SerializeField] Text Enemy_Point_Text;
    [SerializeField] int Self_Score;
    [SerializeField] Text Self_Score_Text;
    [SerializeField] int Enemy_Score;
    [SerializeField] Text Enemy_Score_Text;
    
    // Start is called before the first frame update
    void Start()
    {
        Self_Score = 0;
        Self_Point = 0;   
        Enemy_Point = 0;
        Enemy_Score = 0;
        Self_Point_Text.text = "00";
        Self_Score_Text.text = "00";
        Enemy_Point_Text.text = "00";
        Enemy_Score_Text.text = "00";
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void add_point(){
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        if(obj.tag == "SelfPoint"){
            Self_Point++;
            if(Self_Point < 10){
                Self_Point_Text.text = "0" + Self_Point.ToString();
            }
            else{
                Self_Point_Text.text = Self_Point.ToString();
            }
            add_score();
        }
        else{
            Enemy_Point++;
            if(Enemy_Point < 10){
                Enemy_Point_Text.text = "0" + Enemy_Point.ToString();
            }
            else{
                Enemy_Point_Text.text = Enemy_Point.ToString();
            }
            add_score();
        }
    }

    public void add_score(){
        Debug.Log(Self_Point + " " + Enemy_Point);
        if(Self_Point >= 25 && ((Self_Point - Enemy_Point) >= 2)){
            Self_Score++;
            Self_Score_Text.text = Self_Score.ToString();
            Self_Point_Text.text = "00";
            Enemy_Point_Text.text = "00";
            Self_Point = 0;
            Enemy_Point = 0;
        }
        else if(Enemy_Point >= 25 && ((Enemy_Point - Self_Point) >= 2)){
            Enemy_Score++;
            Enemy_Score_Text.text = Enemy_Score.ToString();
            Enemy_Point_Text.text = "00";
            Self_Point_Text.text = "00";
            Enemy_Point = 0;
            Self_Point = 0;
        }
    }
   
}
