using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _playerBaseSpeed = 5f; //プレイヤーの移動速度 
    [SerializeField] private float _lifetime = 10f;       //エネミーの寿命
    [SerializeField] private float _rotationSpeed = 180f;//振り返る角度
    [SerializeField] private float _enemySpeed = 1.3f;//エネミーのスピード倍率
    [SerializeField] private float _rayDistance = 1.5f; //オブジェクトの検知範囲

    private Rigidbody _rb;
    private float _speed;//エネミーの総合的なスピード
    private float _timer;　//寿命をカウントするためのもの
    private bool _isRotation = false;//回転中かどうか
    private int _turnDirection = 1;    // -1=左, 1=右
    private Quaternion _targetRot;     // 旋回先の角度

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _speed = _playerBaseSpeed * _enemySpeed;

        _timer = _lifetime;//初期化
    }

    private void Update()
    {
        _timer -= Time.deltaTime;//1秒ごとにカウントを減らして、0になったら消滅
        if (_timer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (_isRotation)
        {
            // 旋回処理
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRot, _rotationSpeed * Time.fixedDeltaTime);

            // 目標角度にほぼ到達したら旋回終了
            if (Quaternion.Angle(transform.rotation, _targetRot) < 1f)
            {
                _isRotation = false;
            }

            return; // 旋回中は前進しない
        }

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _rayDistance))//前方にレイの生成
        {
            if (hit.collider.CompareTag("Furniture"))//ヒットしたコライダーのタグが一致したら
            {
                _turnDirection = Random.Range(0, 2) == 0 ? 1 : -1; //左右ランダムに動くように 1 = 右, -1 = 左

                // 直角方向を計算している
                _targetRot = Quaternion.LookRotation(Vector3.Cross(transform.forward, Vector3.up).normalized * _turnDirection, Vector3.up);
            }
            _isRotation = true;
            return;//旋回中は移動しないように
        }
        _rb.MovePosition(_rb.position + transform.forward * _speed * Time.fixedDeltaTime);//前進し続ける
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 reflectDir = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
        transform.rotation = Quaternion.LookRotation(reflectDir, Vector3.up);
    }
}
