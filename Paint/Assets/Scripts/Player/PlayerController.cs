﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Serializable]
    public class SpraySettings
    {
        public GameObject SprayPrefab;
        public Transform SprayPivot;
        public float SprayDistance;
        public LayerMask SprayMask;

        public Texture2D BrushTextureOne;
        public Texture2D BrushTextureTwo;
    }

    public SpraySettings spraySettings = new SpraySettings();

    private bool isButtonPressed;

    private void Update()
    {
        if (GetComponent<PlayerProperties>().PlayerID == 1)
        {
            ButtonPress("Spray_p1");
        }
        else if (GetComponent<PlayerProperties>().PlayerID == 2)
        {
            ButtonPress("Spray_p2");
        }
    }

    void Paint()
    {
        RaycastHit hit;

        if (Physics.Raycast(spraySettings.SprayPivot.position, Vector3.forward, out hit, spraySettings.SprayDistance, spraySettings.SprayMask))
        {
            if (GetComponent<PlayerProperties>().PlayerID == 1)
                hit.collider.gameObject.GetComponent<PlayerCanvas>().AddToCanvas(hit.textureCoord, spraySettings.BrushTextureOne);
            else if (GetComponent<PlayerProperties>().PlayerID == 2)
                hit.collider.gameObject.GetComponent<PlayerCanvas>().AddToCanvas(hit.textureCoord, spraySettings.BrushTextureTwo);

        }
    }

    void Spray()
    {
        RaycastHit hit;

        if (Physics.Raycast(spraySettings.SprayPivot.position, Vector3.forward, out hit, spraySettings.SprayDistance, spraySettings.SprayMask))
        {
            GameObject sprayObj = (GameObject)Instantiate(GameManager.Player.GetPlayerSpray(GetComponent<PlayerProperties>().PlayerID), hit.point, Quaternion.LookRotation(hit.normal));

            sprayObj.transform.parent = GameManager.Level.LevelContainer;

            if(!hit.collider.GetComponent<SpraySpotBehaviour>().hasBeenSprayed)
                hit.collider.GetComponent<SpraySpotBehaviour>().SprayThisSpot(GetComponent<PlayerProperties>().PlayerID);
        }
    }

    void ButtonPress(string key)
    {
        if (Input.GetAxisRaw(key) != 0)
        {
            if (isButtonPressed == false)
            {
                //Spray();
                Paint();
                //isButtonPressed = true;
            }
        }
        if (Input.GetAxisRaw(key) == 0)
        {
            isButtonPressed = false;
        }
    }
}
