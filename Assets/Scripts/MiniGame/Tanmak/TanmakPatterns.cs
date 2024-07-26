using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanmakConstruct : MonoBehaviour
{
    public GameObject bulletPrefab; // bullet prefab
    public float fireRate = 1f; // bullet fire time rate (s)
    public float bulletSpeed = 5f; // bullet speed (m/s)
    public int bulletCount = 10; // bullet count

    public int bulletCountRow = 3;
    public int bulletCountCol = 3;
    public float bulletSpacing = 1.0f;

    void Start()
    {
        // StartCoroutine(FireBulletsCircularPattern()); // It can be changed

        StartCoroutine(FireBulletsRectangularPattern(bulletCountRow, bulletCountCol, bulletSpacing));
    }

    // Fire multiple bullets in a circular pattern
    IEnumerator FireBulletsCircularPattern() 
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


    // Fire multiple bullets in a rectengular pattern
    // Fire start from transform.position
    // row: the number of row
    // col: the number of column
    // spacing: space between adjacent bullets
    IEnumerator FireBulletsRectangularPattern(int row, int col, float spacing)
    {
        while (true)
        {
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < col; ++j)
                {
                    Vector3 positionOffset = new Vector3(j * bulletSpacing, i * spacing, 0);
                    GameObject bullet = Instantiate(bulletPrefab, transform.position + positionOffset, Quaternion.identity);
                    bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(0, bulletSpeed, 0); // 위쪽으로 발사
                }
            }

            yield return new WaitForSeconds(fireRate);
        }
    }
}
