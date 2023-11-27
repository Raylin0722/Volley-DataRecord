using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class openSettingScene : MonoBehaviour
{
    [SerializeField] string MainScene;
    [SerializeField] string SetScene;
    [SerializeField] string data;
    public void showSetting() {
        SceneManager.LoadScene(SetScene);
    }
    public void closeSetting() {
        SceneManager.LoadScene(MainScene);
    }
    public void showData() {
        SceneManager.LoadScene(data);
    }
    public void closeData() {
        SceneManager.LoadScene(MainScene);
    }
}
