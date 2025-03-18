using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuScript : MonoBehaviour
{
    PlayerManager player;
    static int maxPieces = 1;
    public int[] myIDs = new int[3];
    public TMP_Text[] displays = new TMP_Text[3];
    public GameObject[] displayImages = new GameObject[3];
    public int playerUsing = 0; // tracks the id that the player has currently selected

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerManager>();
        maxPieces = player.maxPieces;
        for (int i = 0; i < 3; i++)
        {
            myIDs[i] = RandomID();
        }
        UpdateDisplay();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3)) {
            int num;
            if (Input.GetKeyDown(KeyCode.Alpha1))
                num = 0;
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                num = 1;
            else
                num = 2;
            GetButtonNumber(num);
            playerUsing = num + 1;
        }
    }

    public static int RandomID() {
        return Random.Range(1, maxPieces + 1);
    }

    public void PiecePlaced()
    {
        player.UpdateHelper(0);
        myIDs[playerUsing - 1] = RandomID();
        playerUsing = 0;
        UpdateDisplay();
    }

    public void UpdateDisplay() {
        for (int i = 0; i < 3; i++)
        {
            displays[i].text = player.helperPieces[myIDs[i]].name;
            // change the image's color depending on if the piece can be placed or not.
            if (FindFirstObjectByType<GridManager>().CheckBoard(myIDs[i]))
                displayImages[i].GetComponent<RawImage>().color = Color.white;
            else
                displayImages[i].GetComponent<RawImage>().color = Color.gray;

        }

    }

    // not a placement, but the player is selecting a piece
    public void GetButtonNumber(int number) { player.UpdateHelper(myIDs[number]); }
}
