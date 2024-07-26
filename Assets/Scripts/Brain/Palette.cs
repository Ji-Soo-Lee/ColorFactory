using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palette : MonoBehaviour
{
    BrainGameManager game;
    void Start()
    {
        this.game = BrainGameManager.game;
    }
    public void ChangeColor(int x)
    {
        game.selected = x;
    }
}
