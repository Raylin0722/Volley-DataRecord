using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SystemData : MonoBehaviour
{
    [SerializeField] GameObject SelfScore;
    [SerializeField] GameObject EnemyScore;
    [SerializeField] Text LeftPoint;
    [SerializeField] Text RightPoint;
    public string formation;
}
