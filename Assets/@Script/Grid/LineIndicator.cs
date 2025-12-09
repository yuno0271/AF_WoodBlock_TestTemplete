using UnityEngine;

public class LineIndicator
{
    private int row = 8;
    private int col = 8;

    public int[,] Line_data;
    public int[] columnIndexes;

    public LineIndicator(int _row, int _col)
    {
        row = _row;
        col = _col;
        Line_data = new int[row, col];
        columnIndexes = new int[col];

        int number = 0;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Line_data[i, j] = number;
                number++;
            }
        }

        int colNumber = 0;
        for(int i = 0; i < col; i++)
        {
            columnIndexes[i] = colNumber;
            colNumber++;
        }
    }

    private (int, int) GetSquarePosition(int square_index)
    {
        int pos_row = -1;
        int pos_col = -1;

        for(int _row = 0;_row < row;_row++)
        {
            for(int _col = 0; _col < col;_col++)
            {
                if (Line_data[_row, _col] == square_index)
                {
                    pos_row = _row;
                    pos_col = _col;
                }
            }
        }

        return (pos_row, pos_col);
    }

    public int[] GetVerticalLine(int square_index)
    {
        int[] line = new int[row];

        var square_position_col = GetSquarePosition(square_index).Item2;

        for(int index = 0;index < row;index++)
        {
            line[index] = Line_data[index, square_position_col];
        }

        return line;
    }
}
