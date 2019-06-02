using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamBehaviour : MonoBehaviour
{
    [Header("Movement")]
    public float ScaleSpeed = 1;
    public Vector3 Direction = new Vector3(1,0,0);
    public Vector3 TargetScale;
    public Transform ObjToScale;

    [Header("Impact Check")]
    public float Radius;
    public Transform CheckPosition;
    public LayerMask BuildingMask;

    public Texture2D MyBrush;

    private PlayerCanvas canvasHit;

    private void Start()
    {
        StartCoroutine(ScaleBeam(ScaleSpeed));
    }

    private void Update()
    {
        CheckForBuilding();
    }

    void CheckForBuilding()
    {
        RaycastHit hit;
        Ray ray = new Ray(CheckPosition.position, Direction);

        if (Physics.Raycast(ray, out hit, 5f, BuildingMask))
        {
            print(hit.collider.name);
            canvasHit = hit.collider.GetComponent<PlayerCanvas>();

            canvasHit.AddToCanvas(hit.textureCoord, MyBrush);
            StartCoroutine(WaitForDestroy());
        }

        /*
        Collider[] coll = Physics.OverlapSphere(CheckPosition.position, Radius, BuildingMask);

        if(coll.Length > 0)
        {
            StartCoroutine(WaitForDestroy());S
        }*/
    }

    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    IEnumerator ScaleBeam(float time)
    {
        var i = 0.0f;
        var rate = 1.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            ObjToScale.localScale = Vector3.Lerp(ObjToScale.localScale, TargetScale, i);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(CheckPosition.position, Radius);
    }
}
