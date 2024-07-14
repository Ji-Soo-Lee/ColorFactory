using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{
    public Grid grid;
    
    public void CheckMatches()
    {
        List<Cell> cells_Destroy = new List<Cell>();

        for (int y = 0; y < grid.height; y++)
        {
            for(int x = 0; x < grid.width - 2; x++)
            {
                Cell cell1 = grid.GetCell(x, y);
                Cell cell2 = grid.GetCell(x + 1, y);
                Cell cell3 = grid.GetCell(x + 2, y);

                if(cell1 != null && cell2 != null && cell3 != null)
                {
                    if(cell1.tag == cell2.tag && cell2.tag == cell3.tag)
                    {
                        cells_Destroy.Add(cell1);
                        cells_Destroy.Add(cell2);
                        cells_Destroy.Add(cell3);
                    }
                }
            }
        }

        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height - 2 ; y++)
            {
                Cell cell1 = grid.GetCell(x, y);
                Cell cell2 = grid.GetCell(x, y + 1);
                Cell cell3 = grid.GetCell(x, y + 1);

                if (cell1 != null && cell2 != null && cell3 != null)
                {
                    if (cell1.tag == cell2.tag && cell2.tag == cell3.tag)
                    {
                        cells_Destroy.Add(cell1);
                        cells_Destroy.Add(cell2);
                        cells_Destroy.Add(cell3);
                    }
                }
            }
        }

        foreach(Cell cell in new HashSet<Cell>(cells_Destroy))
        {
            cell.DestroyCell();
        }

        ApplyGravity();
    }

    void ApplyGravity()
    {
        for(int x = 0; x < grid.width;x++)
        {
            for(int y=0; y < grid.height-1;y++) 
            {
                if(grid.GetCell(x,y) == null) //¸ðµç Ä­ Á¡°Ë ÈÄ ºóÄ­ÀÌ ÀÖÀ¸¸é
                {
                   for (int i = y + 1; i < grid.height; i++)
                    {
                        Cell above = grid.GetCell(x, i); 
                        if(above != null)
                        {
                            above.SetPos(x, y);
                            grid.SetCell(x, y, above);
                            grid.SetCell(x, i, null);
                            break;
                        }
                    }
                }
            }
        }
    }
}
