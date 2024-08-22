using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanmak
{
    // class to abstract tanmak pattern
    public abstract class BulletPatternBase : MonoBehaviour
    {
        // bullet prefab with random color
        protected GameObject bulletPrefab0;
        // bullet prefab with first color 
        protected GameObject bulletPrefab1;
        // bullet prefab with second color
        protected GameObject bulletPrefab2;
        // bullet prefab with third color
        protected GameObject bulletPrefab3;

        void Awake()
        {
            bulletPrefab0 = BulletPrefabLoader.Instance.BulletPrefab0;
            bulletPrefab1 = BulletPrefabLoader.Instance.BulletPrefab1;
            bulletPrefab2 = BulletPrefabLoader.Instance.BulletPrefab2;
            bulletPrefab3 = BulletPrefabLoader.Instance.BulletPrefab3;
        }

        public float fireRate = 1f; // bullet fire time rate (s)
        public float bulletSpeed = 5f; // bullet speed (m / s)

        protected Vector3 startPosition = new Vector3(0, 0, 0);

        protected IEnumerator FireBullet(Vector3 position, Vector3 direction, int bulletType = 0)
        {
            if (bulletPrefab0 == null || bulletPrefab1 == null || bulletPrefab2 == null || bulletPrefab3 == null)
            {
                Debug.LogWarning("[Tanmak] bulletPrefab is not assigned");
            }

            position += startPosition;

            GameObject bullet;
            switch (bulletType)
            {
                case 0:
                    bullet = Instantiate(bulletPrefab0, position, Quaternion.identity);
                    break;
                case 1:
                    bullet = Instantiate(bulletPrefab1, position, Quaternion.identity);
                    break;
                case 2:
                    bullet = Instantiate(bulletPrefab2, position, Quaternion.identity);
                    break;
                case 3:
                    bullet = Instantiate(bulletPrefab3, position, Quaternion.identity);
                    break;
                default:
                    bullet = Instantiate(bulletPrefab0, position, Quaternion.identity);
                    Debug.LogWarning("[Tanmak] Invalid Bullet Type, defaulting to bulletPrefab");
                    break;
            }

            bullet.GetComponent<Rigidbody2D>().velocity = bulletSpeed * direction;

            yield break;
        }

        public abstract IEnumerator FirePattern();
        public virtual void SetParameters(Dictionary<string, object> parameters) { }
    }
}