using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tanmak.BulletPatterns;

namespace Tanmak
{
    public class BulletPatternManager : MonoBehaviour
    {
        public TanmakInterpreter interpreter;

        private void Awake()
        {
            if (interpreter == null)
            {
                interpreter = gameObject.AddComponent<TanmakInterpreter>();
            }

            if (interpreter.schedules == null)
            {
                interpreter.schedules = new List<PatternSchedule>();
            }
        }

        void Start()
        {
            // tmdghks: implemented just randomly generated patterns
            // this will be replaced with json loader (maybe)
            for (int i = 0; i < 10000; i++)
            {
                BulletPatternBase pattern = null;
                float radius = Random.Range(1f, 5f);
                int bulletCount = Random.Range(5, 10);
                int rowCount = Random.Range(1, 4);
                int colCount = Random.Range(1, 4);
                float spacing = Random.Range(0.2f, 0.5f);

                float posX = Random.Range(-5f, 5f);
                float posY = Random.Range(-5f, 5f);
                Vector3 position = new Vector3(posX, posY, 0);

                float dirX = Random.Range(-1f, 1f);
                float dirY = Random.Range(-1f, 1f);
                Vector3 direction = new Vector3(dirX, dirY, 0).normalized;

                int bulletSpeed = (int) Random.Range(1f, 5f);

                int patternType = Random.Range(0, 3);
                switch (patternType)
                {
                    case 0:
                        pattern = gameObject.AddComponent<CircularSpreading>();
                        pattern.SetParameters(new Dictionary<string, object> {
                            {"bulletCount", bulletCount },
                            { "radius", radius },
                        });
                        break;
                    case 1:
                        pattern = gameObject.AddComponent<Rectangular>();
                        pattern.SetParameters(new Dictionary<string, object> {
                            {"rowCount", rowCount },
                            {"colCount", colCount },
                            {"spacing", spacing },
                        });
                        break;
                    case 2:
                        pattern = gameObject.AddComponent<Circle>();
                        pattern.SetParameters(new Dictionary<string, object> {
                            {"bulletCount", bulletCount },
                            { "radius", radius },
                        });
                        break;
                }
                pattern.SetParameters(new Dictionary<string, object> {
                    { "startPosition", position },
                    { "direction", direction },
                    { "bulletSpeed", bulletSpeed },
                });
                
                interpreter.schedules.Add(new PatternSchedule(pattern, i / 2f, 5));
            }

            interpreter.ExecutePatternSchedules();

            /** Example usage of the BulletPatternManager
            BulletPatternBase circularSpreading = gameObject.AddComponent<CircularSpreading>();

            BulletPatternBase rectangular = gameObject.
            AddComponent<Rectangular>();

            BulletPatternBase circle = gameObject.AddComponent<Circle>();

            circularSpreading.SetParameters(new Dictionary<string, object> {
                {"bulletCount", 12 },
                { "radius", 3f },
            });

            rectangular.SetParameters(new Dictionary<string, object> {
                {"rowCount", 3 },
                {"colCount", 3 },
                {"spacing", 1.0f },
                {"directionWithAngle", 3 * Mathf.PI / 2 },
            });

            circle.SetParameters(new Dictionary<string, object> {
                {"bulletCount", 12 },
                { "radius", 1f },
            });

            interpreter.schedules = new List<PatternSchedule> {
                new PatternSchedule(circularSpreading, 0f, 5f),
                new PatternSchedule(circle, 3f, 8f),
                new PatternSchedule(rectangular, 10f, 15f),
            };

            interpreter.ExecutePatternSchedules();
            */
        }
    }
}