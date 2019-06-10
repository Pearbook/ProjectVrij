using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    public Renderer MyRenderer;

    public Texture2D MyTexture;
    public int TextureSizeX = 256;
    public int TextureSizeY = 256;

    public Texture2D BrushTexture;

    RenderTexture rt;

    private Color32[] colorData;

    private void Start()
    {
        rt = new RenderTexture(TextureSizeX, TextureSizeY, 32);

        //MyCamera.targetTexture = rt;

        MyRenderer.material.SetTexture("_BaseColorMap", rt);

        Graphics.Blit(MyTexture, rt);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CheckForPixels();
        }
    }

    public void AddToCanvas(Vector2 pos, Texture2D brush)
    {
        Vector2 texCoordToPixels = new Vector2(TextureSizeX * pos.x, TextureSizeY * pos.y);

        RenderTexture.active = rt;

        GL.PushMatrix();

        GL.LoadPixelMatrix(0, TextureSizeX, TextureSizeY, 0);

        Graphics.DrawTexture(new Rect(texCoordToPixels.x - brush.width / 2, (rt.height - texCoordToPixels.y) - brush.height / 2, brush.width, brush.height), brush);
        //Graphics.DrawTexture(new Rect(0 - BrushTexture.width / 2, (rt.height - 0) - BrushTexture.height / 2, BrushTexture.width, BrushTexture.height), BrushTexture, new Rect(0, 0, BrushTexture.width, BrushTexture.height), 0, 0, 0, 0, Color.red, null);

        GL.PopMatrix();

        RenderTexture.active = null;
    }

    public Vector2 CheckForPixels()
    {
        Texture2D tex = new Texture2D(TextureSizeX, TextureSizeY, TextureFormat.RGB24, false);

        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, TextureSizeX, TextureSizeY), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        colorData = tex.GetPixels32();

        int redPixels = 0;
        int bluePixels = 0;

        for(int i = 0; i < colorData.Length; ++i)
        {
            if (colorData[i].r == 255 && colorData[i].g == 71 && colorData[i].b == 71)
                redPixels++;
            if (colorData[i].r == 37 && colorData[i].g == 161 && colorData[i].b == 255)
                bluePixels++;
        }

        return new Vector2(redPixels, bluePixels);
    }

}
