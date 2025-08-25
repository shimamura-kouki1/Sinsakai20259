using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Stagetest1 : MonoBehaviour
{
    //プレハブで部屋を作って、それをランダムで配置する（この時他の部屋から離れる距離を指定する）
    //通路は近くの部屋に対して伸ばす（一本）
    //

    [SerializeField] private int _maxX;               // 横の個数
    [SerializeField] private int _maxZ;               // 縦の個数
    [SerializeField] private List<GameObject> _Box;   // 配置するプレハブのリスト

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
