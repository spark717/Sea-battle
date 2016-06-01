using UnityEngine;
using System.Collections;
using System.Linq;

public enum ETileStatus
{
    Empty,
    Ship
}

public enum EGameStatus
{
    BuildingMode,
    SearchForOpponent,
    MyTurn,
    EnemyTurn,
    Win,
    Defeat
}

public class GameManager : MonoBehaviour
{

    public ConnectAndJoinRandom ConnectToRandomRoom;
    public ShowStatusWhenConnecting ConnectingStatusGUI;
    public ClientRPCManager RpcHandler;
    public Transform MyField;
    public Transform EnemyField;
    //public StartGameGUI StartGameButton;

    private ETileStatus[,] _myFieldMartix;
    public int HP { get; private set; }
    public int EnemyHP { get; private set; }
    public EGameStatus GameStatus { get; private set; }

    public bool GameInProgress
    {
        get { return (GameStatus == EGameStatus.MyTurn || GameStatus == EGameStatus.EnemyTurn); }
    }


    void Awake()
    {
        GameStatus = EGameStatus.BuildingMode;
        HP = 20;
        EnemyHP = HP;

        _myFieldMartix = new ETileStatus[10, 10];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                _myFieldMartix[i, j] = ETileStatus.Empty;
            }
        }
    }


    void Update()
    {
       // bool allPlayersInRoom = (GameStatus == EGameStatus.InRoom && PhotonNetwork.room.playerCount == 2);
        bool allPlayersInRoom = (PhotonNetwork.inRoom && PhotonNetwork.room.playerCount == 2 && GameStatus == EGameStatus.SearchForOpponent);
        if (allPlayersInRoom) StartGame();
    }


    private void StartGame()
    {
        GameStatus = EGameStatus.EnemyTurn;
        RpcHandler.InitPlayers();       
        RpcHandler.FirstTurn();
    }


    public void OnStartSearchButtonDown()
    {
       // if (CheckForAllShipsOnBoard())
        {           
            EnemyField.gameObject.SetActive(true);
            FillMatrixWithShips();
            GameStatus = EGameStatus.SearchForOpponent;
            ConnectToRandomRoom.Connect();
        }
       // else Debug.Log("Not all ships in board");
    }



    private bool CheckForAllShipsOnBoard()
    {
        var allShipsOnBoard = false;
        var ships = FindObjectsOfType<Ship>();
        var shipsOnBoardCount = ships.Count(ship => ship.OnBoard);

        if (shipsOnBoardCount == ships.Length) allShipsOnBoard = true;

        return allShipsOnBoard;
    }


    private void FillMatrixWithShips()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                var tile = MyField.FindChild(string.Format("Tile {0},{1}", i, j));
                var tileScript = tile.GetComponent<TriggerTileHandle>();
                if (tileScript.Status == 10)
                {
                    _myFieldMartix[i, j] = ETileStatus.Ship;
                }
            }
        }
    }


    public bool ShipInTile(int i, int j)
    {
        var isHit = _myFieldMartix[i, j] == ETileStatus.Ship;
        if (isHit) --HP;
        if (HP == 0) GameStatus = EGameStatus.Defeat;
        SetMyTileStatus(isHit, i, j);
        return isHit;
    }


    public void SwitchTurn()
    {
        switch (GameStatus)
        {
            case EGameStatus.MyTurn:
                GameStatus = EGameStatus.EnemyTurn;
                Debug.LogWarning("Curent turm: Enemy");
                break;
            case EGameStatus.EnemyTurn:
                GameStatus = EGameStatus.MyTurn;
                Debug.LogWarning("Curent turm: My");
                break;
        }       
    }


    public void SetMyTileStatus(bool isHit, int i, int j)
    {
        var tile = MyField.FindChild(string.Format("Tile {0},{1}", i, j));
        if (isHit)
        {
            tile.FindChild("XMark").gameObject.SetActive(true);
        }
        else
        {
            tile.FindChild("EmptyMark").gameObject.SetActive(true);
        }

    }


    public void SetEnemyTileStatus(bool isHit, int i, int j)
    {
        var tile = EnemyField.FindChild(string.Format("Tile {0},{1}", i, j));
        var tileScript = tile.GetComponent<EnemyTile>();
        tileScript.Status = isHit ? EnemyTileStatus.Ship : EnemyTileStatus.Empty;
        if (isHit) --EnemyHP;
        if (EnemyHP == 0) GameStatus = EGameStatus.Win;
    } 
}
