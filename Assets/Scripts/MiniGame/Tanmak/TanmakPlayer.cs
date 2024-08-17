using System;
using UnityEngine;

namespace Tanmak
{
    public class TanmakPlayer : MonoBehaviour
    {
        public float speed = 5f;
        [HideInInspector] public bool isMoving;
        [SerializeField] private TanmakGameManager tanmakGM;

        SpriteRenderer spriteRenderer;

        int curColorIdx;
        Vector3 pos;
        Vector3 direction;

        bool isInvincible;

        // set direction of character to move
        public void SetDirection(Vector3 dir)
        {
            if (tanmakGM.CheckPause()) return;

            if (direction == null) return;
            if (dir.z != 0.0f) dir.z = 0.0f;
            direction = dir;
            isMoving = true;
        }

        // stops moving
        public void StopMoving()
        {
            if (tanmakGM.CheckPause()) return;

            direction = new Vector3(0.0f, 0.0f, 0.0f);
            isMoving = false;
        }

        public void ChangeColor()
        {
            if (tanmakGM.CheckPause()) return;

            curColorIdx = (curColorIdx + 1) % TanmakGameManager.colorSize;
            spriteRenderer.color = tanmakGM.colors[curColorIdx];
        }

        public void SetInvincible(bool invincible)
        {
            isInvincible = invincible;
        }

        public void ToggleRenderer()
        {
            Color curColor = tanmakGM.colors[curColorIdx];
            if (spriteRenderer.color.a <= 0.01f)
            {
                curColor.a = 1.0f;
            }
            else
            {
                curColor.a = 0.0f;
            }
            spriteRenderer.color = curColor;
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (tanmakGM.CheckPause()) return;

            if (!isInvincible && collider.tag == "Tanmak")
            {

                TempTanmak tanmak = collider.gameObject.GetComponent<TempTanmak>();
                if (tanmak != null)
                {
                    if (ColorUtils.CompareColor(tanmak.color, tanmakGM.colors[curColorIdx]))
                    {
                        tanmakGM.ModifyScore(5);
                    }
                    else
                    {
                        // tanmakGM.ModifyScore(-10);
                        tanmakGM.EndGame();
                    }
                }
            }
        }

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            curColorIdx = 0;
            spriteRenderer.color = tanmakGM.colors[curColorIdx];

            StopMoving();
        }

        void OnEnable()
        {
            Action timerAction = () => SetInvincible(false);
            timerAction += () =>
            {
                // Ensure Visibility
                spriteRenderer.color = tanmakGM.colors[curColorIdx];
            };

            SetInvincible(true);

            tanmakGM.invincibleTimer.SetupTimer(5.0f, timerAction);
            tanmakGM.invincibleTimer.SetupTimerTik(0.2f, ToggleRenderer);// Blink
            tanmakGM.invincibleTimer.StartTimer();
        }

        void Update()
        {
            if (tanmakGM.CheckPause()) return;

            pos = transform.position;

            // stop for boundary
            if ((pos.x <= -10 && direction.x < 0.0f) || (pos.x >= 10 && direction.x > 0.0f))
            {
                direction.x = 0.0f;
            }
            if ((pos.y <= -10 && direction.y < 0.0f) || (pos.y >= 10 && direction.y > 0.0f))
            {
                direction.y = 0.0f;
            }

            // normalize
            if (direction.magnitude > 1.0f)
            {
                direction = direction.normalized;
            }
            // move
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}