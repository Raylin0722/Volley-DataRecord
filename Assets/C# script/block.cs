using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class block : MonoBehaviour
{
    [SerializeField] public string blockID;

    void OnDisable() {
        if(blockID != null)
            blockID = null;
    }
}
