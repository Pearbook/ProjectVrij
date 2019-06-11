using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PlayerDrawing : MonoBehaviour
{

    public Transform DrawPivot;

    [SerializeField, Range(0f, 2f)] float threshold = 1.5f;
    public float ShapeEndDistance = 1;

    public Material MeshMaterial;

    public LayerMask DrawMask;

    public GameObject IndicatorPrefab;
    private GameObject activeIndicator;
    public Texture2D MyBrush;
    private PlayerCanvas hitCanvas;

    public LayerMask PickUpMask;

    private ParticleSystem myParticle;
    private GameObject myBeam;

    private Vector3 direction;
    private List<Vector3> drawPoints = new List<Vector3>();
    private Vector3 centerOfShape;
    private float surfaceArea;
    private Vector2 textureCoord;

    private bool isDrawing;
    private bool isButtonPressed;
    private bool hasPickup;

    private PlayerProperties prop;

    private void Start()
    {
        drawPoints = new List<Vector3>();

        prop = GetComponent<PlayerProperties>();

        if(GetComponent<PlayerProperties>().PlayerID == 1)
        {
            direction = Vector3.right;
            MyBrush = GameManager.Player.GetPlayerBrush(1);
            myBeam = GameManager.Player.BeamPrefabs[0];
            myParticle = prop.SprayParticles[0];

            GameManager.Player.PlayerLines[0].receiveShadows = false;
        }

        if (GetComponent<PlayerProperties>().PlayerID == 2)
        {
            direction = Vector3.left;
            MyBrush = GameManager.Player.GetPlayerBrush(2);
            myBeam = GameManager.Player.BeamPrefabs[1];
            myParticle = prop.SprayParticles[1];

            GameManager.Player.PlayerLines[1].receiveShadows = false;
        }

        myParticle.transform.GetChild(0).gameObject.SetActive(false);
        myParticle.Stop();
    }

    private void Update()
    {
        if (GameManager.Gameplay.GameHasStarted)
        {
            GameManager.Player.PlayerLines[prop.PlayerID - 1].positionCount = drawPoints.Count;
            
            for(int i = 0; i < drawPoints.Count; ++i)
            {
                if(prop.PlayerID == 1)
                    GameManager.Player.PlayerLines[prop.PlayerID - 1].SetPosition(i, new Vector3(-drawPoints[i].z, drawPoints[i].y, drawPoints[i].x - 0.01f));
                if (prop.PlayerID == 2)
                    GameManager.Player.PlayerLines[prop.PlayerID - 1].SetPosition(i, new Vector3(drawPoints[i].z, drawPoints[i].y, -(drawPoints[i].x + 0.01f)));
            }

            if (prop.AllowControl && prop.AllowPainting)
            {
                if (prop.PlayerID == 1)
                    ButtonPress("Jump_p1");
                else if (prop.PlayerID == 2)
                    ButtonPress("Jump_p2");
            }
            else
            {
                isDrawing = false;
            }

            if (isDrawing)
            {
                if (prop.AllowPainting)
                {
                    Vector3 point = GetDrawPointVector();

                    if (activeIndicator == null)
                    {
                        activeIndicator = (GameObject)Instantiate(IndicatorPrefab, point, Quaternion.identity);
                    }

                    if (activeIndicator != null && drawPoints.Count > 0)
                    {
                        activeIndicator.transform.position = drawPoints[0];
                        activeIndicator.SetActive(true);
                    }

                    if (!myParticle.isPlaying)
                    {
                        myParticle.Play();
                        myParticle.transform.GetChild(0).gameObject.SetActive(true);
                    }

                    hitCanvas.AddToCanvas(textureCoord, MyBrush);

                    if (drawPoints.Count <= 0 || Vector3.Distance(point, drawPoints.Last()) > threshold)
                    {
                        drawPoints.Add(point);
                    }

                    // Only reduce when moving
                    if (prop.isMoving)
                        prop.ReducePaint(); // Reduce paint
                    else
                        prop.StopPaint();

                    //Play Spray Audio
                    GameManager.Player.PlayerAudio[prop.PlayerID - 1].PlaySpraySound();
                }
                else
                    isDrawing = false;
            }
            else
            {
                if (activeIndicator != null)
                    activeIndicator.SetActive(false);

                if (myParticle.isPlaying)
                {
                    myParticle.Stop();
                    myParticle.transform.GetChild(0).gameObject.SetActive(false);
                }

                // Stop reducing paint
                prop.StopPaint();

                //Stop spray audio
                GameManager.Player.PlayerAudio[prop.PlayerID - 1].StopSpraySound();

                Clear();
            }
        }
    }


    void ButtonPress(string key)
    {
        if (Input.GetAxisRaw(key) != 0)
        {
            if (isButtonPressed == false)
            {
                if (!isDrawing)
                {
                    isDrawing = true;
                    hasPickup = false;
                    Clear();
                }
            }
        }
        if (Input.GetAxisRaw(key) == 0)
        {
            isDrawing = false;

            if (drawPoints.Count > 5)
                DoBeam();
            else
                Clear();

            isButtonPressed = false;
        }
    }

    void DoBeam()
    {
        // Check if the shape is closed
        // Get the center of the shape
        // Spawn the beam
        // DEV NOTE: I would like to get the surface of the shape to change the damage parameter.

        float dist = Vector3.Distance(drawPoints.First(), drawPoints.Last());

        if(dist <= ShapeEndDistance)
        {
            centerOfShape = FindCenterPoint();
            surfaceArea = Area();

            // CHECK IF A OBJECT IS WITHING THE SHAPE
            hasObjectWithin();

            if(!hasPickup)
                SpawnBeam();
        }
        else
        {
            Clear();
        }
    }

    void SpawnBeam()
    {
        GameObject obj = (GameObject)Instantiate(myBeam, centerOfShape, Quaternion.identity);

        Clear();
    }

    void hasObjectWithin()
    {
        /*
        for (int i = 0; i < drawPoints.Count; ++i)
        {
            Vector3 dir = (centerOfShape - drawPoints[i]).normalized;
            float dist = Vector3.Distance(centerOfShape, drawPoints[i]);

            RaycastHit hit;
            Ray ray = new Ray(centerOfShape, dir);

            Debug.DrawRay(centerOfShape, dir * dist, Color.blue);

            if (Physics.Raycast(ray, out hit, dist, PickUpMask))
            {
                hasPickup = true;
                hit.collider.GetComponent<PickUpBehaviour>().PickUp(prop.PlayerID);

                StartCoroutine(ClearLine());
            }
        }*/

        float dist = Vector3.Distance(centerOfShape, drawPoints.First());

        Collider[] coll = Physics.OverlapSphere(centerOfShape, dist, PickUpMask);

        if(coll.Length > 0 && coll[0] != null)
        {
            hasPickup = true;
            coll[0].GetComponent<PickUpBehaviour>().PickUp(prop.PlayerID);

            StartCoroutine(ClearLine());
        }

    }

    Vector3 FindCenterPoint()
    {
        Vector3 center = new Vector3();
        int count = 0;

        foreach(Vector3 point in drawPoints)
        {
            center += point;
            count++;
        }

        return center / count;
    }

    public float Area()
    {  
        Vector3 result = Vector3.zero;
        for (int p = drawPoints.Count - 1, q = 0; q < drawPoints.Count; p = q++)
        {
            result += Vector3.Cross(drawPoints[q], drawPoints[p]);
        }
        //Debug.Log(result);
        result *= 0.5f;
        return result.magnitude;
    }

    IEnumerator ClearLine()
    {
        yield return new WaitForSeconds(0.1f);
        drawPoints.Clear();
        centerOfShape = Vector3.zero;
        surfaceArea = 0;

        hasPickup = false;
    }

    void Clear()
    {
        drawPoints.Clear();
        centerOfShape = Vector3.zero;
        surfaceArea = 0;

    }

    Vector3 GetDrawPointVector()
    {
        RaycastHit hit;

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Ray ray = new Ray(DrawPivot.position, direction);

        Debug.DrawRay(DrawPivot.position, direction, Color.red);

        if (Physics.Raycast(ray, out hit, 100f, DrawMask))
        {
            textureCoord = hit.textureCoord;
            hitCanvas = hit.collider.GetComponent<PlayerCanvas>();

            return hit.point;
        }

        //Clear();
        return Vector3.zero;
    }

    private void OnDrawGizmos()
    { 
        for (int i = 0; i < drawPoints.Count; ++i)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(drawPoints[i], 0.2f);
        }

        if (centerOfShape != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(centerOfShape, 0.2f);
        }

        if (drawPoints.Count > 0)
        {
            Gizmos.color = new Color(0, 255, 0, 80);
            Gizmos.DrawCube(drawPoints[0], new Vector3(ShapeEndDistance, ShapeEndDistance, ShapeEndDistance));
        }
    }

}
