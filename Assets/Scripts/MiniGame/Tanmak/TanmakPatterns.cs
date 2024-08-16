using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanmakPatterns : MonoBehaviour
{
    public GameObject bulletPrefab; // bullet prefab
    public GameObject bulletPrefab1; // First Color Prefab
    public GameObject bulletPrefab2; // Second Color Prefab
    public GameObject bulletPrefab3; // Third Color Prefab
    public float fireRate = 1f; // bullet fire time rate (s)
    public float bulletSpeed = 5f; // bullet speed (m/s)
    public int bulletCount = 10; // bullet count

    public int bulletCountRow = 3;
    public int bulletCountCol = 3;
    public float bulletSpacing = 1.0f;

    void Start()
    {
        Debug.Log("Debug log in TanmakPatterns");
        // Log the assigned prefab names to confirm they are set
        Debug.Log("bulletPrefab : " + bulletPrefab);
        Debug.Log("bulletPrefab1: " + bulletPrefab1);
        Debug.Log("bulletPrefab2: " + bulletPrefab2);
        Debug.Log("bulletPrefab3: " + bulletPrefab3);
        Debug.Log("============");

        // StartCoroutine(FireBulletsCircularPattern()); // It can be changed
        // StartCoroutine(FireBulletsRectangularPattern(bulletCountRow, bulletCountCol, bulletSpacing, 90));
        //StartCoroutine(FireBulletsCirclePattern(20, 3, 0));
        StartCoroutine(FireColorTest());
    }

    IEnumerator FireColorTest()
    {
        Vector3 direction = new Vector3(1, 0, 0);
        GameObject bullet1 = Instantiate(bulletPrefab1, transform.position, Quaternion.identity);
        bullet1.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        yield return new WaitForSeconds(1f);

        GameObject bullet2 = Instantiate(bulletPrefab2, transform.position, Quaternion.identity);
        bullet2.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        yield return new WaitForSeconds(1f);

        GameObject bullet3 = Instantiate(bulletPrefab3, transform.position, Quaternion.identity);
        bullet3.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        yield return new WaitForSeconds(1f);
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

    // Fire multiple bullets in a shape of circle
    // bulletCount: ?? ???? ??? ??
    // radius: ???
    // angle: ?? ??
    IEnumerator FireBulletsCirclePattern(int bulletCount, int radius, float angle)
    {
        while (true)
        {
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            float circleAngle = 0, circleAngleIter = Mathf.PI * 2 / bulletCount;
            for (; circleAngle < Mathf.PI + Mathf.PI; circleAngle += circleAngleIter)
            {
                Vector3 positionOffset = new Vector3(radius * Mathf.Cos(circleAngle), radius * Mathf.Sin(circleAngle), 0);
                GameObject bullet = Instantiate(bulletPrefab1, transform.position + positionOffset, Quaternion.identity);
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
    IEnumerator FireBulletsRectangularPattern(int row, int col, float spacing, float angle)
    {
        while (true)
        {
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < col; ++j)
                {
                    Vector3 positionOffset = new Vector3(j * bulletSpacing, i * spacing, 0);
                    Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.PI / 180f), Mathf.Sin(angle * Mathf.PI / 180f), 0);
                    GameObject bullet = Instantiate(bulletPrefab, transform.position + positionOffset, Quaternion.identity);
                    bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
                }
            }

            yield return new WaitForSeconds(fireRate);
        }
    }


    IEnumerator FireBulletsRectangularSpreadingPattern(int row, int col, float spacing)
    {
        while (true)
        {

        }
    }

    /*
    IEnumerator FireBulletsTriangularSpreadingPattern()
    {
        while (true)
        {
            // Todo: ???? ??? ??? ?
        }
    }
    */
}
