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
        // �����Ă���Ԃ͓��̓x�N�g����ێ�
        _inputDirection = context.ReadValue<Vector2>();
    }
    public void OnSprint(InputAction.CallbackContext context)
    {   //�X�v�����g�̐؂�ւ�
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
        //�����Ƒ���̑��x�؂�ւ�
        float Speed = _isSprinting ? _MoveSpeed : _SprintSpeed;
        //�w�肳�ꂽ�ʒu�Ɉړ�����
        _rd.MovePosition(_rd.position + new Vector3(_inputDirection.x, 0f, _inputDirection.y) * Speed * Time.fixedDeltaTime);
    }
}
