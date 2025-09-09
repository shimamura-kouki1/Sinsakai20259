using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    public float _MoveSpeed;

    private Vector2 _inputDirection;

    private Rigidbody _rd;
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


    public void Onmove(InputAction.CallbackContext context)
    {
        // �����Ă���Ԃ͓��̓x�N�g����ێ�
        _inputDirection = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {   //�w�肳�ꂽ�ʒu�Ɉړ�����
        _rd.MovePosition(_rd.position + new Vector3(_inputDirection.x, 0f, _inputDirection.y) * _MoveSpeed * Time.fixedDeltaTime);
    }


}
