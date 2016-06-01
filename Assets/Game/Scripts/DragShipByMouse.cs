using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Xml.Schema;


public class DragShipByMouse : MonoBehaviour
{
    public GameManager GManager;
    private Transform myField;
    private Ship thisShip;
    private Vector3 mouseDownDelta;
    private float distanceToCamera;
    private Vector3 startPosition;
    private readonly List<Collider> _triggeredTiles = new List<Collider>();
    private readonly List<TriggerTileHandle> _tilesAroundShip = new List<TriggerTileHandle>();
    private Collider _headTriggeredTile;



    void Awake()
    {
        GManager = FindObjectOfType<GameManager>();
        CalculateDistanceBetweenObjectAndCamera();
        thisShip = GetComponent<Ship>();
        myField = GameObject.Find("MyField").transform;
    }


    void Start()
    {
        startPosition = transform.position;
    }


    //---------MOUSE EVENT BLOCK----------------------------
    void OnMouseDown()
    {
        if (GManager.GameStatus != EGameStatus.BuildingMode) return;

        mouseDownDelta = transform.position - MouseToWorldPoint();
        if (_triggeredTiles.Count > 0)
        {
            thisShip.OnBoard = false;
            UnfillTiles();
        }
    }

    void OnMouseDrag()
    {
        if (GManager.GameStatus != EGameStatus.BuildingMode) return;

        transform.position = MouseToWorldPoint() + mouseDownDelta;
        if (Input.GetMouseButtonDown(1))
        {
            thisShip.SwapRotation();            
        }
    }

    void OnMouseUp()
    {
        if (GManager.GameStatus != EGameStatus.BuildingMode) return;

        if (_triggeredTiles.Count > 0)
        {
            GetShipPositionInField();
            StartCoroutine(WaitForFixedUpdate());
        }
    }


    private void GetShipPositionInField()
    {      
        //var minDistance = float.MaxValue;
        //int minPos = 0;
       // var distances = new float[_triggeredTiles.Count];
        var distances = new List<float>();
        foreach (var tile in _triggeredTiles)
        {
            var thisPosition = transform.position;
            var tilePosition = tile.transform.position;
            var dist = Vector3.SqrMagnitude((tilePosition - thisPosition));
            //if (distance < minDistance)
            //{
            //    minDistance = distance;
            //    minPos = i;
            //}
            distances.Add(dist);
        }
        var minPos = distances.IndexOf(distances.Min());
        _headTriggeredTile = _triggeredTiles[minPos];
        transform.position = _headTriggeredTile.transform.position;
    }

    private IEnumerator WaitForFixedUpdate()
    {
        yield return new WaitForFixedUpdate();

        Debug.Log(_triggeredTiles.Count);  

        CheckTilesIsEmpty();             
    }

    private void CheckTilesIsEmpty()
    {
        var tilesIsEmpty = true;
        foreach (var tile in _triggeredTiles)
        {
            var tileScript = tile.GetComponent<TriggerTileHandle>();
            if (tileScript.Status > 0)
            {
                tilesIsEmpty = false;                
            }
        }

        if (!tilesIsEmpty)
        {
            transform.position = startPosition;
            
        }
        else
        {
            thisShip.OnBoard = true;
            FillTiles();
        }
    }




    private void FillTiles()
    {     
        var widht = thisShip.Semantic.GetUpperBound(1) + 1;
        var height = thisShip.Semantic.GetUpperBound(0) + 1;

        var j = _headTriggeredTile.GetComponent<TriggerTileHandle>().J + 1; 
        for (int k = 0; k < height; k++, j--)
        {
            var i = _headTriggeredTile.GetComponent<TriggerTileHandle>().I - 1;
            for (int l = 0; l < widht; l++, i++)
            {
                var tile = myField.FindChild(string.Format("Tile {0},{1}", i, j));
                if (tile == null) continue;
                var tileScript = tile.GetComponent<TriggerTileHandle>();
                if (thisShip.Semantic[k, l] == 1)
                {
                    tileScript.Status = 10;                   
                }
                if (thisShip.Semantic[k, l] == 0)
                {
                    ++tileScript.Status;
                    _tilesAroundShip.Add(tileScript);
                }
            }            
        }

        //foreach (var tile in _triggeredTiles)
        //{
        //    //сначала заполняем ячейки короблями
        //    var tileScript = tile.GetComponent<TriggerTileHandle>();
        //    tileScript.Status = 10;

        //    //затем заполняем соседние ячейки
        //    var i = tileScript.I;
        //    var j = tileScript.J;
        //    for (int k = -1; k <=1; k++)
        //    {
        //        for (int l = -1; l <= 1; l++)
        //        {
        //            var sideTile = MyField.FindChild(string.Format("Tile {0},{1}", (i + k), (j + l)));
        //            if (sideTile == null) continue;
        //            tileScript = sideTile.GetComponent<TriggerTileHandle>();
        //            if (tileScript.Status == 10) continue;
        //            ++tileScript.Status;
        //        }
        //    }
        //}

        //DEBUG
        //for (int i = 0; i < 10; i++)
        //{
        //    for (int j = 0; j < 10; j++)
        //    {
        //        Transform debugTile = MyField.FindChild(string.Format("Tile {0},{1}", (i), (j)));
        //        if ((debugTile.GetComponent<TriggerTileHandle>()).Contains != Filler.Empty)
        //        {
        //            Transform dc = debugTile.FindChild("DebugCube");
        //            dc.gameObject.SetActive(true);
        //        }
        //    }
        //}
    }

    private void UnfillTiles()
    {
        foreach (var tile in _triggeredTiles)
        {
            tile.GetComponent<TriggerTileHandle>().Status = 0;
        }
        foreach (var tile in _tilesAroundShip)
        {
            --tile.Status;
        }
        _tilesAroundShip.Clear();       


        //foreach (var tile in _triggeredTiles)
        //{
        //    //сначала освобождаем ячейки с короблями
        //    var tileScript = tile.GetComponent<TriggerTileHandle>();
        //    tileScript.Status = 0;

        //    //DEBUG
        //    //var dc = tile.transform.FindChild("DebugCube");
        //    //dc.gameObject.SetActive(false);

        //    //затем заполняем соседние ячейки
        //    var i = tileScript.I;
        //    var j = tileScript.J;
        //    for (int k = -1; k <= 1; k++)
        //    {
        //        for (int l = -1; l <= 1; l++)
        //        {
        //            var sideTile = myField.FindChild(string.Format("Tile {0},{1}", (i + k), (j + l)));
        //            if (sideTile == null) continue;
        //            tileScript = sideTile.GetComponent<TriggerTileHandle>();
        //            //if (tileScript.Status != 0) continue;
        //            ++tileScript.Status;
        //        }
        //    }
        //}
    }

    

    private Vector3 MouseToWorldPoint()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToCamera));
    }

    private void CalculateDistanceBetweenObjectAndCamera ()
    {
        distanceToCamera = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
    }





    //--------------------TRIGGER BLOCK--------------------------
    void OnTriggerEnter(Collider col)
    {
        if ((col.gameObject.name.Contains("Tile")) &&
            (!_triggeredTiles.Contains(col)))
        {
            _triggeredTiles.Add(col);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if ((col.gameObject.name.Contains("Tile")) &&
            (_triggeredTiles.Contains(col)))
        {
            _triggeredTiles.Remove(col);
        }
    }    
}