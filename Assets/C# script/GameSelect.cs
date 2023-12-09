using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSelect : MonoBehaviour
{
    public GameObject Game; // 你的Prefab
    public RectTransform content; // ScrollView的Content Transform
    public float prefabHeight = 100f; // 調整Prefab的高度

    public GameObject Scrollview;

    private void Start()
    {
        // 初始化ScrollView的滾動位置
        Scrollview.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;

        // 這裡演示創建3個Prefab的例子，實際上你可以根據需求生成不同數量的Prefab
        for (int i = 0; i < 30; i++)
        {
            // 透過Instantiate生成Prefab
            GameObject newPrefab = Instantiate(Game, content);

            // 設定Prefab的位置
            RectTransform rectTransform = newPrefab.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -i * prefabHeight);
        }
        content.sizeDelta = new Vector2(content.sizeDelta.x, 0 * prefabHeight);
    }
}
