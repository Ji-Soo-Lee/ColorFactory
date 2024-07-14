using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int x;
    public int y;
    private Grid grid;
    public void Init(int x, int y, Grid grid)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
    }

    public void SetPos(int x, int y)
    {
        this.x = x;
        this.y = y;
        transform.position = new Vector2(x, y);
    }

    public void DestroyCell()
    {
        Destroy(gameObject);
    }

}
