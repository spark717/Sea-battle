using System;
using UnityEngine;
using System.Collections;

public class CreateTilesForField : MonoBehaviour
{

    public GameObject tilePrefab;
	
	void Awake()
	{
	    for (float x = -4.5f, i = 0; x <= 4.5f; x += 1.0f, i++)
	    {
	        for (float z = -4.5f, j = 0; z <= 4.5f; z += 1.0f, j++)
	        {
	            var tile = (GameObject) Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
	            SetTilePositionInField(tile, x, z);
                SetTilePositionInMassive(tile, Convert.ToByte(i), Convert.ToByte(j));
                tile.name = string.Format("Tile {0},{1}", i, j);
	        }
	    }
	}

    private void SetTilePositionInField(GameObject tile, float x, float z)
    {
        var tileTransform = tile.transform;
        tileTransform.SetParent(transform);
        tileTransform.localPosition = new Vector3(x, 0.08f, z);
        tileTransform.localRotation = tilePrefab.transform.rotation;
    }

    private void SetTilePositionInMassive(GameObject tile, byte i, byte j)
    {
        if (name == "EnemyField")
        {
            tile.GetComponent<MouseEvents>().SetPosInMassive(i, j);
        }
        if (name == "MyField")
        {
            tile.GetComponent<TriggerTileHandle>().SetPosInMassive(i, j);
        }
    }
}
