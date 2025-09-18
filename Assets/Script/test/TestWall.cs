using System.Collections.Generic;
using UnityEngine;

public class TestWall : MonoBehaviour
{
    [SerializeField] private int _LineX;
    [SerializeField] private int _RangeY;
    [SerializeField] private int _Speed;
    [SerializeField] private int _BoxSpace;

    [SerializeField]
    List<GameObject> _Box;
    private int _Count = 0;
    private int _Reset = 0;
    private List<GameObject> _List = new();

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        for (int i = 0; i < _RangeY * _LineX; i++)
        {
            int UnkoA = i % (_RangeY * 2);
            int UnkoB = _RangeY - 1 - (i % (_RangeY));

            if (i % (_RangeY * 2) < _RangeY)
            {
                _List.Add(Instantiate(_Box[i % _Box.Count], new Vector3(i / _RangeY * _BoxSpace, UnkoA * _BoxSpace, 0), Quaternion.identity));
            }
            else
            {
                _List.Add(Instantiate(_Box[i % _Box.Count], new Vector3(i / _RangeY * _BoxSpace, UnkoB * _BoxSpace, 0), Quaternion.identity));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % _Speed == 0)
        {
            if (_Count != _LineX * _RangeY && _Count != -1)
            {
                if (_List[_Count].transform.localScale != Vector3.one * 0.5f)
                {
                    _List[_Count].transform.localScale = Vector3.one * 0.5f;
                }
                else
                {
                    _List[_Count].transform.localScale = Vector3.one;
                }
            }

            if (_Reset % 2 == 0)
            {
                _Count++;
            }
            else
            {
                _Count--;
            }

            if (_Count == _LineX * _RangeY || _Count == -1)
            {
                _Reset++;
            }
        }
    }
}
