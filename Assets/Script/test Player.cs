using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class testPlayer : MonoBehaviour
{
    private Rigidbody _rb;
    private InputMap _inputMap;

    [SerializeField]
    public float moveSpeed = 5f;
    private Vector2 _InputDirectoin;

    private void Awake()
    {
        _inputMap = new InputMap();

        // Move入力の値を受け取る設定
        _inputMap.Player.Move.performed += ctx => _InputDirectoin = ctx.ReadValue<Vector2>();
        _inputMap.Player.Move.canceled += ctx => _InputDirectoin = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _inputMap.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(_inputMap.Player.Fire.triggered)
        {
            Debug.Log("ファイヤー");
        }
        if(_inputMap.Player.Move.triggered)
        {
           
        }
    }
    void FixedUpdate()
    {
        Vector3 currentVelocity = _rb.velocity;
        Vector3 move = new Vector3(_InputDirectoin.x * moveSpeed, currentVelocity.y, _InputDirectoin.y * moveSpeed);
        _rb.velocity = move;
    }

    private void OnDisable()
    {
        _inputMap.Disable();
    }
}
