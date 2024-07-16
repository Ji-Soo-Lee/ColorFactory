using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanmakPlayer : MonoBehaviour
{
    public float speed = 5f;
    Vector3 pos;

    void Update()
    {
        pos = transform.position;

        if (pos.x > -10 && Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("a"))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        else if (pos.x < 10 && Input.GetKey(KeyCode.RightArrow) || Input.GetKey("d"))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        else if (pos.y < 10 && Input.GetKey(KeyCode.UpArrow) || Input.GetKey("w"))
        {
            transform.Translate(0, speed * Time.deltaTime, 0);
        }
        else if (pos.y > -10 && Input.GetKey(KeyCode.DownArrow) || Input.GetKey("s"))
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
        }
    }
}
