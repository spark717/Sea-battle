using UnityEngine;
using System.Collections;


public class TriggerTileHandle : MonoBehaviour
{

   /// <summary>
    /// 0 - empty, 1-4 - count of sided ships, 10 - with ship;
   /// </summary>
    public int Status { get; set; }
    public int I { get; private set; }
    public int J { get; private set; }
    private Transform debugCube;

    void Awake()
    {
        Status = 0;
    }

    void Start()
    {
        debugCube = transform.FindChild("DebugCube");
    }

    void Update()
    {
        //DEBUG
        if (Status > 0)
        {
            debugCube.gameObject.SetActive(true);
        }
        if (Status == 0)
        {
            debugCube.gameObject.SetActive(false);
        }
    }

    public void SetPosInMassive(byte i, byte j)
    {
        I = i;
        J = j;
    }

}
