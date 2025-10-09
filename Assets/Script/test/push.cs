using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class push : MonoBehaviour
{
    [SerializeField] private float rayDistance = 1.5f;       // 家具を検出する距離
    [SerializeField] private LayerMask pushableLayer;        // 押せる家具のレイヤー
    [SerializeField] private Transform holdPoint;            // 家具をくっつける位置（プレイヤーの前方）

    private bool isPushing = false;          // 押している状態かどうか
    private Transform pushingObj;            // 押している家具の Transform
    private Vector3 originalParentPos;       // 元の家具の位置（戻すとき用）
    private Transform originalParent;        // 元の親（戻すとき用）

    private Quaternion lockedRotation;     // 押している間の固定された回転角度

    private Transform _tr;


    void Update()
    {
        // 押すキー検出
        if (Keyboard.current.eKey.wasPressedThisFrame && !isPushing)
        {
            // 前方に家具があるか Raycast で確認
            if (Physics.Raycast(_tr.position, _tr.forward, out RaycastHit hit, rayDistance, pushableLayer))
            {
                // 家具を取得
                pushingObj = hit.collider.transform;
                originalParent = pushingObj.parent;  // 元の親を記録
                originalParentPos = pushingObj.position;

                // 家具をプレイヤーの子にして holdPoint に移動
                pushingObj.SetParent(holdPoint);
                pushingObj.localPosition = Vector3.zero;
                pushingObj.localRotation = Quaternion.identity;

                // 現在のプレイヤー回転を固定
                lockedRotation = _tr.rotation;

                isPushing = true;
            }
        }

        // 離すキー（Eキー離したら）
        if (Keyboard.current.eKey.wasReleasedThisFrame && isPushing)
        {

            // 家具をプレイヤーの子から外すが、位置はそのまま維持
            pushingObj.SetParent(null);

            isPushing = false;
            pushingObj = null;
        }

        // 押してる間は前方向入力のみ許可
        if (isPushing)
        {
            // 回転をロック（押してる間は向きを変えない）
            _tr.rotation = lockedRotation;
            if (Keyboard.current.wKey.isPressed)
            {
                _tr.position += _tr.forward * Time.deltaTime * 2f; // プレイヤー前進
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(_tr.position, _tr.forward * rayDistance);
    }
}

//public void OnPushActoin(InputAction.CallbackContext context)
//    {
//        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _pushDistance))
//        {
//            if (hit.collider.CompareTag("Pushable"))
//            {
//                // 押す方向に移動させる
//                hit.collider.transform.Translate(transform.forward * _pushSpeed * Time.deltaTime, Space.World);
//            }
//        }
//    }
//}
