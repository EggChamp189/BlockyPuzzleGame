using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GridTile tilePrefab;
    public int numRows = 3;
    public int numColumns = 3;
    public float tileSize = 1;
    public Color clearedColor;
    GridTile[,] tileGrid;

    void Awake()
    {
        tileSize = tilePrefab.transform.localScale.x;
        tileGrid = new GridTile[numColumns,numRows];
        Vector2 dimensions = new(numColumns * tileSize, numRows * tileSize);
        dimensions = (-dimensions / 2) + new Vector2(tileSize / 2, tileSize / 2);
        for (int row = 0; row < numRows; row++) {
            for (int col = 0; col < numColumns; col++) {
                Vector2 pos = new(col * (tileSize), row * (tileSize));
                GridTile tile = Instantiate(tilePrefab, pos + dimensions, Quaternion.identity, transform);
                // save data for easy find later.
                tile.SetPos(col, row);
                tileGrid[col, row] = tile;
            }
        }
    }

    private void CheckGridLines()
    {
        List<int> rowsToClear = new();
        List<int> columnsToClear = new();
        int clearedAtOnce = 0;

        // checks rows first
        for (int row = 0; row < numRows; row++)
        {
            int totalOccupiedInRow = 0;
            for (int col = 0; col < numColumns; col++)
            {
                if (tileGrid[col, row].occupied)
                    totalOccupiedInRow++;
            }
            if (totalOccupiedInRow >= numRows)
            {
                rowsToClear.Add(row);
                clearedAtOnce++;
            }
        }
        // checks cols next
        for (int col = 0; col < numRows; col++)
        {
            int totalOccupiedInCol = 0;
            for (int row = 0; row < numColumns; row++)
            {
                if (tileGrid[col, row].occupied)
                    totalOccupiedInCol++;
            }
            if (totalOccupiedInCol >= numColumns)
            {
                columnsToClear.Add(col);
                clearedAtOnce++;
            }
        }

        // clear the tiles and add to score
        foreach (int row in rowsToClear)
        {
            for (int col = 0; col < numColumns; col++)
            {
                ClearTile(tileGrid[col, row], clearedColor);
            }
        }
        foreach (int col in columnsToClear)
        {
            for (int row = 0; row < numColumns; row++)
            {
                ClearTile(tileGrid[col, row], clearedColor);
            }
        }
        FindFirstObjectByType<PlayerManager>().UpdateScore( (int)Mathf.Pow( 2, clearedAtOnce) * 10);
    }

    // ~~~~~PIECE PLACING FUNCTIONS~~~~~
    public void PlacePiece(int id, Vector2Int position)
    {
        // make sure that the spots are open to change first
        if (!CheckID(id, position))
            return;
        PlaceTiles(id, position);
        CheckGridLines();
    }

    // gives the data of each piece
    static private Vector2Int[] SendData(int id) {
        Vector2Int[] data = id switch
        {
            // 1x1 piece - 0
            1 => new Vector2Int[1] { Vector2Int.zero },
            // 1x2 piece - 0o
            2 => new Vector2Int[2] { Vector2Int.zero, Vector2Int.right },
            // 1x3 piece - 0oo
            3 => new Vector2Int[3] { Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2 },
            // 1x4 piece - 0ooo
            4 => new Vector2Int[4] { Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2, Vector2Int.right * 3 },
            // 1x5 piece - 0oooo
            5 => new Vector2Int[5] { Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2, Vector2Int.right * 3, Vector2Int.right * 4 },
            // 2x1 piece
            6 => new Vector2Int[2] { Vector2Int.zero, Vector2Int.up },
            // 3x1 piece
            7 => new Vector2Int[3] { Vector2Int.zero, Vector2Int.up, Vector2Int.up * 2 },
            // 4x1 piece
            8 => new Vector2Int[4] { Vector2Int.zero, Vector2Int.up, Vector2Int.up * 2, Vector2Int.up * 3 },
            // 5x1 piece
            9 => new Vector2Int[5] { Vector2Int.zero, Vector2Int.up, Vector2Int.up * 2, Vector2Int.up * 3, Vector2Int.up * 4 },
            // 2x2 piece
            10 => new Vector2Int[4] { Vector2Int.zero, Vector2Int.right, Vector2Int.up, Vector2Int.one},
            // 3x3 piece
            11 => new Vector2Int[9] { Vector2Int.zero, Vector2Int.right, Vector2Int.right * 2, Vector2Int.up, Vector2Int.up * 2, Vector2Int.one, Vector2Int.one + Vector2Int.right, Vector2Int.one + Vector2Int.up, Vector2Int.one * 2},
            // only check the tile clicked, 1x1 - 0
            _ => new Vector2Int[1] { Vector2Int.zero }
        };
        return data;
    }

    // basically stores all the piece data since this should send the array of where to check to CheckMultiple()
    private bool CheckID(int id, Vector2Int position)
    {
        Vector2Int[] data = SendData(id);

        if (CheckMultiple(position, data))
            return true;
        else
            return false;
    }

    // check if multiple tiles are valid based on the array given, which should contain vectors local to the spot you clicked, not the grid
    private bool CheckMultiple(Vector2Int position, Vector2Int[] spots) {
        foreach (Vector2Int spot in spots)
        {
            if (!CheckTile(position + spot))
                return false;
        }
        return true;
    }

    // check if a single tile is valid, return true if so, false otherwise.
    private bool CheckTile(Vector2Int position) 
    {
        // check if the position given is even in the grid
        if (position.x >= 0 && position.x < numColumns && position.y >= 0 && position.y < numRows)
        {
            // check if the space is occupied by a brick already
            if (!tileGrid[position.x, position.y].occupied)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    // places all the pieces
    private void PlaceTiles(int id, Vector2Int position) {
        // NEVER should be empty
        Vector2Int[] data = SendData(id);
        foreach (Vector2Int point in data)
        {
            PlaceTile(tileGrid[ position.x + point.x, position.y + point.y] , Color.yellow);
        }
    }
    // places one single piece
    private void PlaceTile(GridTile t, Color c) { t.mySprite.color = c; t.occupied = true; }
    // clears one single piece
    private void ClearTile(GridTile t, Color c) { t.mySprite.color = c; t.occupied = false; }

}
