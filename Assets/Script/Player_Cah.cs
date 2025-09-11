using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class Player_Cah : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 2f;     //歩行速度
    [SerializeField] private float _sprintSpeed = 4f;   //スプリント速度
    [SerializeField] private float _gravity = -9.81f;   //重力

    [Header("Look Settings")]
    [SerializeField] private float _mouseSensitivity = 2f;//マウス感度
    [SerializeField] private Transform _cameraTransform; //視点カメラ

    [Header("Flashlight Settings")]
    [SerializeField] private Light _flashlight;
    private bool _flashlightOn = false;


    private CharacterController _controller;
    private Vector2 _moveInput;//移動入力
    private Vector2 _lookInput;//視点入力
    private float _verticalRotation;//上下の視点　回転の角度
    private Vector3 _velocity;//重力のベクトル
    private bool _isSprinting;//ダッシュ中かどうかのフラグ


    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // マウスカーソルを中央に固定
    }

    // Update is called once per frame
    void Update()
    {
        //移動処理
        float speed = _isSprinting ? _sprintSpeed : _walkSpeed;//歩行かダッシュかを判別する
        Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        _controller.Move(move * speed * Time.deltaTime);

        // 重力を適用
        if (_controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; // 地面に押し付ける
        }
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);

        // === 視点回転 ===
        transform.Rotate(Vector3.up * _lookInput.x * _mouseSensitivity);

        _verticalRotation -= _lookInput.y * _mouseSensitivity;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -80f, 80f); // 上下制限
        _cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);

    }

    // ==== Input System コールバック ====
    public void Onmove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed) _isSprinting = true;
        if (context.canceled) _isSprinting = false;
    }
}
