using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    [SerializeField] Button showPlayerButton;
    [SerializeField] Button serveFirstGameButton;
    [SerializeField] public static int showPlayer = 0;
    [SerializeField] public static int whoServe = 0;
    [SerializeField] public static int show_change = 0, serve_change = 0;
    public void showPlayerNumName() {
        showPlayer = 1 - showPlayer;
        showPlayerButton.GetComponentInChildren<Text>().text = (showPlayer == 0) ? "背號" : "名字";
        show_change = 1;
    }
    public void changeServeFirstGame() {
        whoServe = 1 - whoServe;
        serveFirstGameButton.GetComponentInChildren<Text>().text = (whoServe == 0) ? "左方" : "右方";
        serve_change = 1;
    }
}
