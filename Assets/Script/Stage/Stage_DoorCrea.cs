using System.Collections.Generic;
using UnityEngine;

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
        GenerateDungeon();
        AddDoors();
    }

    /// <summary>
    /// 部屋を生成する
    /// </summary>
    void GenerateDungeon()
    {
        if (_roomPrefabs == null || _roomPrefabs.Length == 0) return;//プレハブが未設定なら終了

        PlaceRoom(_roomPrefabs[0], Vector3.zero);//スタートする部屋の位置（0，0，0）

        // 残りの部屋を配置
        for (int i = 1; i < _roomCount; i++)
        {
            bool placed = false;//部屋が置けたかどうか
            int attempts = 0; //試行回数（無限ループにならないために）

            while (!placed && attempts < 50)//50は試行回数
            {
                attempts++;

                // ランダムに配置済みの部屋を選択
                GameObject parentRoom = _placedRooms[Random.Range(0, _placedRooms.Count)];

                //配置する位置をランダムな方向に決定
                Vector3 dir = GetRandomDirection();

                // 新しい部屋の位置を決定　親の部屋から一部屋分ずらした位置に配置
                Vector3 newPos = parentRoom.transform.position + dir * _roomSize;

                // 既に部屋がある場所は再検索

                if (!_occupiedPositions.Contains(newPos))//newPosにまだ部屋が生成されていなければ実行
                {
                    GameObject newRoomPrefab = _roomPrefabs[Random.Range(0, _roomPrefabs.Length)];//配列からランダムに部屋を選ぶ
                    PlaceRoom(newRoomPrefab, newPos);//部屋の配置
                    placed = true;//配置できたから終了
                }
            }
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
    /// 部屋にあるアンカーをチェックして、ドアか壁を配置
    /// </summary>
    void AddDoors()
    {
        foreach (GameObject room in _placedRooms)//すべての部屋に対して
        {
            foreach (Transform anchor in room.GetComponentInChildren<Transform>())//部屋内の全ての Transform を取得
            {

                if (!anchor.name.StartsWith("Anchor")) continue;//Anchor以外はスキップ

                //アンカーの名前から方向を判定する
                Vector3 dir = Vector3.zero;
                if (anchor.name.Contains("North")) dir = Vector3.forward;
                else if (anchor.name.Contains("South")) dir = Vector3.back;
                else if (anchor.name.Contains("East")) dir = Vector3.right;
                else if (anchor.name.Contains("West")) dir = Vector3.left;

                Vector3 neighborPos = room.transform.position + dir * _roomSize;//隣の部屋の位置の座標

                // 補正後の回転を計算（South / West のときだけ - 90°回す）
                Quaternion rot = anchor.rotation;
                if (anchor.name.Contains("South") || anchor.name.Contains("West")) //なぜかSouth / Westだけ補正しないと縦に生成されてしまう
                {
                    rot *= Quaternion.Euler(0, -90, 0);//-90度回転
                }

                //隣に部屋があれば
                if (_occupiedPositions.Contains(neighborPos))
                {
                    Vector3 myPos = room.transform.position;//

                    //重複生成防止のために、座標の小さいほう部屋からドアを作る
                    if (myPos.x < neighborPos.x || (Mathf.Approximately(myPos.x, neighborPos.x) && myPos.z < neighborPos.z))
                    {
                        Instantiate(_doorPrefab, anchor.position, rot);//ドアの設置
                    }
                }
                else//隣に部屋がなかったら壁を配置
                {
                    if (_wallPrefab != null)//壁が設定されていたら
                    {
                        Instantiate(_wallPrefab, anchor.position, rot);//壁の配置
                    }
                }
            }
        }
    }
    /// <summary>
    /// 上下左右のランダム方向を返す
    /// </summary>
    Vector3 GetRandomDirection()
    {
        switch (Random.Range(0, 4))//0～3の値をランダムでとる
        {
            case 0: return Vector3.forward;  // +Z
            case 1: return Vector3.back;     // -Z
            case 2: return Vector3.left;     // -X
            default: return Vector3.right;   // +X
        }
    }
}




/// <summary>
/// 部屋の接続に応じてドアや壁を配置
/// </summary>
//void AddDoors()
//{
//    foreach (GameObject room in _placedRooms)//生成したすべての部屋に対して
//    {
//        Vector3 roomPos = room.transform.position;//ポジション取得

//        TryPlaceDoorOrWall(roomPos, Vector3.forward);
//        TryPlaceDoorOrWall(roomPos, Vector3.back);
//        TryPlaceDoorOrWall(roomPos, Vector3.left);
//        TryPlaceDoorOrWall(roomPos, Vector3.right);
//    }
//}


/// <summary>
/// となりに部屋があるかチェックしてドアか壁を置く
/// </summary>
//void TryPlaceDoorOrWall(Vector3 roomPos, Vector3 dir)