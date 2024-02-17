using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System;
using System.Linq;
using TMPro;


public class clickRecord : MonoBehaviour
{
    [SerializeField] public GameObject[] LeftPlayers;
    [SerializeField] public GameObject[] RightPlayers;
    GraphicRaycaster m_Raycaster; // 判斷block用
    PointerEventData m_PointerEventData; // 判斷block用
    EventSystem m_EventSystem; // 判斷block用
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
