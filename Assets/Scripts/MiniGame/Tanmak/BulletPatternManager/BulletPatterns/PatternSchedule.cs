using System;
using UnityEngine;

namespace Tanmak
{

    public class PatternSchedule
    {
        public BulletPatternBase pattern;
        public float startTime;
        public float duration;

        public PatternSchedule(BulletPatternBase pattern, float startTime, float duration)
        {
            this.pattern = pattern;
            this.startTime = startTime;
            this.duration = duration;
        }
    }

}