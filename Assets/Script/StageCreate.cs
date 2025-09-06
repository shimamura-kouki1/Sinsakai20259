using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StageCreate : MonoBehaviour
{
    [SerializeField] private GameObject[] _roomprafab;

    [SerializeField] private int stageWidth = 3;       // 横に並べる数
    [SerializeField] private int stageHeight = 1;      // 縦に並べる数
    [SerializeField] private float spacing = 2f;      // 部屋の間隔

    private List<Vector3> occupiedPositions = new List<Vector3>(); // 配置済み座標
    
    // Start is called before the first frame update
    void Start()
    {
        Shuffle(_roomprafab);

        for (int x = 0; x < stageWidth; x++)
        {
            for (int z = 0; z < stageHeight; z++)
            {
                    Instantiate(_roomprafab[(x + z) % _roomprafab.Length], new Vector3(x * spacing, 0, z * spacing), Quaternion.identity);
            }
        }
    }

    private void Shuffle<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1); // 0〜i のランダム値
            (array[i], array[j]) = (array[j], array[i]); // 要素を入れ替え
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
