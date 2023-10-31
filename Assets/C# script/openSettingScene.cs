using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class openSettingScene : MonoBehaviour
{
    [SerializeField] string MainScene;
    [SerializeField] string SetScene;
    public void showSetting() {
        SceneManager.LoadScene(SetScene);
    }
    public void closeSetting() {
        SceneManager.LoadScene(MainScene);
    }
}
