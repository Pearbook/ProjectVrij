using UnityEngine;
using System.Collections;

public class Pixelation : MonoBehaviour
{

    public RenderTexture renderTexture;

    public Camera AdditionalCamera;

    void Start()
    {
        int realRatio = Mathf.RoundToInt(Screen.width / Screen.height);
        //renderTexture.width = NearestSuperiorPowerOf2(Mathf.RoundToInt(renderTexture.width * realRatio));
        Debug.Log("(Pixelation)(Start)renderTexture.width: " + renderTexture.width);
    }

    void OnGUI()
    {
        /*
        GUI.depth = 20;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), renderTexture);

        AdditionalCamera.Render();
        */
    }

    int NearestSuperiorPowerOf2(int n)
    {
        return (int)Mathf.Pow(2, Mathf.Ceil(Mathf.Log(n) / Mathf.Log(2)));
    }
}