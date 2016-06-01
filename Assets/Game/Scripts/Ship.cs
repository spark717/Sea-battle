using System.Collections.Generic;
using UnityEngine;


public class Ship : MonoBehaviour
{
	public static Dictionary<string, sbyte[,]> Semantics;
    
    //Parameters of ship
    private string _rotation;
    public sbyte[,] Semantic;
    public bool OnBoard { get; set; }
    //public int HP { get; private set; }

    static Ship()
    {
        SetShipTypes();
    }


    void Awake()
    {
        OnBoard = false;
    }

    void Start()
    {
        _rotation = "Horizontal";
        SetSemantic();
    }


    public void SwapRotation()
    {
        switch (_rotation)
        {
            case "Horizontal":
                _rotation = "Vertical";               
                transform.rotation = Quaternion.AngleAxis(-90.0f, Vector3.forward);
                break;
            case "Vertical":
                _rotation = "Horizontal";
                transform.rotation = Quaternion.AngleAxis(0.0f, Vector3.forward);
                break;
        }
        SetSemantic();
    }


    //public void ReduceHP()
    //{
    //    --HP;
    //}


    private void SetSemantic()
    {
        switch (name)
        {
            case "Single-Deck Ship":
                Semantic = Semantics["SingleDeck"];
                break;
            case "Double-Deck Ship":
                Semantic = Semantics["DoubleDeck" + _rotation];
                break;
            case "Triple-Deck Ship":
                Semantic = Semantics["TripleDeck" + _rotation];
                break;
            case "Four-Deck Ship":
                Semantic = Semantics["FourDeck" + _rotation];
                break;
            default:
                Debug.LogError("Error in set-up ship type");
                break;
        }
    }


    private static void SetShipTypes()
    {
        Semantics = new Dictionary<string, sbyte[,]>
        {
            {
                "SingleDeck", new sbyte[,]
                {
                    {0, 0, 0},
                    {0, 1, 0},
                    {0, 0, 0}
                }
            },
            {
                "DoubleDeckHorizontal", new sbyte[,]
                {
                    {0, 0, 0, 0},
                    {0, 1, 1, 0},
                    {0, 0, 0, 0}
                }
            },
            {
                "DoubleDeckVertical", new sbyte[,]
                {
                    {0, 0, 0},
                    {0, 1, 0},
                    {0, 1, 0},
                    {0, 0, 0}
                }
            },
            {
                "TripleDeckHorizontal", new sbyte[,]
                {
                    {0, 0, 0, 0, 0},
                    {0, 1, 1, 1, 0},
                    {0, 0, 0, 0, 0}
                }
            },
            {
                "TripleDeckVertical", new sbyte[,]
                {
                    {0, 0, 0},
                    {0, 1, 0},
                    {0, 1, 0},
                    {0, 1, 0},
                    {0, 0, 0}
                }
            },
            {
                "FourDeckHorizontal", new sbyte[,]
                {
                    {0, 0, 0, 0, 0, 0},
                    {0, 1, 1, 1, 1, 0},
                    {0, 0, 0, 0, 0, 0}
                }
            },
            {
                "FourDeckVertical", new sbyte[,]
                {
                    {0, 0, 0},
                    {0, 1, 0},
                    {0, 1, 0},
                    {0, 1, 0},
                    {0, 1, 0},
                    {0, 0, 0}
                }
            }
        };
    }
}
