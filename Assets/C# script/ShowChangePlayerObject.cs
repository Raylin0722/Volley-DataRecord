using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowshowListPlayerObject : MonoBehaviour
{
    [SerializeField] GameObject List;
    [SerializeField] GameObject ShowSelfPlayerButtom;
    [SerializeField] GameObject ShowEnemyPlayerButtom;
    [SerializeField] GameObject AllofSelfPlayer1;
    [SerializeField] GameObject AllofSelfPlayer2;
    [SerializeField] GameObject AllofSelfPlayer3;
    [SerializeField] GameObject AllofSelfPlayer4;
    [SerializeField] GameObject AllofSelfPlayer5;
    [SerializeField] GameObject AllofSelfPlayer6;
    [SerializeField] GameObject AllofSelfPlayer7;
    [SerializeField] GameObject AllofSelfPlayer8;
    [SerializeField] GameObject AllofSelfPlayer9;
    [SerializeField] GameObject AllofSelfPlayer10;
    [SerializeField] GameObject AllofSelfPlayer11;
    [SerializeField] GameObject AllofSelfPlayer12;
    [SerializeField] GameObject AllofEnemyPlayer1;
    [SerializeField] GameObject AllofEnemyPlayer2;
    [SerializeField] GameObject AllofEnemyPlayer3;
    [SerializeField] GameObject AllofEnemyPlayer4;
    [SerializeField] GameObject AllofEnemyPlayer5;
    [SerializeField] GameObject AllofEnemyPlayer6;
    [SerializeField] GameObject AllofEnemyPlayer7;
    [SerializeField] GameObject AllofEnemyPlayer8;
    [SerializeField] GameObject AllofEnemyPlayer9;
    [SerializeField] GameObject AllofEnemyPlayer10;
    [SerializeField] GameObject AllofEnemyPlayer11;
    [SerializeField] GameObject AllofEnemyPlayer12;
    public int showList = 0;
    public int showSelfPlayer = 0;
    public int showEnemyPlayer = 0;
    public void showListAndPlayer() {
        showList = 1 - showList;
        if(showList == 1) {
            List.SetActive(true);
            ShowSelfPlayerButtom.SetActive(true);
            ShowEnemyPlayerButtom.SetActive(true);
        }
        else if(showList == 0) {
            List.SetActive(false);
            ShowSelfPlayerButtom.SetActive(false);
            ShowEnemyPlayerButtom.SetActive(false);
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
            AllofSelfPlayer1.SetActive(true);
            AllofSelfPlayer2.SetActive(true);
            AllofSelfPlayer3.SetActive(true);
            AllofSelfPlayer4.SetActive(true);
            AllofSelfPlayer5.SetActive(true);
            AllofSelfPlayer6.SetActive(true);
            AllofSelfPlayer7.SetActive(true);
            AllofSelfPlayer8.SetActive(true);
            AllofSelfPlayer9.SetActive(true);
            AllofSelfPlayer10.SetActive(true);
            AllofSelfPlayer11.SetActive(true);
            AllofSelfPlayer12.SetActive(true);
        }
        if(showSelfPlayer == 0) {
            AllofSelfPlayer1.SetActive(false);
            AllofSelfPlayer2.SetActive(false);
            AllofSelfPlayer3.SetActive(false);
            AllofSelfPlayer4.SetActive(false);
            AllofSelfPlayer5.SetActive(false);
            AllofSelfPlayer6.SetActive(false);
            AllofSelfPlayer7.SetActive(false);
            AllofSelfPlayer8.SetActive(false);
            AllofSelfPlayer9.SetActive(false);
            AllofSelfPlayer10.SetActive(false);
            AllofSelfPlayer11.SetActive(false);
            AllofSelfPlayer12.SetActive(false);
        }
        if(showEnemyPlayer == 1) {
            AllofEnemyPlayer1.SetActive(true);
            AllofEnemyPlayer2.SetActive(true);
            AllofEnemyPlayer3.SetActive(true);
            AllofEnemyPlayer4.SetActive(true);
            AllofEnemyPlayer5.SetActive(true);
            AllofEnemyPlayer6.SetActive(true);
            AllofEnemyPlayer7.SetActive(true);
            AllofEnemyPlayer8.SetActive(true);
            AllofEnemyPlayer9.SetActive(true);
            AllofEnemyPlayer10.SetActive(true);
            AllofEnemyPlayer11.SetActive(true);
            AllofEnemyPlayer12.SetActive(true);
        }
        if(showEnemyPlayer == 0) {
            AllofEnemyPlayer1.SetActive(false);
            AllofEnemyPlayer2.SetActive(false);
            AllofEnemyPlayer3.SetActive(false);
            AllofEnemyPlayer4.SetActive(false);
            AllofEnemyPlayer5.SetActive(false);
            AllofEnemyPlayer6.SetActive(false);
            AllofEnemyPlayer7.SetActive(false);
            AllofEnemyPlayer8.SetActive(false);
            AllofEnemyPlayer9.SetActive(false);
            AllofEnemyPlayer10.SetActive(false);
            AllofEnemyPlayer11.SetActive(false);
            AllofEnemyPlayer12.SetActive(false);
        }
    }
}
