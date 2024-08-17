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
            {
                if (interpreter == null)
                {
                    interpreter = gameObject.AddComponent<TanmakInterpreter>();
                }
            }
        }

        void Start()
        {
            BulletPatternBase circularSpreading = gameObject.AddComponent<CircularSpreading>();

            BulletPatternBase rectangular = gameObject.AddComponent<Rectangular>();

            circularSpreading.SetParameters(new Dictionary<string, object> {
            {"bulletCount", 12 },
            { "radius", 6f }
        });

            rectangular.SetParameters(new Dictionary<string, object> {
            {"rowCount", 3 },
            {"colCount", 3 },
            {"spacing", 1.0f },
            {"directionWithAngle", 3 * Mathf.PI / 2 }
        });

            interpreter.schedules = new List<PatternSchedule>
        {
            new PatternSchedule(circularSpreading, 0f, 5f),
            new PatternSchedule(rectangular, 10f, 15f)
        };
        }
    }
}