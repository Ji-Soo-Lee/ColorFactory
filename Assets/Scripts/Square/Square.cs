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
            if(IsAdjacent(selectedCell, cell)) //�� cell�� ������ ��
            {
                SwapCells(selectedCell, cell); //��ġ �ٲ�
                patternManager.CheckMatches(); //��Ī Ȯ��
                //�ٵ� �پ��� ���ϵ��� �ν��� �� �ְ� checkmatch �� �պ����ҵ�
                //������ 3�� ��Ī�ۿ� üũ ���� 
                // �� cell�� �������� �����¿� check �����ϰ� �������� ��?
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
