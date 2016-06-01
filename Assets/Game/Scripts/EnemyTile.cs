using UnityEngine;
using System.Collections;

public enum EnemyTileStatus
{
    Unknown,
    Empty,
    Ship
}


public class EnemyTile : MonoBehaviour {

    public EnemyTileStatus Status
    {
        get { return _status;}
        set
        {
            _status = value;
            switch (value)
            {
                case EnemyTileStatus.Ship:
                    _xMark.gameObject.SetActive(true);
                    break;
                case EnemyTileStatus.Empty:
                    _emptyMark.gameObject.SetActive(true);
                    break;
            }
        }
    }
    private EnemyTileStatus _status = EnemyTileStatus.Unknown;

    private Transform _xMark;
    private Transform _emptyMark;

    void Awake()
    {
        _xMark = transform.FindChild("XMark");
        _emptyMark = transform.FindChild("EmptyMark");
        if (!_xMark)
        {
           Debug.LogError("XMark not found!");
        }
        if (!_emptyMark)
        {
            Debug.LogError("EmptyMark not found!");
        }
    }



}
