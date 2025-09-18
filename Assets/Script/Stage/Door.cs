using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]private Transform _door;
    [SerializeField] private Vector3 _doorRotation = new Vector3(0, 90, 0);//角度
    [SerializeField] private float _openSpeed;//開閉スピード


    private bool _isOpen = false;
    private Quaternion _openDoor;//空いてる時の角度
    private Quaternion _closeDoor;//閉まっているときの角度
    // Start is called before the first frame update
    void Start()
    {
        _closeDoor = _door.localRotation;//初期状態＝閉まっている状態

        _openDoor = Quaternion.Euler(_doorRotation);//角度＝開いてる状態
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRot = _isOpen ? _openDoor : _closeDoor;//状態によってドアの角度を選択

        _door.localRotation = Quaternion.Lerp(_door.localRotation, targetRot, _openSpeed * Time.deltaTime);//現在の状態から保管速度で目標の角度に動く
    }

    public void OpenDoor()
    {
        _isOpen = !_isOpen;//状態の反転
    }
}
