using UnityEngine;
using System.Collections;

public class ShowStatusWhenConnecting : MonoBehaviour 
{
    public GUISkin Skin;
    public GameManager GManager;

    void Awake()
    {
        //enabled = false;
    }

    void OnGUI()
    {
        if (GManager.GameStatus != EGameStatus.SearchForOpponent) return;
        if( Skin != null )
        {
            GUI.skin = Skin;
        }

        float width = 400;
        float height = 200;

        Rect centeredRect = new Rect( ( Screen.width - width ) / 2, ( Screen.height - height ) / 2, width, height );

        GUILayout.BeginArea( centeredRect, GUI.skin.box );
        {
            GUILayout.Label( "Connecting" + GetConnectingDots(), GUI.skin.customStyles[ 0 ] );
            GUILayout.Label( "Status: " + PhotonNetwork.connectionStateDetailed );
            GUILayout.Label("Player ID: " + PhotonNetwork.player.ID);

            if (PhotonNetwork.connectionStateDetailed == PeerState.Joined)
            {
                GUILayout.Label("Players in room: " + PhotonNetwork.room.playerCount);
                GUILayout.Label("We are master client?: " + PhotonNetwork.isMasterClient);
            }
        }
        GUILayout.EndArea();

        if (PhotonNetwork.inRoom && enabled && PhotonNetwork.room.playerCount == 2)
        {
           enabled = false;
        }
    }

    string GetConnectingDots()
    {
        string str = "";
        int numberOfDots = Mathf.FloorToInt( Time.timeSinceLevelLoad * 3f % 4 );

        for( int i = 0; i < numberOfDots; ++i )
        {
            str += " .";
        }

        return str;
    }
}
