using UnityEngine;
using System.Collections.Generic;

public class ClientRPCManager : Photon.PunBehaviour
{
    private PhotonPlayer _opponentClient;
    public GameManager GManager;
    private readonly RequestDictionary _requests = new RequestDictionary();



    public void InitPlayers()
    {
        _opponentClient = PhotonNetwork.otherPlayers[0];
    }



    public void FirstTurn()
    {
        if (PhotonNetwork.isMasterClient)
        {
            GManager.SwitchTurn();
            //if (Random.value < 0.5f)
            //{
                
                //SetFirstTurn();
           // }
            //else
            //{
            //    photonView.RPC("SetFirstTurn", _opponentClient);
            //}
        }
    }


    [PunRPC]
    private void SetFirstTurn()
    {
       // GManager.SwitchTurn();
    }



    public void CheckOpponentTileForShip(int i, int j)
    {
        GManager.SwitchTurn();
        var code = _requests.AddNew(i, j);
        photonView.RPC("CheckTileForShip", _opponentClient, code, i, j);
        Debug.Log("Message Sent");
    }

    [PunRPC]
    private void CheckTileForShip(int code, int i, int j)
    {
        GManager.SwitchTurn();
        var isHit = GManager.ShipInTile(i, j);
        photonView.RPC("Feedback", _opponentClient, code, isHit);
    }

    [PunRPC]
    private void Feedback(int code, bool isHit)
    {
        int i, j;
        _requests.TryGet_IJ(code, out i, out j);
        _requests.CloseRequest(code);
        GManager.SetEnemyTileStatus(isHit, i, j);
        Debug.Log(isHit ? ("Подбит! " + i + ", " + j) : ("Промах! " + i + ", " + j));
    }
}





public class RequestDictionary
{
    private readonly List<int> _code = new List<int>();
    private readonly List<bool> _isOpen = new List<bool>();
    private readonly List<int> _i = new List<int>();
    private readonly List<int> _j = new List<int>();
    private int _count;

    public int AddNew(int i, int j)
    {
        ++_count;
        _code.Add(_count);
        _isOpen.Add(true);
        _i.Add(i);
        _j.Add(j);
        Debug.Log("Request added!");
        return _count;
    }

    public bool TryGet_IJ(int requestCode, out int i, out int j)
    {
        i = 0;
        j = 0;
        if (!_code.Contains(requestCode)) return false;
        var pos = _code.IndexOf(requestCode);
        i = _i[pos];
        j = _j[pos];
        return true;
    }

    public void CloseRequest(int requestCode)
    {
        if (!_code.Contains(requestCode)) return;
        var pos = _code.IndexOf(requestCode);
        _isOpen[pos] = false;
    }
}
