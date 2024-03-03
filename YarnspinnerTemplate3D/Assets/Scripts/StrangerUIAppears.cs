using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangerUIAppears : MonoBehaviour
 {

    [YarnCommand("UIAppears")]
    public void UIAppears() {
        Debug.Log($"{name} is leaping!");
    }
}
