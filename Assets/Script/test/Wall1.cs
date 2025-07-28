using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall1 : MonoBehaviour
{
    [SerializeField] private int _LineX;
    [SerializeField] private int _RangeY;
    [SerializeField] private int _Speed;
    [SerializeField] private int _BoxSpace;
    [SerializeField] private int _pos;
    [SerializeField] private int _pos2;
    [SerializeField] private int _origin;

    [SerializeField]
    List<GameObject> _Box;
    private int _Count = 0;
    private int _Reset = 0;
    private List<GameObject> _List = new();

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        for (int i = 0; _RangeY < _RangeY * _LineX; _pos++)
        {
            int UnkoA = i % (_RangeY * 2);
            int UnkoB = _RangeY - 1 - (i % (_RangeY));

            if (i % (_RangeY * 2) < _RangeY)
            {
                _List.Add(Instantiate(_Box[i % _Box.Count],new Vector3(i / _RangeY * _BoxSpace, UnkoA * _BoxSpace, 0), Quaternion.identity));
            }
            else
            {
                _List.Add(Instantiate(_Box[i % _Box.Count], new Vector3(i / _RangeY * _BoxSpace, UnkoB * _BoxSpace, 0), Quaternion.identity));
            }
        }
    }
}
