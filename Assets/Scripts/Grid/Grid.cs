using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

public class Grid : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject[] cellPrefabs;

    private Cell[,] cells;
    void Start()
    {
        cells = new Cell[width, height];
        GenerateGrid();
    }
    void GenerateGrid()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y =0; y < height; y++)
            {
                Vector2 position = new Vector2(x, y);
                int randomIndex = UnityEngine.Random.Range(0, cellPrefabs.Length);
                GameObject cellObject = Instantiate(cellPrefabs[randomIndex], position, Quaternion.identity);
                cellObject.transform.parent = this.transform;

                Cell cell = cellObject.GetComponent<Cell>();
                cell.Init(x, y, this); 
                cells[x, y] = cell;
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return cells[x, y];
        }
        return null;
    }
    public void SetCell(int x, int y, Cell cell)
    {
        if(x >= 0 && x < width && y >= 0 && y < height)
        {
            cells[x, y] = cell;
        }
    }
}