using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBehaviour : MonoBehaviour
{
    public GameObject RainPrefab;

    public List<Vector3> RainVectors;
    private List<GameObject> allRainDrops = new List<GameObject>();

    public GameObject CloudObj;

    public Vector3 Direction = Vector3.left;
    public float Lifetime;
    private bool isRaining;

    private void Start()
    {
        transform.localScale = new Vector3(Direction.x, 1, 1);
        CloudObj.transform.localPosition = new Vector3(-1.5f, 0, Random.Range(2, 12.5f));

        isRaining = true;
        StartCoroutine(Rain(0.5f));
        StartCoroutine(WaitForDestroy(Lifetime));
    }

    IEnumerator Rain(float time)
    {
        while(isRaining)
        {
            for (int i = 0; i < RainVectors.Count; ++i)
            {
                GameObject obj = (GameObject)Instantiate(RainPrefab, CloudObj.transform.position + RainVectors[i], Quaternion.identity);
                obj.GetComponent<RainDropBehaviour>().Direction = this.Direction;

                allRainDrops.Add(obj);
            }
            yield return new WaitForSeconds(time);
            
        }
    }

    IEnumerator WaitForDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

    private void Update()
    {

        allRainDrops.RemoveAll(item => item == null);

        if (allRainDrops.Count > 0)
        {
            foreach (GameObject drop in allRainDrops)
            {
                drop.transform.position = Vector3.MoveTowards(drop.transform.position, new Vector3(drop.transform.position.x, -10, drop.transform.position.z), 0.2f);
            }
        }
    }
}
