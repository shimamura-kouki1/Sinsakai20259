using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    public int _MoveSpeed;
    private Vector3 _inputDirection;

    public Rigidbody _rg = null;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        _rg = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
         
        }
        if(Input.GetKeyDown(KeyCode.D))
        {

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            
        }
    }

        public void Move()
        {
            _rg.velocity = new Vector3(_inputDirection.x * _MoveSpeed, _rg.velocity.y,_inputDirection.z);
        }
    public void OnMove(InputAction.CallbackContext context)        //InputAction.CallbackContextはInput Systemで発生したイベントを取得するためのもの
    {
        _inputDirection = context.ReadValue<Vector3>();             //contextをVector2型に変換した値を_inputDirectionに代入している　
                                                                    //つまり、右入力なら（1,0）左入力なら（-1,0）を代入している
    }
}
