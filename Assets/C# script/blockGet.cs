using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class blockGet : MonoBehaviour
{
    [SerializeField] GameObject[] leftSiteEdge;
    [SerializeField] GameObject[] rightSiteEdge;
    [SerializeField] GameObject[] leftServe;
    [SerializeField] GameObject[] rightServe;
    [SerializeField] GameObject[] leftOutsideTop;
    [SerializeField] GameObject[] rightOutsideTop;
    [SerializeField] GameObject[] leftOutsideDown;
    [SerializeField] GameObject[] rightOutsideDown;
    [SerializeField] GameObject[] block;

    Vector3[] leftSiteEdgeVec = new Vector3[4];
    Vector3[] rightSiteEdgeVec = new Vector3[4];
    Vector3[] leftServeVec = new Vector3[4];
    Vector3[] rightServeVec = new Vector3[4];
    Vector3[] leftOutsideTopVec = new Vector3[4];
    Vector3[] rightOutsideTopVec = new Vector3[4];
    Vector3[] leftOutsideDownVec = new Vector3[4];
    Vector3[] rightOutsideDownVec = new Vector3[4];
    void Start(){
        for(int i = 0; i < 4; i++){
            calBlock(ref leftSiteEdge[i], ref leftSiteEdgeVec[i]);
            Debug.Log("leftSiteEdgeVec[" + i + "] X: " + leftSiteEdgeVec[i][0] + " Y:" + leftSiteEdgeVec[i][1]);

            calBlock(ref rightSiteEdge[i], ref rightSiteEdgeVec[i]);
            Debug.Log("rightSiteEdgeVec[" + i + "] X: " + rightSiteEdgeVec[i][0] + " Y:" + rightSiteEdgeVec[i][1]);

            calBlock(ref leftServe[i], ref leftServeVec[i]);
            Debug.Log("leftServeVec[" + i + "] X: " + leftServeVec[i][0] + " Y:" + leftServeVec[i][1]);

            calBlock(ref rightServe[i], ref rightServeVec[i]);
            Debug.Log("rightServeVec[" + i + "] X: " + rightServeVec[i][0] + " Y:" + rightServeVec[i][1]);

            calBlock(ref leftOutsideTop[i], ref leftOutsideTopVec[i]);
            Debug.Log("leftOutsideTopVec[" + i + "] X: " + leftOutsideTopVec[i][0] + " Y:" + leftOutsideTopVec[i][1]);

            calBlock(ref leftOutsideDown[i], ref leftOutsideDownVec[i]);
            Debug.Log("leftOutsideDownVec[" + i + "] X: " + leftOutsideDownVec[i][0] + " Y:" + leftOutsideDownVec[i][1]);

            calBlock(ref rightOutsideTop[i], ref rightOutsideTopVec[i]);
            Debug.Log("rightOutsideTopVec[" + i + "] X: " + rightOutsideTopVec[i][0] + " Y:" + rightOutsideTopVec[i][1]);

            calBlock(ref rightOutsideDown[i], ref rightOutsideDownVec[i]);
            Debug.Log("rightOutsideDownVec[" + i + "] X: " + rightOutsideDownVec[i][0] + " Y:" + rightOutsideDownVec[i][1]);

        }
    }
    private void calBlock(ref GameObject input,ref Vector3 inputVec){

        RectTransform temp = input.GetComponent<RectTransform>();
        inputVec = new Vector3(temp.rect.x, temp.rect.y, 0);
        inputVec = temp.TransformPoint(inputVec);
        //Debug.Log("X: " + inputVec[0] + " Y:" + inputVec[1]);
    }

    /*RectTransform temp = block.GetComponent<RectTransform>();

        Vector3 test = new Vector3(temp.rect.x, temp.rect.y, 0);

        Debug.Log(" X: " + test[0] + " Y:" + test[1]);

        test = temp.TransformPoint(test);
        
        Debug.Log("WidthRatio: " + widthRatio + " HeightRatio: " + heightRatio + " X: " + test[0] + " Y:" + test[1] + " Playerx: " + initialPosition[0] + " Playery: " +initialPosition[1]);*/
}
