using UnityEngine;
using System.Collections.Generic;

public class RoomBasedDungeon : MonoBehaviour
{
    [Header("Prefab Settings")]
    [SerializeField] private GameObject[] _roomPrefabs; // 部屋のプレハブ（複数種類）
    [SerializeField] private GameObject _doorPrefab;    // ドアPrefab
    [SerializeField] private GameObject _wallPrefab;    // 塞ぐ用の壁Prefab

    [Header("Dungeon Settings")]
    [SerializeField] private int _roomCount = 10; // 生成する部屋の数
    [SerializeField] private int _roomSize = 10;  // 1部屋の大きさ

    // 内部管理用
    private readonly List<Vector3> _occupiedPositions = new List<Vector3>(); // 部屋を置いた座標
    private readonly List<GameObject> _placedRooms = new List<GameObject>(); // 実際に生成した部屋

    void Start()
    {
        GenerateDungeon();//部屋の作成
        AddDoors();//ドアの追加
    }

    /// <summary>
    /// 部屋を生成する
    /// </summary>
    void GenerateDungeon()
    {
        if (_roomPrefabs == null || _roomPrefabs.Length == 0) return;//プレハブが未設定なら終了

        // スタート部屋を配置
        Vector3 startPos = Vector3.zero;
        PlaceRoom(_roomPrefabs[0],startPos);//スタートする部屋の位置

        // 残りの部屋を配置
        for (int i = 1; i < _roomCount; i++)
        {
            bool placed = false;
            int attempts = 0;

            while (!placed && attempts < 50)//試行回数が50まで
            {
                attempts++;

                // ランダムに既存の部屋を選択
                GameObject parentRoom = _placedRooms[Random.Range(0, _placedRooms.Count)];
                Vector3 dir = GetRandomDirection();//ランダムな方向を決定

                // 新しい部屋の位置を決定
                Vector3 newPos = parentRoom.transform.position + dir * _roomSize;//一部屋分ずらした位置に配置

                // 既に部屋がある場所はスキップ
                if (!_occupiedPositions.Contains(newPos))//newPosにまだ部屋が生成されていなければ実行
                {
                    GameObject newRoomPrefab = _roomPrefabs[Random.Range(0, _roomPrefabs.Length)];//ランダムに部屋を選ぶ
                    PlaceRoom(newRoomPrefab, newPos);//部屋の配置
                    placed = true;//配置できたから終了
                }
            }
        }
    }

    /// <summary>
    /// 上下左右のランダム方向を返す
    /// </summary>
    Vector3 GetRandomDirection()//配置する部屋の位置を四方からランダムにとる
    {
        switch (Random.Range(0, 4))//0～3の値をランダムでとる
        {
            case 0: return Vector3.forward;  // +Z
            case 1: return Vector3.back;     // -Z
            case 2: return Vector3.left;     // -X
            default: return Vector3.right;   // +X
        }
    }

    /// <summary>
    /// 部屋を1つ配置
    /// </summary>
    void PlaceRoom(GameObject prefab, Vector3 pos)
    {
        GameObject room = Instantiate(prefab, pos, Quaternion.identity);//指定された位置に配置
        _placedRooms.Add(room);//配置リストに追加
        _occupiedPositions.Add(pos);//座標リストに追加
    }

    /// <summary>
    /// 部屋の接続に応じてドアや壁を配置
    /// </summary>
    void AddDoors()
    {
        foreach (GameObject room in _placedRooms)//生成したすべての部屋に対して
        {
            Vector3 roomPos = room.transform.position;//ポジション取得

            TryPlaceDoorOrWall(roomPos, Vector3.forward);
            TryPlaceDoorOrWall(roomPos, Vector3.back);
            TryPlaceDoorOrWall(roomPos, Vector3.left);
            TryPlaceDoorOrWall(roomPos, Vector3.right);
        }
    }

    /// <summary>
    /// となりに部屋があるかチェックしてドアか壁を置く
    /// </summary>
    void TryPlaceDoorOrWall(Vector3 roomPos, Vector3 dir)
    {
        Vector3 neighborPos = roomPos + dir * _roomSize;//隣の部屋の位置の座標
        Vector3 doorPos = roomPos + dir * (_roomSize / 2f); //ドアの置く位置

        Quaternion rot = Quaternion.identity;//回転の初期化
        if (dir == Vector3.back) rot = Quaternion.Euler(0, 180, 0);//ドアを置く位置によって回転する度数
        if (dir == Vector3.left) rot = Quaternion.Euler(0, -90, 0);
        if (dir == Vector3.right) rot = Quaternion.Euler(0, 90, 0);

        if (_occupiedPositions.Contains(neighborPos))//隣に部屋があれば
        {
            if (_doorPrefab != null)//変数にドアが設定されていたら
            {
                Instantiate(_doorPrefab, doorPos, rot);//ドアの設置
            }
        }
        else//部屋がなかったら
        {
            if (_wallPrefab != null)//壁が設定されていたら
            {
                Instantiate(_wallPrefab, doorPos, rot);//壁の配置
            }
        }
    }
}