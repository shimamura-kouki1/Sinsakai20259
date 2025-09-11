using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]private Transform _door;
    [SerializeField] private Vector3 _doorRotation = new Vector3(0, 90, 0);
    [SerializeField] private float _openSpeed;


    private bool _isOpen = false;
    private Quaternion _openDoor;
    private Quaternion _closeDoor;
    // Start is called before the first frame update
    void Start()
    {
        _closeDoor = _door.localRotation;

        _openDoor = Quaternion.Euler(_doorRotation);
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetRot = _isOpen ? _openDoor : _closeDoor;

        _door.localRotation = Quaternion.Lerp(_door.localRotation, targetRot, _openSpeed * Time.deltaTime);
    }

    public void OpenDoor()
    {
        _isOpen = !_isOpen;
    }
}
