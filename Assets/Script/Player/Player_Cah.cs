using UnityEngine;
using UnityEngine.InputSystem;


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
    [SerializeField] private Light _flashlight;//ライト
    private bool _flashlightOn = false;//オン・オフ

    [Header("Player Action")]
    [SerializeField] private float _rayDistance = 3f; // 手が届く距離


    private CharacterController _controller;//プレイヤー移動用のCharacterController
    private Vector2 _moveInput;//移動入力
    private Vector2 _lookInput;//視点入力
    private float _verticalRotation;//上下の視点　回転の角度
    private float _yaw;              // 左右視点の角度
    private Vector3 _velocity;//重力のベクトル
    private bool _isSprinting;//ダッシュ中かどうかのフラグ


    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>(); // CharacterControllerを取得
        Cursor.lockState = CursorLockMode.Locked; // マウスカーソルを中央に固定

        if (_flashlight != null)//ライトが設定されているか
        {
            _flashlight.enabled = false;// ライトを初期状態では消灯にする
        }
    }

    // Update is called once per frame
    void Update()
    {
        // === 移動処理 ===
        float speed = _isSprinting ? _sprintSpeed : _walkSpeed;//歩行かダッシュかを判別する
        Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;// 入力方向をワールド空間に変換
        _controller.Move(move * speed * Time.deltaTime);//　移動を反映

        // === 重力を適用 ===　重力がないとキャラクターが浮いたままになる
        if (_controller.isGrounded && _velocity.y < 0)// 地面に接地していて、かつ下方向の速度がある場合
        {
            _velocity.y = -2f; // 地面に押し付ける
        }
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);

        // === 視点回転 === 

        _yaw += _lookInput.x * _mouseSensitivity;
        _verticalRotation -= _lookInput.y * _mouseSensitivity;// 上下回転を入力に応じて増減
        _verticalRotation = Mathf.Clamp(_verticalRotation, -80f, 80f); // 上下視点の角度制限

        transform.rotation = Quaternion.Euler(0f, _yaw, 0f); // プレイヤー本体は左右
        _cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f); // カメラは上下
    }

    // ==== Input System コールバック ====
    public void Onmove(InputAction.CallbackContext context)// 移動入力を受け取る
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)// 視点移動入力を受け取る
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context) // ダッシュ入力を受け取る
    {
        if (context.performed) _isSprinting = true;// 押された瞬間にダッシュ開始
        if (context.canceled) _isSprinting = false;// 離されたらダッシュ終了
    }

    public void OnLight(InputAction.CallbackContext context)// 懐中電灯のON/OFF切り替え
    {
        if (_flashlight == null) return; // ぬるチェック

        if (context.performed)// ボタンが押された瞬間
        {
            _flashlightOn = !_flashlightOn;//bool状態の反転
            _flashlight.enabled = _flashlightOn;//コンポーネントの有効/無効の状態反転
        }
    }

    public void OnAction(InputAction.CallbackContext context)//アクションの入力受付
    {
        //押された瞬間以外リターン
        if (!context.performed) return;

        // カメラ中央からRayを飛ばす+レイの距離
        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, _rayDistance))
        {
            //当たったオブジェクトがドアに当たったか調べる
            Door door = hit.collider.GetComponentInParent<Door>();
            //ドアだった場合実行（開閉）
            if (door != null)
            {
                door.OpenDoor();
            }
        }
    }
}
