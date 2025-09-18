using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]private Transform _door;
    [SerializeField] private Vector3 _doorRotation = new Vector3(0, 90, 0);//�p�x
    [SerializeField] private float _openSpeed;//�J�X�s�[�h


    private bool _isOpen = false;
    private Quaternion _openDoor;//�󂢂Ă鎞�̊p�x
    private Quaternion _closeDoor;//�܂��Ă���Ƃ��̊p�x
    // Start is called before the first frame update
    void Start()
    {
        _closeDoor = _door.localRotation;//������ԁ��܂��Ă�����

        _openDoor = Quaternion.Euler(_doorRotation);//�p�x���J���Ă���
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRot = _isOpen ? _openDoor : _closeDoor;//��Ԃɂ���ăh�A�̊p�x��I��

        _door.localRotation = Quaternion.Lerp(_door.localRotation, targetRot, _openSpeed * Time.deltaTime);//���݂̏�Ԃ���ۊǑ��x�ŖڕW�̊p�x�ɓ���
    }

    public void OpenDoor()
    {
        _isOpen = !_isOpen;//��Ԃ̔��]
    }
}
