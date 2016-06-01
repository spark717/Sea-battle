using UnityEngine;
using System.Collections;

public class TurnGui : MonoBehaviour
{

    public GameManager GManager;
    public ClientRPCManager RpcHandler;

    public GUISkin Skin;
    public float Widht = 200.0f, Height = 100.0f;
    private string _turnMessage = "Prepare for battle!";

    void Update()
    {
        if (GManager.GameInProgress)
        {
           _turnMessage = GManager.GameStatus == EGameStatus.MyTurn ? "Your turn" : "Opponent turn";
        }
        if (GManager.GameStatus == EGameStatus.Defeat)
        {
            _turnMessage = "You are lose :/";
        }
        if (GManager.GameStatus == EGameStatus.Win)
        {
            _turnMessage = "You are win :)";
        }
    }

    void OnGUI()
    {
        if (Skin != null)
        {
            GUI.skin = Skin;
        }

        var centeredRect = new Rect((Screen.width - Widht) / 2, 2.5f, Widht, Height);
        var leftRect = new Rect(0.0f, 2.5f, Widht/1.5f, Height);
        var rightRect = new Rect((Screen.width - Widht/2), 2.5f, Widht/2, Height);

        GUILayout.BeginArea(centeredRect, GUI.skin.box);
        {
            GUILayout.Label(_turnMessage, GUI.skin.label);
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(leftRect, GUI.skin.box);
        {
            GUILayout.Label(("EnemyHP: " + GManager.EnemyHP), GUI.skin.label);
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(rightRect, GUI.skin.box);
        {
            GUILayout.Label("HP: " + GManager.HP, GUI.skin.label);
        }
        GUILayout.EndArea();


    }
}
