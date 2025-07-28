using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPlayer : MonoBehaviour
{
    private InputMap _inputMap; 

    // Start is called before the first frame update
    void Start()
    {
        _inputMap = new InputMap();
        _inputMap.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(_inputMap.Player.Fire.triggered)
        {
            Debug.Log("ファイヤー");
        }
    }
}
