using UnityEngine;

public class TanmakPlayer : MonoBehaviour
{
    public float speed = 5f;
    [HideInInspector]
    public bool isMoving;
    Vector3 pos;
    Vector3 direction;

    // set direction of character to move
    public void SetDirection(Vector3 dir)
    {
        if (direction == null) return;
        if (dir.z != 0.0f) dir.z = 0.0f;
        direction = dir;
        isMoving = true;
    }

    // stops moving
    public void Stop()
    {
        direction = new Vector3(0.0f, 0.0f, 0.0f);
        isMoving = false;
    }

    void Start()
    {
        Stop();
    }

    void Update()
    {
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

        if (direction.magnitude > 1.0f)
        {
            direction = direction.normalized;
        }
        // move
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
