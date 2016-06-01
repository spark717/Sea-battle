using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EntryMenuGUI : MonoBehaviour
{


    public GUISkin Skin;
    public float Widht = 200.0f, Height = 100.0f;

    void OnGUI()
    {
        if (Skin != null)
        {
            GUI.skin = Skin;
        }

        var centeredRect = new Rect((Screen.width - Widht) / 2, (Screen.height - Height) / 2, Widht, Height);

        GUILayout.BeginArea(centeredRect, GUI.skin.box);
        {
            GUILayout.Label("Морской бой", GUI.skin.label);
            if (GUILayout.Button("Играть", GUI.skin.button))
            {
                SceneManager.LoadScene(1, LoadSceneMode.Single);   
            }
        }
        GUILayout.EndArea();
    }
}
