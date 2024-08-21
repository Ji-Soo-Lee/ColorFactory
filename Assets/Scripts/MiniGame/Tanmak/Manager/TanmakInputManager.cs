using System.Diagnostics;
using UnityEngine;

namespace Tanmak
{
    public class TanmakInputManager : MonoBehaviour
    {
        public GameObject Player;
        public GameObject Joystick;
        [HideInInspector]
        public bool isMovingKeyDown;

        TanmakPlayer player;
        Vector3 dir;

        void Start()
        {
            player = Player.GetComponent<TanmakPlayer>();
        }

        void Update()
        {
            dir = new Vector3(0.0f, 0.0f, 0.0f);
            isMovingKeyDown = false;

            // get joystick input
            if (Joystick.GetComponent<TanmakJoystick>() != null &&
                Joystick.GetComponent<TanmakJoystick>().isJoystickMoving)
            {
                dir = Joystick.GetComponent<TanmakJoystick>().dir;
                isMovingKeyDown = true;
            }

            // get moving key
            if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
            {
                dir.x -= 1.0f;
                isMovingKeyDown = true;
            }
            if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
            {
                dir.x += 1.0f;
                isMovingKeyDown = true;
            }
            if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))
            {
                dir.y += 1.0f;
                isMovingKeyDown = true;
            }
            if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow))
            {
                dir.y -= 1.0f;
                isMovingKeyDown = true;
            }

            // control player
            if (player != null)
            {
                if (isMovingKeyDown)
                {
                    player.SetDirection(dir);
                }
                else if (player.isMoving)
                {
                    player.StopMoving();
                }


                // change color if enter pressed
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    player.ChangeColor();
                }
            }
        }
    }
}