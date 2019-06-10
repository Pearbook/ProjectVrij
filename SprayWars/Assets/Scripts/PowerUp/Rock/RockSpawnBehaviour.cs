using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawnBehaviour : MonoBehaviour
{
    public Transform RockSpawnPoint;
    public int SpawnAmount;
    private int currentAmount;
    public float SpawnRate;

    public GameObject RockPrefab;

    public Vector3 Direction = Vector3.left;

    private bool isSpawing;

    private void Start()
    {
        transform.localScale = new Vector3(Direction.x, 1, 1);
        StartCoroutine(SpawnRocks());
    }

    private void Update()
    {
        if(currentAmount >= SpawnAmount)
            Destroy(this.gameObject);
    }

    IEnumerator SpawnRocks()
    {
        while(currentAmount < SpawnAmount)
        {
            Spawn();
            currentAmount++;
            yield return new WaitForSeconds(SpawnRate);
        }
    }

    void Spawn()
    {
        RockSpawnPoint.transform.localPosition = new Vector3(RockSpawnPoint.transform.localPosition.x, RockSpawnPoint.transform.localPosition.y, Random.Range(-2, -14));

        GameObject obj = (GameObject)Instantiate(RockPrefab, RockSpawnPoint.transform.position, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
    }
}
