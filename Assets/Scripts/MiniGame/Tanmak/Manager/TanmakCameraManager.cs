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
        Vector3 velocity = Vector3.zero; // SmoothDamp 사용 시 필요

        float speed;
        Vector3 targetPos;

        void LateUpdate()
        {
            targetPos = new Vector3(
                Target.transform.position.x + offsetX,
                Target.transform.position.y + offsetY,
                offsetZ
                );

            if (Vector3.Distance(targetPos, transform.position) >= 2.0f)
            {
                speed = Target.GetComponent<TanmakPlayer>().speed / 2.0f;
                
                // Vector3 nextPos = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
                Vector3 nextPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 0.3f);

                nextPos.x = Mathf.Clamp(nextPos.x, -10f, 10f);
                nextPos.y = Mathf.Clamp(nextPos.y, -8f, 8f);

                transform.position = nextPos;
            }
        }
    }
}