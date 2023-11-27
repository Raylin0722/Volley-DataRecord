using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;
public class DataChart : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
        var chart = gameObject.GetComponent<LineChart>();
        if (chart == null){
            chart = gameObject.AddComponent<LineChart>();
            chart.Init();
        }
        var title = chart.EnsureChartComponent<Title>();
        title.text = "Simple Line";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
