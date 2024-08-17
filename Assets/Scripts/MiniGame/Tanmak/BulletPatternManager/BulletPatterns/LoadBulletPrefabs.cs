using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanmak
{
    public class LoadBulletPrefabs : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab0;
        [SerializeField] private GameObject bulletPrefab1;
        [SerializeField] private GameObject bulletPrefab2;
        [SerializeField] private GameObject bulletPrefab3;

        // Todo: Load prefabs from MyResources/Prefabs folder
        // In order to use bulletPrefabs in multiple files of bulletPatterns
        public GameObject GetBulletPrefabByIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return bulletPrefab0;
                case 1:
                    return bulletPrefab1;
                case 2:
                    return bulletPrefab2;
                case 3:
                    return bulletPrefab3;
                default:
                    Debug.Log("[Warning] Invalid bullet index, returning default bulletPrefab0");
                    return bulletPrefab0;
            }
        }
    }

}