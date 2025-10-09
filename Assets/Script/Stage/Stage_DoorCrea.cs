using System.Collections.Generic;
using UnityEngine;

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
        GenerateDungeon();
        AddDoors();
    }

    /// <summary>
    /// �����𐶐�����
    /// </summary>
    void GenerateDungeon()
    {
        if (_roomPrefabs == null || _roomPrefabs.Length == 0) return;//�v���n�u�����ݒ�Ȃ�I��

        PlaceRoom(_roomPrefabs[0], Vector3.zero);//�X�^�[�g���镔���̈ʒu�i0�C0�C0�j

        // �c��̕�����z�u
        for (int i = 1; i < _roomCount; i++)
        {
            bool placed = false;//�������u�������ǂ���
            int attempts = 0; //���s�񐔁i�������[�v�ɂȂ�Ȃ����߂Ɂj

            while (!placed && attempts < 50)//50�͎��s��
            {
                attempts++;

                // �����_���ɔz�u�ς݂̕�����I��
                GameObject parentRoom = _placedRooms[Random.Range(0, _placedRooms.Count)];

                //�z�u����ʒu�������_���ȕ����Ɍ���
                Vector3 dir = GetRandomDirection();

                // �V���������̈ʒu������@�e�̕�������ꕔ�������炵���ʒu�ɔz�u
                Vector3 newPos = parentRoom.transform.position + dir * _roomSize;

                // ���ɕ���������ꏊ�͍Č���

                if (!_occupiedPositions.Contains(newPos))//newPos�ɂ܂���������������Ă��Ȃ���Ύ��s
                {
                    GameObject newRoomPrefab = _roomPrefabs[Random.Range(0, _roomPrefabs.Length)];//�z�񂩂烉���_���ɕ�����I��
                    PlaceRoom(newRoomPrefab, newPos);//�����̔z�u
                    placed = true;//�z�u�ł�������I��
                }
            }
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
        foreach (GameObject room in _placedRooms)//���ׂĂ̕����ɑ΂���
        {
            foreach (Transform anchor in room.GetComponentInChildren<Transform>())//�������̑S�Ă� Transform ���擾
            {

                if (!anchor.name.StartsWith("Anchor")) continue;//Anchor�ȊO�̓X�L�b�v

                //�A���J�[�̖��O��������𔻒肷��
                Vector3 dir = Vector3.zero;
                if (anchor.name.Contains("North")) dir = Vector3.forward;
                else if (anchor.name.Contains("South")) dir = Vector3.back;
                else if (anchor.name.Contains("East")) dir = Vector3.right;
                else if (anchor.name.Contains("West")) dir = Vector3.left;

                Vector3 neighborPos = room.transform.position + dir * _roomSize;//�ׂ̕����̈ʒu�̍��W

                // �␳��̉�]���v�Z�iSouth / West �̂Ƃ����� - 90���񂷁j
                Quaternion rot = anchor.rotation;
                if (anchor.name.Contains("South") || anchor.name.Contains("West")) //�Ȃ���South / West�����␳���Ȃ��Əc�ɐ�������Ă��܂�
                {
                    rot *= Quaternion.Euler(0, -90, 0);//-90�x��]
                }

                //�ׂɕ����������
                if (_occupiedPositions.Contains(neighborPos))
                {
                    Vector3 myPos = room.transform.position;//

                    //�d�������h�~�̂��߂ɁA���W�̏������ق���������h�A�����
                    if (myPos.x < neighborPos.x || (Mathf.Approximately(myPos.x, neighborPos.x) && myPos.z < neighborPos.z))
                    {
                        Instantiate(_doorPrefab, anchor.position, rot);//�h�A�̐ݒu
                    }
                }
                else//�ׂɕ������Ȃ�������ǂ�z�u
                {
                    if (_wallPrefab != null)//�ǂ��ݒ肳��Ă�����
                    {
                        Instantiate(_wallPrefab, anchor.position, rot);//�ǂ̔z�u
                    }
                }
            }
        }
    }
    /// <summary>
    /// �㉺���E�̃����_��������Ԃ�
    /// </summary>
    Vector3 GetRandomDirection()
    {
        switch (Random.Range(0, 4))//0�`3�̒l�������_���łƂ�
        {
            case 0: return Vector3.forward;  // +Z
            case 1: return Vector3.back;     // -Z
            case 2: return Vector3.left;     // -X
            default: return Vector3.right;   // +X
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