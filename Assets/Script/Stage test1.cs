using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Stagetest1 : MonoBehaviour
{
    //�v���n�u�ŕ���������āA����������_���Ŕz�u����i���̎����̕������痣��鋗�����w�肷��j
    //�ʘH�͋߂��̕����ɑ΂��ĐL�΂��i��{�j
    //

    [SerializeField] private int _maxX;               // ���̌�
    [SerializeField] private int _maxZ;               // �c�̌�
    [SerializeField] private List<GameObject> _Box;   // �z�u����v���n�u�̃��X�g

    [SerializeField] private GameObject _Wall;

    private List<GameObject> _List = new();

    // Start is called before the first frame update
    void Start()
    {
        int[,] filed = new int[_maxX,_maxZ];


        for (int x = 0; x < _maxX; x=x+1)
        {
            filed[0,x] = 1;
            filed[_maxX-1,x] = 1;
        }

       // _Wall = (GameObject)Instantiate(_Wall,new Vector3())
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
