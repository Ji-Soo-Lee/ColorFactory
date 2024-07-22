using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanmakConstruct : MonoBehaviour
{
    public GameObject bulletPrefab; // bullet prefab
    public float fireRate = 1f; // bullet fire time rate (s)
    public float bulletSpeed = 5f; // bullet speed (m/s)
    public int bulletCount = 10; // bullet count

    void Start()
    {
        StartCoroutine(FireBullets()); // It can be changed
    }

    IEnumerator FireBullets()
    {
        while (true)
        {
            for (int i = 0; i < bulletCount; ++i)
            {
                float angle = i * Mathf.PI * 2 / bulletCount;
                Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            }

            yield return new WaitForSeconds(fireRate);
        }
    }
}
