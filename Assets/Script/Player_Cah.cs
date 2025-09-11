using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class Player_Cah : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 2f;     //���s���x
    [SerializeField] private float _sprintSpeed = 4f;   //�X�v�����g���x
    [SerializeField] private float _gravity = -9.81f;   //�d��

    [Header("Look Settings")]
    [SerializeField] private float _mouseSensitivity = 2f;//�}�E�X���x
    [SerializeField] private Transform _cameraTransform; //���_�J����

    [Header("Flashlight Settings")]
    [SerializeField] private Light _flashlight;
    private bool _flashlightOn = false;


    private CharacterController _controller;
    private Vector2 _moveInput;//�ړ�����
    private Vector2 _lookInput;//���_����
    private float _verticalRotation;//�㉺�̎��_�@��]�̊p�x
    private Vector3 _velocity;//�d�͂̃x�N�g��
    private bool _isSprinting;//�_�b�V�������ǂ����̃t���O


    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // �}�E�X�J�[�\���𒆉��ɌŒ�
    }

    // Update is called once per frame
    void Update()
    {
        //�ړ�����
        float speed = _isSprinting ? _sprintSpeed : _walkSpeed;//���s���_�b�V�����𔻕ʂ���
        Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        _controller.Move(move * speed * Time.deltaTime);

        // �d�͂�K�p
        if (_controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; // �n�ʂɉ����t����
        }
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);

        // === ���_��] ===
        transform.Rotate(Vector3.up * _lookInput.x * _mouseSensitivity);

        _verticalRotation -= _lookInput.y * _mouseSensitivity;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -80f, 80f); // �㉺����
        _cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);

    }

    // ==== Input System �R�[���o�b�N ====
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
