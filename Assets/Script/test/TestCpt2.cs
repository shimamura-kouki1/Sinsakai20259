using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCpt2 : MonoBehaviour
{
        [SerializeField] private Transform _origin;        // �N�_�ƂȂ�I�u�W�F�N�g�iTransform�j
        [SerializeField] private int _LineX;               // ���̌�
        [SerializeField] private int _RangeY;              // �c�̌�
        [SerializeField] private int _BoxSpace;        // �e�I�u�W�F�N�g�Ԃ̊Ԋu
        [SerializeField] private List<GameObject> _Box;    // �z�u����v���n�u�̃��X�g

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
                    // �z�u�ʒu���v�Z�i�N�_�{�I�t�Z�b�g�j
                    Vector3 spawnPos = originPos + new Vector3(x * _BoxSpace, y * _BoxSpace, 0);

                    // �v���n�u�̒����珇�Ɏ擾�i���[�v�j
                    GameObject prefab = _Box[(x * _RangeY + y) % _Box.Count];

                    GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
                    _List.Add(obj);
                }
            }
        }
}
