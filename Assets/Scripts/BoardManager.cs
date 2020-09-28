//FIXME ???SWitching back and forth instantly dun,nowhuy
                //System.Threading.Thread.Sleep(1000)
                //MOVE CHARACTERS FUNCTION SHOULD HANDLE THE POLLING BECAUSE WAIT CLICK IS CALLED EVERY REFRESH

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance{set;get;}
    private bool[,] allowedMoves{set;get;} 
    public Character[] Characters{set; get;}
    private Character selectedCharacter;
    public BoardHighlights xxx;
    public BoardHighlights[] BoardHighlightss{set;get;}
    private byte playerTurn;
    private byte numPlayers;
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;
    private int selectionX = -1;
    private int selectionY = -1;
    private byte Width, Height;
    public bool[,] moves{set;get;}
    public List<GameObject> playerCharPrefabs;
    private List<GameObject> activePlayerChar = new List<GameObject>();

    public ProgressBar[] PBS{set;get;}
    public ProgressBar pb1, pb2;


    private void Start()
    {   
       // Debug.Log("GAME STARTED");
        Width = 8;
        Height = 8;
        numPlayers = 0;    
        playerTurn = 0;
        xxx = BoardHighlights.Instance;
        SpawnAllPlayers();
        selectedCharacter = Characters[0];
    }

    private void Update()
    {
        BoardHighlights.Instance.Hidehighlights();
        UpdateSelection();
        DrawBoard();
        UpdateHealth();
        waitClick();
        //ReceivePlayerActions();
    }

    struct input
    { 
        public int i, x, y; 
    }; 

    private void waitClick()
    {
        input[] inputs = new input[numPlayers];
        for (int i = 0; i < numPlayers; i++)
        {
            BoardHighlights.Instance.HighlightAllowedMoves(selectedCharacter.PossibleMoves());
            if (Input.GetMouseButtonDown(0) && (selectionX >= 0 && selectionY >= 0))
            {
                input temp;
                temp.i = i; temp.x = selectionX; temp.y = selectionY;
                inputs[i] = temp;
                //FIXME ???SWitching back and forth instantly dun,nowhuy
                //System.Threading.Thread.Sleep(1000)
                SwitchPlayer();System.Threading.Thread.Sleep(100);
            }
        }


    }
    
    private void ReceivePlayerActions()
    {
        //waitClick();

    }

    private void SwitchPlayer()    //TEMP
    {
        if (selectedCharacter == Characters[numPlayers-1])
        {
            selectedCharacter = Characters[0];
            playerTurn = 0;
        }
        else
        {
            playerTurn++;
            selectedCharacter = Characters[playerTurn];
        }
        Debug.Log("Current Player Turn: " + (playerTurn+1).ToString());
    }

    private void MoveCharacter(int x, int y)
    {
        if(selectedCharacter.moves[x,y])
        {
            //FIXME
            //Characters[selectedCharacter.CurrentX, selectedCharacter.CurrentY] = null;
            selectedCharacter.transform.position = GetTileCenter(x,y);
            selectedCharacter.SetPosition(x,y);
            selectedCharacter.Health--;
            for (int i = 0; i < numPlayers && i != selectedCharacter.getIndex(); i++)
            {
                Debug.Log("INDEX " + i);
            }
        }
 
    }
    private void UpdateSelection()
    {
        if (!Camera.main)
            return;
        
        RaycastHit hit; 
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit, 25.0f, LayerMask.GetMask("Plane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z; 
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    public void SpawnPlayer(int index, int x, int y, byte H, byte W, byte S)
    {

        GameObject go = Instantiate(playerCharPrefabs[index], GetTileCenter(x,y), Quaternion.identity) as GameObject;
        //Debug.Log("GO DONE");
        go.transform.SetParent(transform);
        Characters[index] = go.GetComponent<Character> (); 
        
        //pbs[index] = public ProgressBar     
        /*if (go.GetComponent<Character>() == null){
        Debug.Log("NULl");
        }
        else
        Debug.Log("Not Null");*/
        Characters[index].SetPosition(x,y);
        Characters[index].SetDimensions(H,W);
        Characters[index].SetSpeed(S);
        Characters[index].CheckMoves();
        Characters[index].SetHealth(100);
        Characters[index].SetIndex(index);

        //pbs[index].BarValue = 75;
        activePlayerChar.Add(go);
        numPlayers++;
        //Characters[x,y].PrintCoordinates();
    }

    
    public void SpawnAllPlayers()
    {
        //Debug.Log("GO DONE");
        activePlayerChar = new List<GameObject> ();
        Characters = new Character[2];
        PBS = new ProgressBar[2];
        //RANDOM SPAWN POINTS, SPECIFIC TO MAP SPAWN BOUNDARIES==================================================`          
        SpawnPlayer(0,3,4,Height,Width,2);
        PBS[0] = pb1; PBS[1] = pb2;
        //Debug.Log("numplayers = " + numPlayers.ToString() + " after spawning all players");
        //numPlayers++; Move to SpawnPlayer
        //Debug.Log("numplayers = " + numPlayers.ToString());
        SpawnPlayer(1,6,6,Height,Width,2);
        //numPlayers++; MOVED TO SpawnPlayer
        
        //Debug.Log("numPlayers = " + numPlayers.ToString() + " after spawning all players");
    } 

        private Vector3 GetTileCenter(int x, int z)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * z) + TILE_OFFSET;

        return origin;
    }
    private void DrawBoard()
    {
        Vector3 widthLine = Vector3.right * Width;
        Vector3 heightLine = Vector3.forward * Height;

        for (int i = 0; i <= Height; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= Width; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }

        }

        if(selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

            Debug.DrawLine(
                Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
    }

    private void UpdateHealth()
    {
        //int arrlen = 0; int i = 0;
        //while (Characters[i] != null){
           // arrlen++;
            //i++;
       // }
            PBS[0].BarValue = Characters[0].Health;
            PBS[1].BarValue = Characters[1].Health;
            //pb2.BarValue = 69;
        }


    
}
