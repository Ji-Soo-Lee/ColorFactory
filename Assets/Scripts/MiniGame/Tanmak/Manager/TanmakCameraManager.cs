using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tanmak
{
    public class TanmakCameraManager : MonoBehaviour
    {
        public GameObject Target;

        float offsetX = 0.0f;
        float offsetY = 0.0f;
        float offsetZ = -10.0f;

        float speed;
        Vector3 targetPos;

        void Update()
        {
            targetPos = new Vector3(
                Target.transform.position.x + offsetX,
                Target.transform.position.y + offsetY,
                offsetZ
                );

            if (Vector3.Distance(targetPos, transform.position) >= 2.0f)
            {
                speed = Target.GetComponent<TanmakPlayer>().speed / 2.0f;

                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
            }
        }
    }
}