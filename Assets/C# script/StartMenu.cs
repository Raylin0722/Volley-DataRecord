using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void GoToLogin(){
        SceneManager.LoadScene("Login");
    }

    public void GoToRegister(){
        SceneManager.LoadScene("Register");
    }
}
