using System;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Tanmak
{
    public class TanmakPlayer : MonoBehaviour
    {
        public float speed = 5f;
        [HideInInspector] public bool isMoving;
        [SerializeField] private TanmakGameManager tanmakGM;

        SpriteRenderer spriteRenderer;
        public GameObject ShieldObject;

        public int curColorIdx { get; private set; } = 0;
        Vector3 pos;
        Vector3 direction;

        bool isInvincible;
        float invincibleTime = 3.0f;
        bool isShielded = false;

        #if UNITY_IOS && !UNITY_EDITOR
            [DllImport("__Internal")]
            private static extern void Vibrate(long _n);
        # endif

        
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

            curColorIdx = GetNextColorIdx();
            spriteRenderer.color = tanmakGM.colors[curColorIdx];

            SetChangeButtonNextColor();
        }

        public void SetInvincible(bool invincible)
        {
            isInvincible = invincible;
        }

        public void SetShield(bool isActive)
        {
            isShielded = isActive;
            ShieldObject.SetActive(isActive);
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

        private int GetNextColorIdx()
        {
            return (curColorIdx + 1) % TanmakGameManager.colorSize;
        }

        public void SetChangeButtonNextColor()
        {
            Color nextColor = tanmakGM.colors[GetNextColorIdx()];
            tanmakGM.TUIManager.SetColorChangeImageColor(nextColor);
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
                        // Vibration
                        # if UNITY_ANDROID && !UNITY_EDITOR
                            Vibration.Vibrate(30);
                        # elif UNITY_IOS && !UNITY_EDITOR
                            Vibrate(1519);
                        # endif
                        tanmakGM.ModifyScore(5);
                    }
                    else
                    {
                        // tanmakGM.ModifyScore(-10);
                        if (isShielded)
                        {
                            SetShield(false);
                        }
                        else
                        {
                            // Vibration
                            # if UNITY_ANDROID && !UNITY_EDITOR
                                Vibration.Vibrate(60);
                            # elif UNITY_IOS && !UNITY_EDITOR
                                Vibrate(1521);
                            # endif
                            tanmakGM.EndGameWithTime();
                        }
                    }
                }
            }
            else if (collider.tag == "TanmakItem")
            {
                TanmakItem item = collider.gameObject.GetComponent<TanmakItem>();
                if (item != null)
                {
                    # if UNITY_ANDROID && !UNITY_EDITOR
                        Vibration.Vibrate(30);
                    # elif UNITY_IOS && !UNITY_EDITOR
                        Vibrate(1519);
                    # endif
                    if (item.itemName == "TanmakBarrier")
                    {
                        SetShield(true);
                    }

                    Destroy(item.gameObject);
                }
            }
        }

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            spriteRenderer.color = tanmakGM.colors[curColorIdx];

            StopMoving();

            SetChangeButtonNextColor();
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

            tanmakGM.invincibleTimer.SetupTimer(invincibleTime, timerAction);
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