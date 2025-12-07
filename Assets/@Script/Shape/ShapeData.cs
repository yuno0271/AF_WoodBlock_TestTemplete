using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "ShapeData", fileName = "New ShapeData")]
public class ShapeData : ScriptableObject
{
    #region Row
    [System.Serializable]
    public class Row
    {
        [SerializeField]
        private bool[] column;
        private int _size;

        public bool[] Column
        {
            set => column = value;
            get => column;
        }
        public Row()
        {
        }

        public Row(int size)
        {
            _size = size;
            column = new bool[size];
            ClearRow();
        }
        public void CreateRow()
        {
            ClearRow();
        }
        public void ClearRow()
        {
            for(int i = 0; i < _size;i++)
            {
                column[i] = false;
            }
        }
    }
    #endregion
    [SerializeField]
    private int rows = 0;
    [SerializeField]
    private int columns = 0;
    [SerializeField]
    private Row[] board;

    public int Columns
    {
        set => columns = value;
        get => columns;
    }
    public int Rows
    {
        set => rows = value;
        get => rows;
    }

    public Row[] Board => board;

    public void Clear()
    {
        for(int i = 0;i < rows;i++)
        {
            board[i].ClearRow();
        }
    }

    public void CreateNewBoard()
    {
        board = new Row[rows];

        for(var i = 0; i < rows;i++)
        {
            board[i] = new Row(columns);
        }
    }
}
