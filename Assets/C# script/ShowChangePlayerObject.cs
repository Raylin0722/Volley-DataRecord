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
    void showPlayer() {
        if(showSelfPlayer == 1) {
            for(int i = 0;i < 12;i++) {
                if(!string.IsNullOrWhiteSpace(SaveAndLoadName.SelfPlayerInfo[i,0]) && !string.IsNullOrWhiteSpace(SaveAndLoadName.SelfPlayerInfo[i,1]))
                    AllofSelfPlayer[i].SetActive(true);
            }
        }
        if(showSelfPlayer == 0) {
            for(int i = 0;i < 12;i++)
                AllofSelfPlayer[i].SetActive(false);
        }
        if(showEnemyPlayer == 1) {
            for(int i = 0;i < 12;i++) {
                if(!string.IsNullOrWhiteSpace(SaveAndLoadName.EnemyPlayerInfo[i,0]) && !string.IsNullOrWhiteSpace(SaveAndLoadName.EnemyPlayerInfo[i,1]))
                    AllofEnemyPlayer[i].SetActive(true);
            }
        }
        if(showEnemyPlayer == 0) {
            for(int i = 0;i < 12;i++)
                AllofEnemyPlayer[i].SetActive(false);
        }
    }
}
