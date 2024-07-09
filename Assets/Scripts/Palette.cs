using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palette : MonoBehaviour
{
    GameManager game;
    int color;//이 팔레트가 담당하는 색
    void Start()
    {
        this.game = GameManager.game;
    }
    public void assign_color(int x,Material m)
    {
        this.color = x;
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material = m;
    }
    void OnMouseDown()
    {
        game.selected = this.color;
    }
}
