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

    private ParticleSystem myParticle;
    private GameObject myBeam;

    private Vector3 direction;
    private List<Vector3> drawPoints = new List<Vector3>();
    private Vector3 centerOfShape;
    private float surfaceArea;
    private Vector2 textureCoord;

    private bool isDrawing;
    private bool isButtonPressed;

    private void Start()
    {
        drawPoints = new List<Vector3>();

        if(GetComponent<PlayerProperties>().PlayerID == 1)
        {
            direction = Vector3.right;
            MyBrush = GameManager.Player.GetPlayerBrush(1);
            myBeam = GameManager.Player.BeamPrefabs[0];
            myParticle = GetComponent<PlayerProperties>().SprayParticles[0];
        }

        if (GetComponent<PlayerProperties>().PlayerID == 2)
        {
            direction = Vector3.left;
            MyBrush = GameManager.Player.GetPlayerBrush(2);
            myBeam = GameManager.Player.BeamPrefabs[1];
            myParticle = GetComponent<PlayerProperties>().SprayParticles[1];
        }
    }

    private void Update()
    {
        if (GetComponent<PlayerProperties>().PlayerID == 1)
            ButtonPress("Jump_p1");
        else if (GetComponent<PlayerProperties>().PlayerID == 2)
            ButtonPress("Jump_p2");

        if (isDrawing)
        {
            Vector3 point = GetDrawPointVector();

            if(activeIndicator == null)
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
                    Clear();
                }
            }
        }
        if (Input.GetAxisRaw(key) == 0)
        {
            isDrawing = false;

            if (drawPoints.Count > 0)
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

    /*
    void Build()
    {
        Poly2Mesh.Polygon poly = new Poly2Mesh.Polygon();
        poly.outside = drawPoints;

        Poly2Mesh.CreateGameObject(poly, MeshMaterial);

        Clear();
    }
    */

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

        Clear();
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
