using UnityEngine;
using System.Collections.Generic;

public class RoomBasedDungeon : MonoBehaviour
{
    [Header("Prefab Settings")]
    [SerializeField] private GameObject[] _roomPrefabs; // �����̃v���n�u�i������ށj
    [SerializeField] private GameObject _doorPrefab;    // �h�APrefab
    [SerializeField] private GameObject _wallPrefab;    // �ǂ��p�̕�Prefab

    [Header("Dungeon Settings")]
    [SerializeField] private int _roomCount = 10; // �������镔���̐�
    [SerializeField] private int _roomSize = 10;  // 1�����̑傫��

    // �����Ǘ��p
    private readonly List<Vector3> _occupiedPositions = new List<Vector3>(); // ������u�������W
    private readonly List<GameObject> _placedRooms = new List<GameObject>(); // ���ۂɐ�����������

    void Start()
    {
        GenerateDungeon();//�����̍쐬
        AddDoors();//�h�A�̒ǉ�
    }

    /// <summary>
    /// �����𐶐�����
    /// </summary>
    void GenerateDungeon()
    {
        if (_roomPrefabs == null || _roomPrefabs.Length == 0) return;//�v���n�u�����ݒ�Ȃ�I��

        // �X�^�[�g������z�u
        Vector3 startPos = Vector3.zero;
        PlaceRoom(_roomPrefabs[0],startPos);//�X�^�[�g���镔���̈ʒu

        // �c��̕�����z�u
        for (int i = 1; i < _roomCount; i++)
        {
            bool placed = false;
            int attempts = 0;

            while (!placed && attempts < 50)//���s�񐔂�50�܂�
            {
                attempts++;

                // �����_���Ɋ����̕�����I��
                GameObject parentRoom = _placedRooms[Random.Range(0, _placedRooms.Count)];
                Vector3 dir = GetRandomDirection();//�����_���ȕ���������

                // �V���������̈ʒu������
                Vector3 newPos = parentRoom.transform.position + dir * _roomSize;//�ꕔ�������炵���ʒu�ɔz�u

                // ���ɕ���������ꏊ�̓X�L�b�v
                if (!_occupiedPositions.Contains(newPos))//newPos�ɂ܂���������������Ă��Ȃ���Ύ��s
                {
                    GameObject newRoomPrefab = _roomPrefabs[Random.Range(0, _roomPrefabs.Length)];//�����_���ɕ�����I��
                    PlaceRoom(newRoomPrefab, newPos);//�����̔z�u
                    placed = true;//�z�u�ł�������I��
                }
            }
        }
    }

    /// <summary>
    /// �㉺���E�̃����_��������Ԃ�
    /// </summary>
    Vector3 GetRandomDirection()//�z�u���镔���̈ʒu���l�����烉���_���ɂƂ�
    {
        switch (Random.Range(0, 4))//0�`3�̒l�������_���łƂ�
        {
            case 0: return Vector3.forward;  // +Z
            case 1: return Vector3.back;     // -Z
            case 2: return Vector3.left;     // -X
            default: return Vector3.right;   // +X
        }
    }

    /// <summary>
    /// ������1�z�u
    /// </summary>
    void PlaceRoom(GameObject prefab, Vector3 pos)
    {
        GameObject room = Instantiate(prefab, pos, Quaternion.identity);//�w�肳�ꂽ�ʒu�ɔz�u
        _placedRooms.Add(room);//�z�u���X�g�ɒǉ�
        _occupiedPositions.Add(pos);//���W���X�g�ɒǉ�
    }

    /// <summary>
    /// �����ɂ���A���J�[���`�F�b�N���āA�h�A���ǂ�z�u
    /// </summary>
    void AddDoors()
    {
        foreach (GameObject room in _placedRooms)
        {
            foreach (Transform anchor in room.GetComponentInChildren<Transform>())
            {

                if (!anchor.name.StartsWith("Anchor")) continue;
                {

                    Vector3 dir = Vector3.zero;
                    if(anchor.name.Contains("North"))dir = Vector3.forward;
                    else if (anchor.name.Contains("South")) dir= Vector3.back;
                    else if (anchor.name.Contains("East")) dir = Vector3.right;
                    else if (anchor.name.Contains("West")) dir = Vector3.left; 

                    Vector3 neighborPos = room.transform.position + dir * _roomSize;//�ׂ̕����̈ʒu�̍��W

                    if (_occupiedPositions.Contains(neighborPos))//�ׂɕ����������
                    {
                        if (_doorPrefab != null)//�ϐ��Ƀh�A���ݒ肳��Ă�����
                        {
                            Quaternion correction = Quaternion.Euler(0, 90, 0);
                            Instantiate(_doorPrefab, anchor.position, anchor.rotation * correction, anchor.transform);//�h�A�̐ݒu
                        }
                    }
                    else//�������Ȃ�������
                    {
                        if (_wallPrefab != null)//�ǂ��ݒ肳��Ă�����
                        {
                            Instantiate(_wallPrefab, anchor.position, anchor.rotation);//�ǂ̔z�u
                        }
                    }
                }
            }
        }
    }
}




/// <summary>
/// �����̐ڑ��ɉ����ăh�A��ǂ�z�u
/// </summary>
//void AddDoors()
//{
//    foreach (GameObject room in _placedRooms)//�����������ׂĂ̕����ɑ΂���
//    {
//        Vector3 roomPos = room.transform.position;//�|�W�V�����擾

//        TryPlaceDoorOrWall(roomPos, Vector3.forward);
//        TryPlaceDoorOrWall(roomPos, Vector3.back);
//        TryPlaceDoorOrWall(roomPos, Vector3.left);
//        TryPlaceDoorOrWall(roomPos, Vector3.right);
//    }
//}


/// <summary>
/// �ƂȂ�ɕ��������邩�`�F�b�N���ăh�A���ǂ�u��
/// </summary>
//void TryPlaceDoorOrWall(Vector3 roomPos, Vector3 dir)