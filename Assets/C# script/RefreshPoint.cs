using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RefreshPoint : MonoBehaviour
{
    [SerializeField] int Self_Point;
    [SerializeField] Text Self_Point_Text;
    [SerializeField] int They_Point;
    [SerializeField] Text They_Point_Text;
    [SerializeField] int Self_Score;
    [SerializeField] Text Self_Score_Text;
    [SerializeField] int They_Score;
    [SerializeField] Text They_Score_Text;
    
    // Start is called before the first frame update
    void Start()
    {
        Self_Score = 0;
        Self_Point = 0;   
        They_Point = 0;
        They_Score = 0;
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
            They_Point++;
            if(They_Point < 10){
                They_Point_Text.text = "0" + They_Point.ToString();
            }
            else{
                They_Point_Text.text = They_Point.ToString();
            }
            add_score();
        }
    }

    public void add_score(){
        if(Self_Point >= 25 && ((Self_Point - They_Point) >= 2)){
            Self_Score++;
            Self_Score_Text.text = Self_Score.ToString();
        }
        else if(They_Point >= 25 && ((They_Point - Self_Point) >= 2)){
            They_Score++;
            They_Score_Text.text = They_Score.ToString();
        }
    }
   
}
