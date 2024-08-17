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
            BulletPatternBase circularPattern = gameObject.AddComponent<CircularPattern>();

            circularPattern.SetParameters(new Dictionary<string, object> {
            {"bulletCount", 12 },
            { "radius", 6f }
        });

            interpreter.schedules = new List<PatternSchedule>
        {
            new PatternSchedule(circularPattern, 0f, 5f),
        };
        }
    }
}