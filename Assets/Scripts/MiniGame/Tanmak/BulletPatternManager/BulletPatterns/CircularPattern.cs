using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularPattern : BulletPatternBase
{
    public GameObject prefab0;
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;

    private void Awake()
    {
        bulletPrefab0 = prefab0;
        bulletPrefab1 = prefab1;
        bulletPrefab2 = prefab2;
        bulletPrefab3 = prefab3;
    }

    private int bulletCount = 10;
    private float radius = 5f;

    public override IEnumerator FirePattern()
    {
        for (float angle = 0; angle < 2 * Mathf.PI; angle += Mathf.PI / bulletCount)
        {
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0).normalized;
            Vector3 position = transform.position + direction * radius;
            StartCoroutine(FireBullet(position, direction, 0));
        }
        yield break;
    }

    public override void SetParameters(Dictionary<string, object> parameters)
    {
        foreach (var parameter in parameters.Keys)
        {
            switch(parameter)
            {
                case "fireRate":
                    fireRate = (float)parameters["fireRate"];
                    break;
                case "bulletSpeed":
                    bulletSpeed = (int)parameters["bulletSpeed"];
                    break;
                case "bulletCount":
                    bulletCount = (int)parameters["bulletCount"];
                    break;
                case "radius":
                    radius = (float)parameters["radius"];
                    break;
                default:
                    Debug.LogWarning("[Tanmak] Invalid parameter name");
                    Debug.LogWarning("Pattern Name: CircularPattern");
                    Debug.LogWarning("Parameter: " + parameter);
                    break;
            }
        }

        return;
    }
}
