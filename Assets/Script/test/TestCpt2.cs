using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCpt2 : MonoBehaviour
{
        [SerializeField] private Transform _origin;        // 起点となるオブジェクト（Transform）
        [SerializeField] private int _LineX;               // 横の個数
        [SerializeField] private int _RangeY;              // 縦の個数
        [SerializeField] private int _BoxSpace;        // 各オブジェクト間の間隔
        [SerializeField] private List<GameObject> _Box;    // 配置するプレハブのリスト

        private List<GameObject> _List = new();

        void Start()
        {
            GenerateGridFromOrigin();
        }

        private void GenerateGridFromOrigin()
        {
            Vector3 originPos = _origin.position;

            for (int x = 0; x < _LineX; x++)
            {
                for (int y = 0; y < _RangeY; y++)
                {
                    // 配置位置を計算（起点＋オフセット）
                    Vector3 spawnPos = originPos + new Vector3(x * _BoxSpace, y * _BoxSpace, 0);

                    // プレハブの中から順に取得（ループ）
                    GameObject prefab = _Box[(x * _RangeY + y) % _Box.Count];

                    GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
                    _List.Add(obj);
                }
            }
        }
}
