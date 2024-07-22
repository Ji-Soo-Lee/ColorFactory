using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palette : MonoBehaviour
{
    GameManager game;
    void Start()
    {
        this.game = GameManager.game;
    }
    public void ChangeColor(int x)
    {
        game.selected = x;
    }
}
