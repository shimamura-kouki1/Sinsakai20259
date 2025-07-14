using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class test1 : MonoBehaviour
{
    private InputAciton controls;
    private Vector2 moveInput;
    private Rigidbody rb;

    [SerializeField]
    private float moveSpeed = 5f;

    private void Awake()
    {
        controls = new InputAciton();

        // 入力イベントに関数を登録
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void FixedUpdate()
    {
        // 入力された2D（X, Y）ベクトルを3Dベクトルに変換
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        rb.AddForce(move * moveSpeed, ForceMode.Force);
    }
}
