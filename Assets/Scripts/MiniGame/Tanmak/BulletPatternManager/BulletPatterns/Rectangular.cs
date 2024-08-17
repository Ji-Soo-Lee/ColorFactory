using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanmak.BulletPatterns
{
    public class Rectangular : BulletPatternBase
    {
        private int rowCount = 3;
        private int colCount = 3;
        private float spacing = 1.0f;
        private Vector3 direction = new Vector3(1, 0, 0);

        public override IEnumerator FirePattern()
        {
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    Vector3 position = transform.position + new Vector3(col * spacing, row * spacing, 0);
                    StartCoroutine(FireBullet(position, direction, 0));
                }
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
                    case "rowCount":
                        rowCount = (int)parameters["rowCount"];
                        break;
                    case "colCount":
                        colCount = (int)parameters["colCount"];
                        break;
                    case "spacing":
                        spacing = (float)parameters["spacing"];
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