using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class openSettingScene : MonoBehaviour
{
    [SerializeField] GameObject MainScene;
    [SerializeField] GameObject SettingScene;
    public void showSetting() {
        SettingScene.SetActive(true);
    }
    public void closeSetting() {
        SettingScene.SetActive(false);
    }
}
