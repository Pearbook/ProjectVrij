using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    public int PlayerID;
    public int Score;
    public bool AllowControl = false;
    public bool AllowPainting = false;
    public bool isMoving;

    [Header("Paint")]
    [Range(0, 100)]
    public float MaxPaint;
    public float CurrentPaint;
    public float ReductionSpeed;
    private bool isReducing;
    private bool isRefilling;
    private float refillSpeed;

    [Header ("Visuals")]
    public List<GameObject> PlayerGraphic;
    public List<GameObject> PaintCannister;
    public List<ParticleSystem> SprayParticles;
    public List<MeshRenderer> SprayCanRenderer;

    private Vector3 prevPos;

    private void Start()
    {
        CurrentPaint = MaxPaint;

        if (PlayerID == 1)
            PaintCannister[0].SetActive(true);
        if (PlayerID == 2)
            PaintCannister[1].SetActive(true);

    }

    private void Update()
    {
        if (GameManager.Gameplay.GameHasStarted)
        {
            if (AllowPainting)
            {
                if (PlayerID == 1)
                {
                    SprayCanRenderer[0].material.SetFloat("_Amount", Custom.map(CurrentPaint, 0, 100, 0, 155));
                    PaintCannister[0].GetComponent<MeshRenderer>().material.SetVector("_amount", new Vector4(0, Custom.map(CurrentPaint, 0, 100, -100, 100), 0, 0));
                }
                if (PlayerID == 2)
                {
                    SprayCanRenderer[1].material.SetFloat("_Amount", Custom.map(CurrentPaint, 0, 100, 0, 155));
                    PaintCannister[1].GetComponent<MeshRenderer>().material.SetVector("_amount", new Vector4(0, Custom.map(CurrentPaint, 0, 100, -100, 100), 0, 0));
                }

                if (CurrentPaint <= 0)
                {
                    AllowPainting = false;
                    StopPaint();
                }
            }
            else
            {
                if (CurrentPaint > 0)
                    AllowPainting = true;
            }


            
            if (CurrentPaint > MaxPaint)
            {
                CurrentPaint = MaxPaint;
                isRefilling = false;
                //StopAllCoroutines();
            }

            if (isRefilling)
            {
                isRefilling = false;
            }
        }
    }

    public void RefillPaint(float speed)
    {
        if(!isRefilling)
        {
            if (CurrentPaint < MaxPaint)
            {
                isRefilling = true;
                refillSpeed = speed;

                StartCoroutine(Refilling());
            }
        }
    }

    public void ReducePaint()
    {
        if(!isReducing)
        {
            isReducing = true;
            StartCoroutine(Reducing());
        }
    }

    public void StopPaint()
    {
        if (isReducing)
        {
            isReducing = false;
            StopAllCoroutines();
        }
    }

    IEnumerator Refilling()
    {
        while(isRefilling)
        {
            yield return new WaitForSeconds(refillSpeed);
            CurrentPaint++;
        }
    }

    IEnumerator Reducing()
    {
        while(isReducing)
        {
            yield return new WaitForSeconds(ReductionSpeed);
            CurrentPaint--;
        }
        
    }

}
