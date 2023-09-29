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
    
    // Start is called before the first frame update
    void Start()
    {
        Self_Point = 0;   
        They_Point = 0;
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
        }
        else{
            They_Point++;
            if(They_Point < 10){
                They_Point_Text.text = "0" + They_Point.ToString();
            }
            else{
                They_Point_Text.text = They_Point.ToString();
            }
        }
    }
   
}
