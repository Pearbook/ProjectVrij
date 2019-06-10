using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainDropBehaviour : MonoBehaviour
{
    public LayerMask CanvasMask;
    private PlayerCanvas hitCanvas;

    [HideInInspector]
    public Vector3 Direction = new Vector3(-1, 0, 0);

    public Texture2D MyBrush;

    public float Lifetime;

    private Vector2 textureCoord;

    private void Update()
    {
        DrawOnCanvas();
        StartCoroutine(WaitToDestroy());
    }

    void DrawOnCanvas()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Direction);

        if(Physics.Raycast(ray, out hit, 10f, CanvasMask))
        {
            textureCoord = hit.textureCoord;
            hitCanvas = hit.collider.GetComponent<PlayerCanvas>();
        }

        if(hitCanvas != null)
        {
            hitCanvas.AddToCanvas(textureCoord, MyBrush);
        }
    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(Lifetime);
        Destroy(this.gameObject);
    }
}
