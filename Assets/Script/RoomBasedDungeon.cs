using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBasedDungeon : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] _roomprafab;

    [SerializeField] private int _roomCount = 10; //部屋のカウント数
    [SerializeField] private int _roomSize = 10;

    [SerializeField] private int _stageWidth = 3;       // 横に並べる数
    [SerializeField] private int _stageHeight = 1;      // 縦に並べる数
    [SerializeField] private float _spacing = 2f;      // 部屋の間隔

    private List<Vector3> _occupiedPositions = new List<Vector3>();
    private List<GameObject> _Placeroom = new List<GameObject>();

    void Start()
    {
        GenerateDungeon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void GenerateDungeon()
    {
        if (_roomprafab.Length == 0) return;

            Vector3 stratpos = Vector3.zero;
            PlaceRoom(_roomprafab[0], stratpos);

            for (int i = 1; i < _roomCount; i++)
            {
                bool placed = false;
                int attempts = 0;

            while(!placed && attempts <50)
            {
                GameObject _Placeroom = _Placeroom[Random.Range(0,_Placeroom.Count)];
                Vector3 dir = GetRandomDirection();

                Vector3 newPos = _Placeroom.transform.position + dir * _roomSize;
            }
            if (!_occupiedPositions.Contains(newPos))
            {
                GameObject newRoomPrefab = _roomprafab[Random.Range(0,_roomprafab.Length)];
                PlaceRoom(newRoomPrefab, newPos);
                placed = true;
            }

        }

    }

    Vector3 GetRandomDirection()
    {
        // 上下左右（X,Z軸）に1マス移動
        int rand = Random.Range(0, 4);
        switch (rand)
        {
            case 0: return Vector3.forward;  // +Z
            case 1: return Vector3.back;     // -Z
            case 2: return Vector3.left;     // -X
            default: return Vector3.right;   // +X
        }
    }

    void PlaceRoom(GameObject prefab, Vector3 pos)
    {
        Instantiate(prefab, pos, Quaternion.identity);
        _Placeroom.Add(prefab);
        _occupiedPositions.Add(pos);
    }
}
