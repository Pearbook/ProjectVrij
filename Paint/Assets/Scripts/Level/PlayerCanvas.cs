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

    private void Start()
    {
        rt = new RenderTexture(TextureSizeX, TextureSizeY, 32);

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

    void CheckForPixels()
    {
        Texture mainTexture = MyRenderer.material.GetTexture("_BaseColorMap");
        Texture2D texture2D = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);

        /*
        RenderTexture currentRT = rt;

        RenderTexture renderTexture = new RenderTexture(mainTexture.width, mainTexture.height, 32);
        Graphics.Blit(mainTexture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        Color[] pixels = texture2D.GetPixels();

        RenderTexture.active = currentRT;

        int redPixels = 0;

        for(int i = 0; i < pixels.Length; ++i)
        {
            if (pixels[i].r >= 125)
                redPixels++;

        }

        print(redPixels);*/

        
        var whitePixels = 0;
        var blackPixels = 0;

        for (int i = 0; i < texture2D.width; i++)
            for (int j = 0; j < texture2D.height; j++)
            {
                Color pixel = texture2D.GetPixel(i, j);

                // if it's a white color then just debug...
                if (pixel.r == 255)
                    whitePixels++;
                else
                    blackPixels++;
            }

        Debug.Log(string.Format("White pixels {0}, black pixels {1}", whitePixels, blackPixels));
        
    }

}
