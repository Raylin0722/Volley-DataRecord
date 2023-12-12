using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class openSettingScene : MonoBehaviour
{
    [SerializeField] GameObject MainScene;
    [SerializeField] GameObject SettingScene;
    [SerializeField] GameObject DataScene;
    [SerializeField] public static int interactable = 1;
    
    [SerializeField] GameObject BarScene;
    public void showBar(){
        BarScene.SetActive(true);
    }

    public void showSetting() {
        SettingScene.SetActive(true);
        setzero();
    }
    public void closeSetting() {
        SettingScene.SetActive(false);
        setone();
    }
    public void showData() {
        DataScene.SetActive(true);
        setzero();
    }
    public void closeDataScene() {
        DataScene.SetActive(false);
        BarScene.SetActive(false);
        setone();
    }

    public void setzero(){
        interactable = 0;
    }

    public void setone(){
        interactable = 1;
    }
}
