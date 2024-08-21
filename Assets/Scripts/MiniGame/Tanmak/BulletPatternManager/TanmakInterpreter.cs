using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Tanmak
{
    public class TanmakInterpreter : MonoBehaviour
    {
        public List<PatternSchedule> schedules;

        private float elapsedTime = 0f;

        public void ExecutePatternSchedules()
        {
            schedules = schedules.OrderBy(schedule => schedule.startTime).ToList();

            foreach (var schedule in schedules)
            {
                StartCoroutine(ExecutePatterns(schedule));
            }
        }

        IEnumerator ExecutePatterns(PatternSchedule schedule)
        {
            // waiting by next schedule
            yield return new WaitForSeconds(schedule.startTime - elapsedTime);

            StartCoroutine(schedule.pattern.FirePattern());

            elapsedTime = schedule.startTime + schedule.duration;
        }
    }
}