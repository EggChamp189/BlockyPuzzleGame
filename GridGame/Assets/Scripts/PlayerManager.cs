using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    GridTile hoveringTile = null;
    Color defaultColor = new(25, 38, 55, 1);
    bool wasOnTileLastFrame = false;
    public GameObject[] helperPieces;
    public int pieceID = 0;
    int maxPieces = 5;
    bool wasChangedThisFrame = false;
    public GameObject helper;

    private void Start()
    {
        helper.transform.localScale = Vector3.one * FindFirstObjectByType<GridManager>().tileSize;
    }

    // Update is called once per frame
    void Update()
    {
        wasChangedThisFrame = false;
        // check if the mouse has moved
        if (Input.mousePositionDelta.magnitude != 0) {
            OnMouseMove();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (pieceID <= 1)
                pieceID = maxPieces;
            else
                pieceID--;
            wasChangedThisFrame = true;
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            if (pieceID >= maxPieces)
                pieceID = 0;
            else
                pieceID++;
            wasChangedThisFrame = true;
        }

        if (wasChangedThisFrame) {
            Destroy(helper);
            helper = Instantiate(helperPieces[pieceID]);
            helper.transform.localScale = Vector3.one * FindFirstObjectByType<GridManager>().tileSize;
        }

        if (!wasOnTileLastFrame)
        {
            helper.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10;
        }
        else 
        {
            helper.transform.position = hoveringTile.gameObject.transform.position;
        }


        if (Input.GetMouseButtonUp(0)) {
            MouseClick();
        }
    }

    void OnMouseMove() {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Tile"))
            {
                // mouse moved to a new tile, reset the color on the old tile then change the new.
                if (hoveringTile != hit.collider.GetComponent<GridTile>())
                {
                    hoveringTile = hit.collider.GetComponent<GridTile>();
                    wasOnTileLastFrame = true;
                }

            }
        }
        else if (wasOnTileLastFrame)
        {
            hoveringTile = null;
            wasOnTileLastFrame = false;
        }
    }

    private void MouseClick()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Tile"))
            {
                // mouse moved to a new tile
                if (hoveringTile != hit.collider.GetComponent<GridTile>())
                {
                    hoveringTile = hit.collider.GetComponent<GridTile>();
                    wasOnTileLastFrame = true;
                }
                FindFirstObjectByType<GridManager>().PlacePiece(pieceID, hit.collider.GetComponent<GridTile>().myPos);
            }
        }
        else if (wasOnTileLastFrame)
        {
            hoveringTile = null;
            wasOnTileLastFrame = false;
        }
    }
}
