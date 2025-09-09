using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]float _MoveSpeed;
    [SerializeField] float _SprintSpeed;

    private bool _isSprinting;

    private Vector2 _inputDirection;

    private Rigidbody _rd;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        _rd = GetComponent<Rigidbody>();
    }

    public void Onmove(InputAction.CallbackContext context)
    {
        // 押している間は入力ベクトルを保持
        _inputDirection = context.ReadValue<Vector2>();
    }
    public void OnSprint(InputAction.CallbackContext context)
    {   //スプリントの切り替え
        if(context.performed)
        {   
            _isSprinting = true;
        }
        if(context.canceled)
        {
            _isSprinting = false;
        }
    }
    private void FixedUpdate()
    {
        //歩きと走りの速度切り替え
        float Speed = _isSprinting ? _MoveSpeed : _SprintSpeed;
        //指定された位置に移動する
        _rd.MovePosition(_rd.position + new Vector3(_inputDirection.x, 0f, _inputDirection.y) * Speed * Time.fixedDeltaTime);
    }
}
