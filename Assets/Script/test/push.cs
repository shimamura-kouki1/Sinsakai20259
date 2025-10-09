using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class push : MonoBehaviour
{
    [SerializeField] private float rayDistance = 1.5f;       // �Ƌ�����o���鋗��
    [SerializeField] private LayerMask pushableLayer;        // ������Ƌ�̃��C���[
    [SerializeField] private Transform holdPoint;            // �Ƌ����������ʒu�i�v���C���[�̑O���j

    private bool isPushing = false;          // �����Ă����Ԃ��ǂ���
    private Transform pushingObj;            // �����Ă���Ƌ�� Transform
    private Vector3 originalParentPos;       // ���̉Ƌ�̈ʒu�i�߂��Ƃ��p�j
    private Transform originalParent;        // ���̐e�i�߂��Ƃ��p�j

    private Quaternion lockedRotation;     // �����Ă���Ԃ̌Œ肳�ꂽ��]�p�x

    private Transform _tr;


    void Update()
    {
        // �����L�[���o
        if (Keyboard.current.eKey.wasPressedThisFrame && !isPushing)
        {
            // �O���ɉƋ���邩 Raycast �Ŋm�F
            if (Physics.Raycast(_tr.position, _tr.forward, out RaycastHit hit, rayDistance, pushableLayer))
            {
                // �Ƌ���擾
                pushingObj = hit.collider.transform;
                originalParent = pushingObj.parent;  // ���̐e���L�^
                originalParentPos = pushingObj.position;

                // �Ƌ���v���C���[�̎q�ɂ��� holdPoint �Ɉړ�
                pushingObj.SetParent(holdPoint);
                pushingObj.localPosition = Vector3.zero;
                pushingObj.localRotation = Quaternion.identity;

                // ���݂̃v���C���[��]���Œ�
                lockedRotation = _tr.rotation;

                isPushing = true;
            }
        }

        // �����L�[�iE�L�[��������j
        if (Keyboard.current.eKey.wasReleasedThisFrame && isPushing)
        {

            // �Ƌ���v���C���[�̎q����O�����A�ʒu�͂��̂܂܈ێ�
            pushingObj.SetParent(null);

            isPushing = false;
            pushingObj = null;
        }

        // �����Ă�Ԃ͑O�������͂̂݋���
        if (isPushing)
        {
            // ��]�����b�N�i�����Ă�Ԃ͌�����ς��Ȃ��j
            _tr.rotation = lockedRotation;
            if (Keyboard.current.wKey.isPressed)
            {
                _tr.position += _tr.forward * Time.deltaTime * 2f; // �v���C���[�O�i
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
//                // ���������Ɉړ�������
//                hit.collider.transform.Translate(transform.forward * _pushSpeed * Time.deltaTime, Space.World);
//            }
//        }
//    }
//}
