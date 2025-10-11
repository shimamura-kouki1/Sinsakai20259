using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))] // 例として CharacterController を使った移動
public class PushAligner : MonoBehaviour
{
    [Header("Input (assign from Input Actions)")]
    [Tooltip("押すボタンの InputAction を Inspector で割り当てる（例: Player/Push -> 'E'）")]
    public InputActionReference pushActionRef;

    [Header("Detection")]
    [SerializeField] private float rayDistance = 1.8f;          // 家具検出距離
    [SerializeField] private LayerMask pushableLayer;          // 押せる家具レイヤー

    [Header("Align (player positioning)")]
    [SerializeField] private float alignDistance = 0.6f;       // 家具の前に立つ距離
    [SerializeField] private float alignMoveSpeed = 6f;        // 位置合わせの移動速度（高いほど即座に移動）
    [SerializeField] private float alignRotateSpeed = 720f;    // 向き合わせの回転速度（deg/sec）

    [Header("Pushing")]
    [SerializeField] private float pushForce = 200f;           // 押す力（Rigidbody に加える力）
    [SerializeField] private bool useContinuousForce = true;   // ホールド中に力を加え続けるか

    // 内部状態
    private CharacterController _cc;
    private bool _isAligning = false;
    private bool _isPushing = false;
    private Rigidbody _targetRb;
    private Transform _targetTransform;
    private Vector3 _alignTargetPos;
    private Quaternion _alignTargetRot;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        if (pushActionRef == null) Debug.LogWarning("pushActionRef が未割り当てです。Inspector で割り当ててください。");
    }

    private void OnEnable()
    {
        if (pushActionRef != null)
        {
            pushActionRef.action.started += OnPushStarted;
            pushActionRef.action.performed += OnPushPerformed;   // ホールド中のイベント（必要なら）
            pushActionRef.action.canceled += OnPushCanceled;
            pushActionRef.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (pushActionRef != null)
        {
            pushActionRef.action.started -= OnPushStarted;
            pushActionRef.action.performed -= OnPushPerformed;
            pushActionRef.action.canceled -= OnPushCanceled;
            pushActionRef.action.Disable();
        }
    }

    // 押すボタンを押した瞬間
    private void OnPushStarted(InputAction.CallbackContext ctx)
    {
        if (_isAligning || _isPushing) return; // 既に処理中なら無視

        // 前方にレイキャストして家具を探す（プレイヤーの目線／胸位置から）
        Vector3 origin = transform.position + Vector3.up * 0.8f; // 調整しやすい
        if (Physics.Raycast(origin, transform.forward, out RaycastHit hit, rayDistance, pushableLayer, QueryTriggerInteraction.Ignore))
        {
            _targetTransform = hit.collider.transform;
            _targetRb = hit.rigidbody;  // ← 修正済み
            if (_targetRb == null)
            {
                Debug.Log("押そうとしたオブジェクトに Rigidbody がアタッチされていません。");
                _targetTransform = null;
                return;
            }

            // 立つべき位置と向きを計算
            Vector3 directionToObject = (_targetTransform.position - transform.position);
            directionToObject.y = 0f;
            if (directionToObject.sqrMagnitude < 0.0001f)
            {
                directionToObject = transform.forward; // 近すぎたら今の前方
            }
            Vector3 objectForward = -directionToObject.normalized; // オブジェクトに顔を向ける向き
            _alignTargetPos = _targetTransform.position + objectForward * alignDistance;
            _alignTargetPos.y = transform.position.y; // 同じ高さにする（必要なら調整）
            _alignTargetRot = Quaternion.LookRotation((_targetTransform.position - _alignTargetPos).normalized, Vector3.up);

            // 開始
            _isAligning = true;
            StartCoroutine(AlignThenStartPushing());
        }
    }

    // performed はホールド時に呼ばれることがある（ここでは特に使わないが残してある）
    private void OnPushPerformed(InputAction.CallbackContext ctx)
    {
        // 何もしない（代わりに固定更新で押力を与える）
    }

    // 押すボタンを離したとき
    private void OnPushCanceled(InputAction.CallbackContext ctx)
    {
        // 押すのを止める
        _isPushing = false;
        _isAligning = false;
        _targetRb = null;
        _targetTransform = null;
    }

    // 位置合わせしてから押し状態へ
    private IEnumerator AlignThenStartPushing()
    {
        // 位置合わせフェーズ（滑らかに移動＋回転）
        float timeout = 0.5f; // 無限ループを避けるための簡単なタイムアウト（必要なら調整）
        float timer = 0f;

        while (timer < timeout)
        {
            // 位置は CharacterController を使って滑らかに移動（衝突は CharacterController に任せる）
            Vector3 toTarget = _alignTargetPos - transform.position;
            Vector3 move = Vector3.zero;
            if (toTarget.magnitude > 0.01f)
            {
                Vector3 desiredMove = toTarget.normalized * alignMoveSpeed * Time.deltaTime;
                if (desiredMove.magnitude > toTarget.magnitude) desiredMove = toTarget;
                move = desiredMove;
                _cc.Move(move);
            }

            // 回転
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _alignTargetRot, alignRotateSpeed * Time.deltaTime);

            // 近づいたら終了
            if (toTarget.magnitude <= 0.05f && Quaternion.Angle(transform.rotation, _alignTargetRot) <= 2f)
            {
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // 位置合わせ完了 → 押す状態開始（ホールドしている限り押す）
        _isAligning = false;
        _isPushing = true;
    }

    private void FixedUpdate()
    {
        if (_isPushing && _targetRb != null)
        {
            // プレイヤーが向いている方向（水平成分のみ）
            Vector3 pushDir = transform.forward;
            pushDir.y = 0f;
            pushDir.Normalize();

            if (useContinuousForce)
            {
                // 物理力を加える（拡張性のため ForceMode を選べるがここでは Force）
                _targetRb.AddForce(pushDir * pushForce * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
            else
            {
                // 1回だけのインパルスにしたい場合は別ロジックにする
                _targetRb.AddForce(pushDir * pushForce, ForceMode.Impulse);
                _isPushing = false; // 一度押したら終了（用途によって変えてください）
            }
        }
    }

    // デバッグ用：視覚的に見やすくする
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 origin = transform.position + Vector3.up * 0.8f;
        Gizmos.DrawLine(origin, origin + transform.forward * rayDistance);
        if (_targetTransform != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(_alignTargetPos, 0.05f);
            Gizmos.DrawLine(transform.position, _alignTargetPos);
        }
    }
}
