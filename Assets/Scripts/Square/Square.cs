using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square
{
    public Grid grid;
    public PatternManager patternManager;

    private Cell selectedCell;

    public void OnCellClicked(Cell cell)
    {
        if(selectedCell == null)
        {
            selectedCell = cell;
        }
        else
        {
            if(IsAdjacent(selectedCell, cell)) //두 cell이 인접할 때
            {
                SwapCells(selectedCell, cell); //위치 바꿔
                patternManager.CheckMatches(); //매칭 확인
                //근데 다양한 패턴들을 인식할 수 있게 checkmatch 더 손봐야할듯
                //지금은 3개 매칭밖에 체크 못함 
                // 한 cell을 기준으로 상하좌우 check 가능하게 만들어야할 듯?
            }
            selectedCell = null;
        }
    }

    public void SwapCells(Cell cell1, Cell cell2)
    {
        int tempX = cell1.x;
        int tempY = cell1.y;

        cell1.SetPos(cell2.x, cell2.y);
        cell2.SetPos(tempX, tempY);

        grid.SetCell(cell1.x, cell1.y, cell1);
        grid.SetCell(cell2.x, cell2.y, cell2);
    }

    bool IsAdjacent(Cell cell1,Cell cell2)
    {
        return (Mathf.Abs(cell1.x - cell2.x) +  Mathf.Abs(cell1.y - cell2.y)) == 1;
    }
}
