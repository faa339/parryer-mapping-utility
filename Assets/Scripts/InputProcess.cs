using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProcess : MonoBehaviour
{
    public GameObject manager;
    public void WakeUpManager() {
        //simple little guy!
        GameObject.Instantiate(manager);
    }
}
