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

    [Header("Visuals")]
    public GameObject DropletPrefab;
    private bool isActive;

    private int waiting;

    private void Start()
    {
        isActive = true;
        StartCoroutine(DropRandomDrops());

        StartCoroutine(ScaleBeam(ScaleSpeed));
    }

    private void Update()
    {
        CheckForBuilding();

        if(waiting == 1)
        {
            StartCoroutine(MakeBeamThin(0.3f));
            waiting = 2;
        }
    }

    IEnumerator DropRandomDrops()
    {
        while(isActive)
        {
            yield return new WaitForSeconds(Random.Range(0f, 0.3f));
            Instantiate(DropletPrefab, CheckPosition.position, Quaternion.identity);
        }
    }

    void CheckForBuilding()
    {
        RaycastHit hit;
        Ray ray = new Ray(CheckPosition.position, Direction);

        if (Physics.Raycast(ray, out hit, 5f, BuildingMask))
        {
            canvasHit = hit.collider.GetComponent<PlayerCanvas>();

            canvasHit.AddToCanvas(hit.textureCoord, MyBrush);
            StartCoroutine(WaitForDestroy());
        }

    }

    IEnumerator WaitForDestroy()
    {
        if(waiting == 0)
            waiting = 1;
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

    IEnumerator MakeBeamThin(float time)
    {
        var i = 0.0f;
        var rate = 1.0f / time;

        yield return new WaitForSeconds(0.2f);

        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 0, 0), i);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(CheckPosition.position, Radius);
    }
}
