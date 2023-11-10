using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveAndLoadName : MonoBehaviour
{
    [SerializeField] public static string[] TeamName = new string[2];
    [SerializeField] public static string[,] SelfPlayerInfo = new string[12, 2];
    [SerializeField] public static string[,] EnemyPlayerInfo = new string[12, 2];
    [SerializeField] InputField SelfTeamName, EnemyTeamName;
    [SerializeField] InputField SP1Number, SP2Number, SP3Number, SP4Number, SP5Number, SP6Number, SP7Number, SP8Number, SP9Number, SP10Number, SP11Number, SP12Number;
    [SerializeField] InputField SP1Name, SP2Name, SP3Name, SP4Name, SP5Name, SP6Name, SP7Name, SP8Name, SP9Name, SP10Name, SP11Name, SP12Name;
    [SerializeField] InputField EP1Number, EP2Number, EP3Number, EP4Number, EP5Number, EP6Number, EP7Number, EP8Number, EP9Number, EP10Number, EP11Number, EP12Number;
    [SerializeField] InputField EP1Name, EP2Name, EP3Name, EP4Name, EP5Name, EP6Name, EP7Name, EP8Name, EP9Name, EP10Name, EP11Name, EP12Name;
    public void readTeamName() {
        TeamName[0] = SelfTeamName.text;
        TeamName[1] = EnemyTeamName.text;
    }
    public void readSelfInfo() {
        SelfPlayerInfo[0, 0] = SP1Number.text; SelfPlayerInfo[0, 1] = SP1Name.text;
        SelfPlayerInfo[1, 0] = SP2Number.text; SelfPlayerInfo[1, 1] = SP2Name.text;
        SelfPlayerInfo[2, 0] = SP3Number.text; SelfPlayerInfo[2, 1] = SP3Name.text;
        SelfPlayerInfo[3, 0] = SP4Number.text; SelfPlayerInfo[3, 1] = SP4Name.text;
        SelfPlayerInfo[4, 0] = SP5Number.text; SelfPlayerInfo[4, 1] = SP5Name.text;
        SelfPlayerInfo[5, 0] = SP6Number.text; SelfPlayerInfo[5, 1] = SP6Name.text;
        SelfPlayerInfo[6, 0] = SP7Number.text; SelfPlayerInfo[6, 1] = SP7Name.text;
        SelfPlayerInfo[7, 0] = SP8Number.text; SelfPlayerInfo[7, 1] = SP8Name.text;
        SelfPlayerInfo[8, 0] = SP9Number.text; SelfPlayerInfo[8, 1] = SP9Name.text;
        SelfPlayerInfo[9, 0] = SP10Number.text; SelfPlayerInfo[9, 1] = SP10Name.text;
        SelfPlayerInfo[10, 0] = SP11Number.text; SelfPlayerInfo[10, 1] = SP11Name.text;
        SelfPlayerInfo[11, 0] = SP12Number.text; SelfPlayerInfo[11, 1] = SP12Name.text;
    }
    public void readEnemyInfo() {
        EnemyPlayerInfo[0, 0] = EP1Number.text; EnemyPlayerInfo[0, 1] = EP1Name.text;
        EnemyPlayerInfo[1, 0] = EP2Number.text; EnemyPlayerInfo[1, 1] = EP2Name.text;
        EnemyPlayerInfo[2, 0] = EP3Number.text; EnemyPlayerInfo[2, 1] = EP3Name.text;
        EnemyPlayerInfo[3, 0] = EP4Number.text; EnemyPlayerInfo[3, 1] = EP4Name.text;
        EnemyPlayerInfo[4, 0] = EP5Number.text; EnemyPlayerInfo[4, 1] = EP5Name.text;
        EnemyPlayerInfo[5, 0] = EP6Number.text; EnemyPlayerInfo[5, 1] = EP6Name.text;
        EnemyPlayerInfo[6, 0] = EP7Number.text; EnemyPlayerInfo[6, 1] = EP7Name.text;
        EnemyPlayerInfo[7, 0] = EP8Number.text; EnemyPlayerInfo[7, 1] = EP8Name.text;
        EnemyPlayerInfo[8, 0] = EP9Number.text; EnemyPlayerInfo[8, 1] = EP9Name.text;
        EnemyPlayerInfo[9, 0] = EP10Number.text; EnemyPlayerInfo[9, 1] = EP10Name.text;
        EnemyPlayerInfo[10, 0] = EP11Number.text; EnemyPlayerInfo[10, 1] = EP11Name.text;
        EnemyPlayerInfo[11, 0] = EP12Number.text; EnemyPlayerInfo[11, 1] = EP12Name.text;
    }

    void Awake() {
        SelfTeamName.text = TeamName[0]; EnemyTeamName.text = TeamName[1];
        SP1Number.text = SelfPlayerInfo[0, 0]; SP1Name.text = SelfPlayerInfo[0, 1];
        SP2Number.text = SelfPlayerInfo[1, 0]; SP2Name.text = SelfPlayerInfo[1, 1];
        SP3Number.text = SelfPlayerInfo[2, 0]; SP3Name.text = SelfPlayerInfo[2, 1];
        SP4Number.text = SelfPlayerInfo[3, 0]; SP4Name.text = SelfPlayerInfo[3, 1];
        SP5Number.text = SelfPlayerInfo[4, 0]; SP5Name.text = SelfPlayerInfo[4, 1];
        SP6Number.text = SelfPlayerInfo[5, 0]; SP6Name.text = SelfPlayerInfo[5, 1];
        SP7Number.text = SelfPlayerInfo[6, 0]; SP7Name.text = SelfPlayerInfo[6, 1];
        SP8Number.text = SelfPlayerInfo[7, 0]; SP8Name.text = SelfPlayerInfo[7, 1];
        SP9Number.text = SelfPlayerInfo[8, 0]; SP9Name.text = SelfPlayerInfo[8, 1];
        SP10Number.text = SelfPlayerInfo[9, 0]; SP10Name.text = SelfPlayerInfo[9, 1];
        SP11Number.text = SelfPlayerInfo[10, 0]; SP11Name.text = SelfPlayerInfo[10, 1];
        SP12Number.text = SelfPlayerInfo[11, 0]; SP12Name.text = SelfPlayerInfo[11, 1];
        EP1Number.text = EnemyPlayerInfo[0, 0]; EP1Name.text = EnemyPlayerInfo[0, 1];
        EP2Number.text = EnemyPlayerInfo[1, 0]; EP2Name.text = EnemyPlayerInfo[1, 1];
        EP3Number.text = EnemyPlayerInfo[2, 0]; EP3Name.text = EnemyPlayerInfo[2, 1];
        EP4Number.text = EnemyPlayerInfo[3, 0]; EP4Name.text = EnemyPlayerInfo[3, 1];
        EP5Number.text = EnemyPlayerInfo[4, 0]; EP5Name.text = EnemyPlayerInfo[4, 1];
        EP6Number.text = EnemyPlayerInfo[5, 0]; EP6Name.text = EnemyPlayerInfo[5, 1];
        EP7Number.text = EnemyPlayerInfo[6, 0]; EP7Name.text = EnemyPlayerInfo[6, 1];
        EP8Number.text = EnemyPlayerInfo[7, 0]; EP8Name.text = EnemyPlayerInfo[7, 1];
        EP9Number.text = EnemyPlayerInfo[8, 0]; EP9Name.text = EnemyPlayerInfo[8, 1];
        EP10Number.text = EnemyPlayerInfo[9, 0]; EP10Name.text = EnemyPlayerInfo[9, 1];
        EP11Number.text = EnemyPlayerInfo[10, 0]; EP11Name.text = EnemyPlayerInfo[10, 1];
        EP12Number.text = EnemyPlayerInfo[11, 0]; EP12Name.text = EnemyPlayerInfo[11, 1];
    }
}
