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
    public void OnMove(InputAction.CallbackContext context)        //InputAction.CallbackContext��Input System�Ŕ��������C�x���g���擾���邽�߂̂���
    {
        _inputDirection = context.ReadValue<Vector3>();             //context��Vector2�^�ɕϊ������l��_inputDirection�ɑ�����Ă���@
                                                                    //�܂�A�E���͂Ȃ�i1,0�j�����͂Ȃ�i-1,0�j�������Ă���
    }
}
