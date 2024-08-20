using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanmak.BulletPatterns
{
    public class Circle : BulletPatternBase
    {
        private int bulletCount = 10;
        private float radius = 5f;
        private Vector3 direction = new Vector3(1, 0, 0);

        public override IEnumerator FirePattern()
        {
            for (int i = 0; i < bulletCount; ++i)
            {
                float angle = i * Mathf.PI * 2 / bulletCount;
                Vector3 bulletSpawnPosition = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
                StartCoroutine(FireBullet(bulletSpawnPosition, direction, 0)); 
            }

            yield break;
        }

        public override void SetParameters(Dictionary<string, object> parameters)
        {
            foreach (var parameter in parameters.Keys)
            {
                switch (parameter)
                {
                    // defualt values are set in the TanmakPatterns.cs
                    case "fireRate":
                        fireRate = (float)parameters["fireRate"];
                        break;
                    case "bulletSpeed":
                        bulletSpeed = (int)parameters["bulletSpeed"];
                        break;
                    case "direction":
                        direction = (Vector3)parameters["direction"];
                        break;
                    case "directionWithAngle":
                        float angle = (float)parameters["directionWithAngle"];
                        direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0).normalized;
                        break;

                    // special values for rectangular pattern
                    case "bulletCount":
                        bulletCount = (int)parameters["bulletCount"];
                        break;
                    case "radius":
                        radius = (float)parameters["radius"];
                        break;

                    default:
                        Debug.LogWarning("[Tanmak] Invalid parameter name");
                        Debug.LogWarning("Pattern Name: Rectangular");
                        Debug.LogWarning("Parameter: " + parameter);
                        break;
                }
            }
        }
    }
}
