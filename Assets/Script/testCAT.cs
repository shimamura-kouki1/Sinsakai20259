using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCAT : MonoBehaviour
{
    [SerializeField] private int _LineX;
    [SerializeField] private int _RangeY;
    [SerializeField] private int _Speed;
    [SerializeField] private int _BoxSpace;

    [SerializeField]
    List<GameObject> _Box;

    private List<GameObject> _List = new();

    void Start()
    {
        Application.targetFrameRate = 60;

        Vector3 offset = new Vector3(-_LineX * _BoxSpace * 2f, -_RangeY * _BoxSpace * 2f, 0);

        for (int i = 0; i < _RangeY * _LineX; i++)
        {
            int UnkoA = i % (_RangeY * 2);
            int UnkoB = _RangeY - 1 - (i % (_RangeY));

            int x = i / _RangeY;
            int y = (i % (_RangeY * 2) < _RangeY) ? UnkoA : UnkoB;

            Vector3 pos = new Vector3(x * _BoxSpace, y * _BoxSpace, 0) + offset;

            _List.Add(Instantiate(_Box[i % _Box.Count], pos, Quaternion.identity));
        }
    }
}
