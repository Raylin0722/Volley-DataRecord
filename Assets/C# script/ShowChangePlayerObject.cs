using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowshowListPlayerObject : MonoBehaviour
{
    [SerializeField] GameObject List;
    [SerializeField] GameObject ShowSelfPlayerButtom;
    [SerializeField] GameObject ShowEnemyPlayerButtom;
    [SerializeField] GameObject[] AllofSelfPlayer;
    [SerializeField] GameObject[] AllofEnemyPlayer;
    public GameObject GameView;
    public GameObject deleteNewData;
    public bool change = false;
    public int showList = 0;
    public int showSelfPlayer = 0;
    public int showEnemyPlayer = 0;
    public void showListAndPlayer() {
        showList = 1 - showList;
        showSelfPlayer = 0;
        showEnemyPlayer = 0;
        if(showList == 1) {
            List.SetActive(true);
            ShowSelfPlayerButtom.SetActive(true);
            ShowEnemyPlayerButtom.SetActive(true);
        }
        else if(showList == 0) {
            List.SetActive(false);
            ShowSelfPlayerButtom.SetActive(false);
            ShowEnemyPlayerButtom.SetActive(false);
            for(int i = 0;i < 12;i++) {
                AllofSelfPlayer[i].SetActive(false);
                AllofEnemyPlayer[i].SetActive(false);
            }
        }
    }
    public void callShowSelfPlayer() {
        showSelfPlayer = 1 - showSelfPlayer;
        if(showSelfPlayer == 1)
            showEnemyPlayer = 0;
        showPlayer();
    }
    public void callShowEnemyPlayer() {
        showEnemyPlayer = 1 - showEnemyPlayer;
        if(showEnemyPlayer == 1)
            showSelfPlayer = 0;
        showPlayer();
    }
    public void showPlayer() {
        if(!change){
            GameView.SetActive(false);
            deleteNewData.SetActive(false);
            for(int i = 0; i < 6; i++) {
                AllofSelfPlayer[i].SetActive(true);
                AllofEnemyPlayer[i + 6].SetActive(true);
            }
            change = true;
        }
        else{
            GameView.SetActive(true);
            deleteNewData.SetActive(true);
            for(int i = 0; i < 6; i++) {
                AllofSelfPlayer[i].SetActive(false);
                AllofEnemyPlayer[i + 6].SetActive(false);
            }
            change = false;
        }
    }
}
