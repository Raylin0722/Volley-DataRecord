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
    [SerializeField] public int Self_Score;
    [SerializeField] Text Self_Score_Text;
    [SerializeField] public int Enemy_Score;
    [SerializeField] Text Enemy_Score_Text;
    [SerializeField] GameObject Self_Serve;
    [SerializeField] GameObject Enemy_Serve;

    [SerializeField] GameObject Self_Player1;
    [SerializeField] GameObject Self_Player2;
    [SerializeField] GameObject Self_Player3;
    [SerializeField] GameObject Self_Player4;
    [SerializeField] GameObject Self_Player5;
    [SerializeField] GameObject Self_Player6;
    [SerializeField] GameObject Enemy_Player1;
    [SerializeField] GameObject Enemy_Player2;
    [SerializeField] GameObject Enemy_Player3;
    [SerializeField] GameObject Enemy_Player4;
    [SerializeField] GameObject Enemy_Player5;
    [SerializeField] GameObject Enemy_Player6;

    [SerializeField] int whoServe;// 0 unassign 1 left -1 right

    // Start is called before the first frame update
    void Start()
    {
        Self_Score = 0;
        Self_Point = 0;   
        Enemy_Point = 0;
        Enemy_Score = 0;
        Self_Point_Text.text = "00";
        Self_Score_Text.text = "0";
        Enemy_Point_Text.text = "00";
        Enemy_Score_Text.text = "0";
        whoServe = 1;
        Self_Serve.SetActive(true);
        Enemy_Serve.SetActive(false);
        
    }


    public void add_point(){//小比分
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        if(obj.tag == "SelfPoint"){
            
            Self_Point++;
            if(Self_Point < 10){
                Self_Point_Text.text = "0" + Self_Point.ToString();
            }
            else{
                Self_Point_Text.text = Self_Point.ToString();
            }
            changeServe(obj);
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
            changeServe(obj);
            add_score();
            
        }
        
    }

    public void add_score(){//大比分
        //Debug.Log(Self_Point + " " + Enemy_Point);
        int winPoints = 25;
        if(Self_Score + Enemy_Score >= 4)
            winPoints = 15;
        if(Self_Point >= winPoints && ((Self_Point - Enemy_Point) >= 2)){
            Self_Score++;
            Self_Score_Text.text = Self_Score.ToString();
            Self_Point_Text.text = "00";
            Enemy_Point_Text.text = "00";
            changeSideServe();
            Self_Point = 0;
            Enemy_Point = 0;
            
        }
        else if(Enemy_Point >= winPoints && ((Enemy_Point - Self_Point) >= 2)){
            Enemy_Score++;
            Enemy_Score_Text.text = Enemy_Score.ToString();
            Enemy_Point_Text.text = "00";
            Self_Point_Text.text = "00";
            changeSideServe();
            Enemy_Point = 0;
            Self_Point = 0;
        }

        
    }
   
    public void changeServe(GameObject obj){
        if(obj.tag == "SelfPoint" && Enemy_Serve.activeSelf){
            Enemy_Serve.SetActive(false);
            Self_Serve.SetActive(true);
            selfRotation();
        }
        else if(obj.tag == "EnemyPoint" && Self_Serve.activeSelf){
            Enemy_Serve.SetActive(true);
            Self_Serve.SetActive(false);
            enemyRotation();
        }
    }

    public void changeSideServe(){
        //Debug.Log("Test");
       
        if(whoServe == 1){
            Self_Serve.SetActive(false);
            Enemy_Serve.SetActive(true);
            whoServe = -1;
        }
        else if (whoServe == -1){
            Self_Serve.SetActive(true);
            Enemy_Serve.SetActive(false);
            whoServe = 1;
        }
        
    }
    
    public Sprite Sprite_tmp;
    string Name_tmp;
    public void selfRotation() {
        Sprite_tmp = Self_Player1.gameObject.GetComponent<SpriteRenderer>().sprite;
        Self_Player1.gameObject.GetComponent<SpriteRenderer>().sprite = Self_Player2.gameObject.GetComponent<SpriteRenderer>().sprite;
        Self_Player2.gameObject.GetComponent<SpriteRenderer>().sprite = Self_Player3.gameObject.GetComponent<SpriteRenderer>().sprite;
        Self_Player3.gameObject.GetComponent<SpriteRenderer>().sprite = Self_Player4.gameObject.GetComponent<SpriteRenderer>().sprite;
        Self_Player4.gameObject.GetComponent<SpriteRenderer>().sprite = Self_Player5.gameObject.GetComponent<SpriteRenderer>().sprite;
        Self_Player5.gameObject.GetComponent<SpriteRenderer>().sprite = Self_Player6.gameObject.GetComponent<SpriteRenderer>().sprite;
        Self_Player6.gameObject.GetComponent<SpriteRenderer>().sprite = Sprite_tmp;

        Name_tmp = Self_Player1.gameObject.GetComponent<dragPlayer>().playerName;
        Self_Player1.gameObject.GetComponent<dragPlayer>().playerName = Self_Player2.gameObject.GetComponent<dragPlayer>().playerName;
        Self_Player2.gameObject.GetComponent<dragPlayer>().playerName = Self_Player3.gameObject.GetComponent<dragPlayer>().playerName;
        Self_Player3.gameObject.GetComponent<dragPlayer>().playerName = Self_Player4.gameObject.GetComponent<dragPlayer>().playerName;
        Self_Player4.gameObject.GetComponent<dragPlayer>().playerName = Self_Player5.gameObject.GetComponent<dragPlayer>().playerName;
        Self_Player5.gameObject.GetComponent<dragPlayer>().playerName = Self_Player6.gameObject.GetComponent<dragPlayer>().playerName;
        Self_Player6.gameObject.GetComponent<dragPlayer>().playerName = Name_tmp;
    }
    public void enemyRotation() {
        Sprite_tmp = Enemy_Player1.gameObject.GetComponent<SpriteRenderer>().sprite;
        Enemy_Player1.gameObject.GetComponent<SpriteRenderer>().sprite = Enemy_Player2.gameObject.GetComponent<SpriteRenderer>().sprite;
        Enemy_Player2.gameObject.GetComponent<SpriteRenderer>().sprite = Enemy_Player3.gameObject.GetComponent<SpriteRenderer>().sprite;
        Enemy_Player3.gameObject.GetComponent<SpriteRenderer>().sprite = Enemy_Player4.gameObject.GetComponent<SpriteRenderer>().sprite;
        Enemy_Player4.gameObject.GetComponent<SpriteRenderer>().sprite = Enemy_Player5.gameObject.GetComponent<SpriteRenderer>().sprite;
        Enemy_Player5.gameObject.GetComponent<SpriteRenderer>().sprite = Enemy_Player6.gameObject.GetComponent<SpriteRenderer>().sprite;
        Enemy_Player6.gameObject.GetComponent<SpriteRenderer>().sprite = Sprite_tmp;

        Name_tmp = Enemy_Player1.gameObject.GetComponent<dragPlayer>().playerName;
        Enemy_Player1.gameObject.GetComponent<dragPlayer>().playerName = Enemy_Player2.gameObject.GetComponent<dragPlayer>().playerName;
        Enemy_Player2.gameObject.GetComponent<dragPlayer>().playerName = Enemy_Player3.gameObject.GetComponent<dragPlayer>().playerName;
        Enemy_Player3.gameObject.GetComponent<dragPlayer>().playerName = Enemy_Player4.gameObject.GetComponent<dragPlayer>().playerName;
        Enemy_Player4.gameObject.GetComponent<dragPlayer>().playerName = Enemy_Player5.gameObject.GetComponent<dragPlayer>().playerName;
        Enemy_Player5.gameObject.GetComponent<dragPlayer>().playerName = Enemy_Player6.gameObject.GetComponent<dragPlayer>().playerName;
        Enemy_Player6.gameObject.GetComponent<dragPlayer>().playerName = Name_tmp;
    }

}
