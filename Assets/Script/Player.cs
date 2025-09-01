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


    public void OnMove(InputAction.CallbackContext context)        //InputAction.CallbackContext��Input System�Ŕ��������C�x���g���擾���邽�߂̂���
    {

        _inputDirection = context.ReadValue<Vector2>();             //context��Vector2�^�ɕϊ������l��_inputDirection�ɑ�����Ă���@
                                                                    //�܂�A�E���͂Ȃ�i1,0�j�����͂Ȃ�i-1,0�j�������Ă���
        Debug.Log("aaa");
    }
}
