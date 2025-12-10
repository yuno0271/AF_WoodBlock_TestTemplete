using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private ShapeStorage shapeStorage;
    [SerializeField]
    private int columns = 8;
    [SerializeField]
    private int rows = 8;
    //[SerializeField]
    //float squaresGap = 0.1f;
    [SerializeField]
    private GameObject gridSquare;
    //[SerializeField]
    //private Vector2 startPosition = new Vector2 (0, 0);
    [SerializeField]
    private float squareScale = 0.5f;
    //[SerializeField]
    //private float everySquareOffset = 0.0f;

    private Vector2 _offset = new Vector2(0, 0);
    private List<GameObject> _gridSquares = new List<GameObject>();
    private LineIndicator _lineIndicator;
    private bool isCombo = false;
    private int _combo = 0;
    private int _lineClearCount = 0;
    private bool isGameOver = false;

    private void Start()
    {
        _lineIndicator = new LineIndicator(rows, columns);
        CreateGrid();
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
    }

    private void SpawnGridSquares()
    {
        // 0, 1, 2, 3, 4
        // 5, 6, 7, 8, 9

        int squareIndex = 0;
        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < columns; col++)
            {
                _gridSquares.Add(Instantiate(gridSquare) as GameObject);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SquareIndex = squareIndex;
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform);
                _gridSquares[_gridSquares.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                //_gridSquares[_gridSquares.Count-1].GetComponent<GridSquare>().SetImage(squareIndex % 2 == 0);
                squareIndex++;
            }
        }
    }

    private void OnEnable()
    {
        GameEvents.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
    }

    private void OnDisable()
    {
        GameEvents.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
    }

    private void CheckIfShapeCanBePlaced()
    {
        var squareIndexes = new List<int>();

        foreach(var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();

            if(gridSquare.Selected && !gridSquare.SquareOccupied)
            {
                squareIndexes.Add(gridSquare.SquareIndex);
                gridSquare.Selected = false;
                //gridSquare.ActivateSquare();
            }
        }

        var currentSelectedShape = shapeStorage.GetCurrentSelectedShape();
        if (currentSelectedShape == null) return; // there is no selected shape

        if(currentSelectedShape.TotalSquareNumber == squareIndexes.Count)
        {
            foreach(var squareIndex in squareIndexes)
            {
                _gridSquares[squareIndex].GetComponent<GridSquare>().PlaceShapeOnBoard();
            }

            var shapeLeft = 0;

            foreach(var shape in shapeStorage.shapeList)
            {
                if(shape.IsOnStartPosition() && shape.IsAnyOfShapeSquareActive())
                {
                    shapeLeft++;
                }
            }

            if (shapeLeft == 0)
            {
                GameEvents.RequestNewShapes();
            }
            else
            {
                GameEvents.SetShapeInActive();
            }

            //CheckIfAnyLineIsCompleted();

            AudioManager.Instance.PlayPlaceBlockClip();
            var score = CalculateLineCombo() + currentSelectedShape.TotalSquareNumber + _lineClearCount;
            GameEvents.AddScores(score);
            CheckIfPlayerLost();
        }
        else
        {
            GameEvents.MoveShapeToStartPosition();
        }
    }

    private int CheckIfAnyLineIsCompleted()
    {
        List<int[]> lines = new List<int[]>();

        // columns
        foreach(var column in _lineIndicator.columnIndexes)
        {
            lines.Add(_lineIndicator.GetVerticalLine(column));
        }

        // rows
        for(var row = 0;row < rows;row++)
        {
            List<int> data = new List<int>(rows);
            for(var index = 0;index < rows;index++)
            {
                data.Add(_lineIndicator.Line_data[row, index]);
            }

            lines.Add(data.ToArray());
        }

        var completedLines = CheckIfSquareAreCompleted(lines);

        if (completedLines >= 2)
        {
            //TODO: Play Bonus animation.
        }

        return completedLines;
    }

    private int CalculateLineCombo()
    {
        var completedLine = CheckIfAnyLineIsCompleted();

        if (completedLine >=1 )
        {
            _combo++;
            AudioManager.Instance.PlayLineClearClip();
            if (isCombo)
            {
                AudioManager.Instance.PlayComboClip(_combo - 1);
            }
            else
            {
                isCombo = true;
            }
        }
        else
        {
            isCombo = false;
            _combo = 0;
        }

        var totalScores = _combo * completedLine;

        return totalScores;
    }

    private int CheckIfSquareAreCompleted(List<int[]> data)
    {
        _lineClearCount = 0;
        List<int[]> completedLines = new List<int[]>();

        var linesCompleted = 0;

        foreach(var line in data)
        {
            var lineCompleted = true;
            foreach(var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                if (comp.SquareOccupied == false)
                {
                    lineCompleted = false;
                }
            }

            if(lineCompleted)
            {
                completedLines.Add(line);
            }
        }

        foreach(var line in completedLines)
        {
            var completed = false;

            foreach(var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.DeactivateSquare();
                completed = true;
                _lineClearCount++;
            }

            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.ClearOccupied();
            }

            if(completed)
            {
                linesCompleted++;
            }
        }

        return linesCompleted;
    }

    private void CheckIfPlayerLost()
    {
        var validShapes = 0;

        for(var index = 0;index < shapeStorage.shapeList.Count;index++)
        {
            var isShapeActive = shapeStorage.shapeList[index].IsAnyOfShapeSquareActive();
            if (CheckIfShapeCanBePlacedOnGrid(shapeStorage.shapeList[index]) && isShapeActive)
            {
                shapeStorage.shapeList[index]?.ActivateShape();
                validShapes++;
            }
        }

        if(validShapes <= 0)
        {
            //GAME OVER
            isGameOver = true;
            GameEvents.GameOver(false);
            Debug.Log("GAME OVER");
        }
    }

    private bool CheckIfShapeCanBePlacedOnGrid(Shape currentShape)
    {
        var currentShapeData = currentShape.CurrentShapeData;
        var shapeColumns = currentShapeData.Columns;
        var shapeRows = currentShapeData.Rows;

        // All indexes of filled up squares.
        List<int> originalShapeFilledUpSquares = new List<int>();
        var squareIndex = 0;

        for(var rowIndex = 0;rowIndex < shapeRows;rowIndex++)
        {
            for(var columnIndex = 0;columnIndex < shapeColumns;columnIndex++)
            {
                if (currentShapeData.Board[rowIndex].Column[columnIndex])
                {
                    originalShapeFilledUpSquares.Add(squareIndex);
                }

                squareIndex++;
            }
        }

        if (currentShape.TotalSquareNumber != originalShapeFilledUpSquares.Count)
            Debug.LogError("Number of filled up squares are not the same as the original shape have");

        var squareList = GetAllSquareCombination(shapeColumns, shapeRows);

        bool canBePlaced = false;

        foreach(var number in squareList)
        {
            bool shapeCanBePlacedOnTheBoard = true;
            foreach(var squareIndexToCheck in originalShapeFilledUpSquares)
            {
                var comp = _gridSquares[number[squareIndexToCheck]].GetComponent<GridSquare>();
                if(comp.SquareOccupied)
                {
                    shapeCanBePlacedOnTheBoard = false;
                }
            }

            if(shapeCanBePlacedOnTheBoard)
            {
                canBePlaced = true;
            }
        }

        return canBePlaced;
    }

    private List<int[]> GetAllSquareCombination(int columns, int rows)
    {
        var squareList = new List<int[]>();
        var lastColumnIndex = 0;
        var lastRowIndex = 0;

        int safeIndex = 0;

        while(lastRowIndex + (rows - 1) < this.rows)
        {
            var rowData = new List<int>();
            for(var row = lastRowIndex;row < lastRowIndex + rows;row++)
            {
                for(var column = lastColumnIndex;column < lastColumnIndex + columns;column++)
                {
                    rowData.Add(_lineIndicator.Line_data[row, column]);
                }
            }

            squareList.Add(rowData.ToArray());

            lastColumnIndex++;

            if(lastColumnIndex + (columns - 1) >= this.columns)
            {
                lastRowIndex++;
                lastColumnIndex = 0;
            }

            safeIndex++;
            if(safeIndex > 100)
            {
                break;
            }
        }

        return squareList;
    }
}
