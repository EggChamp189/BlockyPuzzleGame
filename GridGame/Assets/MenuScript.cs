using UnityEngine;

public class MenuScript : MonoBehaviour
{
    PlayerManager player;
    static int maxPieces = 1;
    public int[] myIDs = new int[3];
    public int playerUsing = 0; // tracks the id that the player has currently selected
    GameObject lastClicked;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerManager>();
        maxPieces = player.maxPieces;
        myIDs[0] = RandomID();
        myIDs[1] = RandomID();
        myIDs[2] = RandomID();
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
            playerUsing = num;
        }
    }

    public static int RandomID() {
        return Random.Range(1, maxPieces + 1);
    }

    public void PiecePlaced()
    {
        GetButtonNumber(0);
        myIDs[playerUsing] = RandomID();
    }

    // not a placement, but the player is selecting a piece
    public void GetButtonNumber(int number) { player.UpdateHelper(myIDs[number]); }
}
