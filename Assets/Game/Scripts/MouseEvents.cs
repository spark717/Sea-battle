using UnityEngine;
using System.Collections;

public class MouseEvents : MonoBehaviour
{

    private GameManager GManager; 

    public int I { get; private set; }
    public int J { get; private set; }

    private Material _material;
    private MeshCollider _collider;
    private bool isMouseOverObject = false;

	void Start ()
	{
	    GManager = FindObjectOfType<GameManager>();
	    _material = GetComponent<Renderer>().material;
	    _collider = GetComponent<MeshCollider>();
	}

    public void SetPosInMassive(byte i, byte j)
    {
        I = i;
        J = j;
    }

    void OnClick()
    {
        Debug.Log(I + " : " + J);
        if (GManager.GameStatus == EGameStatus.MyTurn)
        {
            GManager.RpcHandler.CheckOpponentTileForShip(I, J);
            _collider.enabled = false;
        }
    }

    void OnMouseEnter()
    {
        isMouseOverObject = true;

        if (GManager.GameStatus == EGameStatus.MyTurn)
        {         
            StartCoroutine(Flash());
        }
    }

    void OnMouseExit()
    {
        isMouseOverObject = false;
    }

    private IEnumerator Flash()
    {
        var myColor = _material.GetColor("_TintColor");
        myColor.a = 0.0f;
        for (float f = 0.0f; f <= 1.0f; f += 0.08f)
        {
            if (!isMouseOverObject) break;       
            var lerped = Mathf.Lerp(0.0f, 1.0f, f);
            myColor.a = lerped;
            _material.SetColor("_TintColor", myColor);

            yield return new WaitForSeconds(0.01f);
        }

       yield return new WaitWhile(() => isMouseOverObject);

        myColor.a = 0;
       _material.SetColor("_TintColor", myColor);
    }
}
