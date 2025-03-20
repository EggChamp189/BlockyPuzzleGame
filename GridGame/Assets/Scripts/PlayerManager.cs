using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    public GridManager gridData;
    GridTile hoveringTile = null;
    Color defaultColor = new(25, 38, 55, 1);
    bool wasOnTileLastFrame = false;
    public GameObject[] helperPieces;
    public int pieceID = 0;
    public int maxPieces = 11;
    bool wasChangedThisFrame = false;
    public GameObject helper;
    public TMP_Text scoreText;
    public int score = 0;
    public bool isDead = false;
    private void Start()
    {
        helper.transform.localScale = Vector3.one * gridData.tileSize;
        UpdateScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        wasChangedThisFrame = false;
        // check if the mouse has moved
        if (Input.mousePositionDelta.magnitude != 0 && !isDead) {
            OnMouseMove();
        }

        if (wasChangedThisFrame && !isDead) {
            Destroy(helper);
            helper = Instantiate(helperPieces[pieceID]);
            helper.transform.localScale = Vector3.one * gridData.tileSize;
        }

        if (!wasOnTileLastFrame && !isDead)
        {
            helper.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward * 10;
        }
        else 
        {
            helper.transform.position = hoveringTile.gameObject.transform.position;
        }


        if (Input.GetMouseButtonUp(0) && !isDead) {
            MouseClick();
        }
    }

    public void UpdateHelper(int id) {
        pieceID = id;
        Destroy(helper);
        helper = Instantiate(helperPieces[pieceID]);
        helper.transform.localScale = Vector3.one * gridData.tileSize;
    }

    public void UpdateScore(int by) {
        score += by;
        scoreText.text = "Score: " + score;
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
            if (hit.collider.CompareTag("Tile") && hit.collider.GetComponent<GridTile>() != null && pieceID != 0)
            {
                // mouse moved to a new tile
                if (hoveringTile != hit.collider.GetComponent<GridTile>())
                {
                    hoveringTile = hit.collider.GetComponent<GridTile>();
                    wasOnTileLastFrame = true;
                }
                gridData.PlacePiece(pieceID, hit.collider.GetComponent<GridTile>().myPos);
            }
        }
        else if (wasOnTileLastFrame)
        {
            hoveringTile = null;
            wasOnTileLastFrame = false;
        }
    }
}
