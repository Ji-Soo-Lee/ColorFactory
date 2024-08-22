using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanmak
{
    public class BulletPrefabLoader : MonoBehaviour
    {
        private static BulletPrefabLoader _instance;
        public static BulletPrefabLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject loader = new GameObject("BulletPrefabLoader");
                    _instance = loader.AddComponent<BulletPrefabLoader>();
                    DontDestroyOnLoad(loader);
                }
                return _instance;
            }
        }

        public GameObject BulletPrefab0 { get; private set; }
        public GameObject BulletPrefab1 { get; private set; }
        public GameObject BulletPrefab2 { get; private set; }
        public GameObject BulletPrefab3 { get; private set; }

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);

                BulletPrefab0 = Resources.Load<GameObject>("Prefabs/BulletPrefab0");
                BulletPrefab1 = Resources.Load<GameObject>("Prefabs/BulletPrefab1");
                BulletPrefab2 = Resources.Load<GameObject>("Prefabs/BulletPrefab2");
                BulletPrefab3 = Resources.Load<GameObject>("Prefabs/BulletPrefab3");
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}