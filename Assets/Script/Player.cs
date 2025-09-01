using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    public int _MoveSpeed;
    private Vector2 _inputDirection;

    public Rigidbody _rd;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        _rd = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(_inputDirection.x,0f,_inputDirection.y);
        _rd.MovePosition(_rd.position + move * _MoveSpeed * Time.fixedDeltaTime);
    }


    public void OnMove(InputAction.CallbackContext context)        //InputAction.CallbackContextはInput Systemで発生したイベントを取得するためのもの
    {

        _inputDirection = context.ReadValue<Vector2>();             //contextをVector2型に変換した値を_inputDirectionに代入している　
                                                                    //つまり、右入力なら（1,0）左入力なら（-1,0）を代入している
        Debug.Log("aaa");
    }
}
