using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulb : MonoBehaviour
{
    SimonGameManager game;
    SpriteRenderer sprite;
    void Start()
    {
        this.game = SimonGameManager.game;
        this.sprite = GetComponent<SpriteRenderer>();
    }
    public IEnumerator blink(Color color)
    {
        StopAllCoroutines();
        this.sprite.color = color;
        yield return new WaitForSeconds(0.5f);
        this.sprite.color = Color.white;
    }
}
