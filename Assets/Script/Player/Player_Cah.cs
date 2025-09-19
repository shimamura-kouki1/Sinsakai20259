using UnityEngine;
using UnityEngine.InputSystem;


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
    [SerializeField] private Light _flashlight;//���C�g
    private bool _flashlightOn = false;//�I���E�I�t

    [Header("Player Action")]
    [SerializeField] private float _rayDistance = 3f; // �肪�͂�����


    private CharacterController _controller;//�v���C���[�ړ��p��CharacterController
    private Vector2 _moveInput;//�ړ�����
    private Vector2 _lookInput;//���_����
    private float _verticalRotation;//�㉺�̎��_�@��]�̊p�x
    private float _yaw;              // ���E���_�̊p�x
    private Vector3 _velocity;//�d�͂̃x�N�g��
    private bool _isSprinting;//�_�b�V�������ǂ����̃t���O


    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>(); // CharacterController���擾
        Cursor.lockState = CursorLockMode.Locked; // �}�E�X�J�[�\���𒆉��ɌŒ�

        if (_flashlight != null)//���C�g���ݒ肳��Ă��邩
        {
            _flashlight.enabled = false;// ���C�g��������Ԃł͏����ɂ���
        }
    }

    // Update is called once per frame
    void Update()
    {
        // === �ړ����� ===
        float speed = _isSprinting ? _sprintSpeed : _walkSpeed;//���s���_�b�V�����𔻕ʂ���
        Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;// ���͕��������[���h��Ԃɕϊ�
        _controller.Move(move * speed * Time.deltaTime);//�@�ړ��𔽉f

        // === �d�͂�K�p ===�@�d�͂��Ȃ��ƃL�����N�^�[���������܂܂ɂȂ�
        if (_controller.isGrounded && _velocity.y < 0)// �n�ʂɐڒn���Ă��āA���������̑��x������ꍇ
        {
            _velocity.y = -2f; // �n�ʂɉ����t����
        }
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);

        // === ���_��] === 

        _yaw += _lookInput.x * _mouseSensitivity;
        _verticalRotation -= _lookInput.y * _mouseSensitivity;// �㉺��]����͂ɉ����đ���
        _verticalRotation = Mathf.Clamp(_verticalRotation, -80f, 80f); // �㉺���_�̊p�x����

        transform.rotation = Quaternion.Euler(0f, _yaw, 0f); // �v���C���[�{�͍̂��E
        _cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f); // �J�����͏㉺
    }

    // ==== Input System �R�[���o�b�N ====
    public void Onmove(InputAction.CallbackContext context)// �ړ����͂��󂯎��
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)// ���_�ړ����͂��󂯎��
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context) // �_�b�V�����͂��󂯎��
    {
        if (context.performed) _isSprinting = true;// �����ꂽ�u�ԂɃ_�b�V���J�n
        if (context.canceled) _isSprinting = false;// �����ꂽ��_�b�V���I��
    }

    public void OnLight(InputAction.CallbackContext context)// �����d����ON/OFF�؂�ւ�
    {
        if (_flashlight == null) return; // �ʂ�`�F�b�N

        if (context.performed)// �{�^���������ꂽ�u��
        {
            _flashlightOn = !_flashlightOn;//bool��Ԃ̔��]
            _flashlight.enabled = _flashlightOn;//�R���|�[�l���g�̗L��/�����̏�Ԕ��]
        }
    }

    public void OnAction(InputAction.CallbackContext context)//�A�N�V�����̓��͎�t
    {
        //�����ꂽ�u�ԈȊO���^�[��
        if (!context.performed) return;

        // �J������������Ray���΂�+���C�̋���
        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, _rayDistance))
        {
            //���������I�u�W�F�N�g���h�A�ɓ������������ׂ�
            Door door = hit.collider.GetComponentInParent<Door>();
            //�h�A�������ꍇ���s�i�J�j
            if (door != null)
            {
                door.OpenDoor();
            }
        }
    }
}
