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
        // ‰Ÿ‚µ‚Ä‚¢‚éŠÔ‚Í“ü—ÍƒxƒNƒgƒ‹‚ğ•Û
        _inputDirection = context.ReadValue<Vector2>();
        Debug.Log("Onmove called! " + _inputDirection);
    }

    private void FixedUpdate()
    {

        //Vector3 move = new Vector3(_inputDirection.x, 0f, _inputDirection.y);
        //_rd.MovePosition(_rd.position + move * _MoveSpeed * Time.fixedDeltaTime);

        _rd.MovePosition(_rd.position + new Vector3(_inputDirection.x, 0f, _inputDirection.y) * _MoveSpeed * Time.fixedDeltaTime);
    }


}
