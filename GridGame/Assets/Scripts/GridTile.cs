using UnityEngine;
using UnityEngine.UIElements;

public class GridTile : MonoBehaviour
{
    public Vector2Int myPos = new(0,0);
    public SpriteRenderer mySprite;
    public bool occupied = false;

    public void SetPos(int col, int row)
    {
        myPos = new(col, row);
        Debug.Log("Tile loaded at: col " + myPos.x + ", row " + myPos.y);
    }
}
