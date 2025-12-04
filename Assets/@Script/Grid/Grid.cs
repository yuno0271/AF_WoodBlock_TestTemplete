using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private int columns = 0;
    [SerializeField]
    private int rows = 0;
    [SerializeField]
    float squaresGap = 0.1f;
    [SerializeField]
    private GameObject gridSquare;
    [SerializeField]
    private Vector2 startPosition = new Vector2 (0, 0);
    [SerializeField]
    private float squareScale = 0.5f;
    [SerializeField]
    private float everySquareOffset = 0.0f;

    private Vector2 _offset = new Vector2 (0, 0);
    private List<GameObject> _gridSquares = new List<GameObject> ();

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        //SetGridSquaresPositions();
    }

    private void SpawnGridSquares()
    {
        // 0, 1, 2, 3, 4
        // 5, 6, 7, 8, 9

        int squareIndex = 0;
        for(var row = 0;row < rows;row++)
        {
            for(var col = 0;col < columns;col++)
            {
                _gridSquares.Add(Instantiate(gridSquare) as GameObject);
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform);
                _gridSquares[_gridSquares.Count-1].transform.localScale = new Vector3(squareScale,squareScale,squareScale);
                _gridSquares[_gridSquares.Count-1].GetComponent<GridSquare>().SetImage(squareIndex % 2 == 0);
                squareIndex++;
            }
        }
    }

    private void SetGridSquaresPositions()
    {
        int columnNumber = 0;
        int rowNumber = 0;
        Vector2 squareGapNumber = new Vector2(0, 0);
        bool rowMoved = false;

        var squareRect = _gridSquares[0].GetComponent<RectTransform>();
        _offset.x = squareRect.rect.width * squareRect.transform.localScale.x + everySquareOffset;
        _offset.y = squareRect.rect.height * squareRect.transform.localScale.y + everySquareOffset;

        foreach(GameObject square in _gridSquares)
        {
            if(columnNumber + 1 > columns)
            {
                squareGapNumber.x = 0;
                // go to the next column
                columnNumber = 0;
                rowNumber++;
                rowMoved = true;
            }

            var pos_x_Offset = _offset.x * columnNumber + (squareGapNumber.x * squaresGap);
            var pos_y_Offset = _offset.y * rowNumber + (squareGapNumber.y * squaresGap);

            if(columnNumber > 0 && columnNumber % 3 == 0)
            {
                squareGapNumber.x++;
                pos_x_Offset += squaresGap;
            }

            if (rowNumber > 0 && rowNumber % 3 == 0 && rowMoved == false)
            {
                rowMoved = true;
                squareGapNumber.y++;
                pos_y_Offset += squaresGap;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + pos_x_Offset,
                startPosition.y - pos_y_Offset);

            square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + pos_x_Offset,
                startPosition.y - pos_y_Offset, 0.0f);

            columnNumber++; 
        }
    }
}
