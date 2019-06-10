using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public GameObject CarPrefab;
    public List<GameObject> CarSpawnPoints;

    bool isSpawning = true;

    private void Start()
    {
        StartCoroutine(WaitForCarSpawn());
    }

    void SpawnCar()
    {
        int randomPoint = Random.Range(0, 4);
        GameObject car;

        if (randomPoint > 1)
            return;

        car = (GameObject)Instantiate(CarPrefab, CarSpawnPoints[randomPoint].transform.position, Quaternion.identity);
        
        if (randomPoint == 0)
            car.GetComponent<CarBehaviour>().Direction = Vector3.right;

        if (randomPoint == 1)
            car.GetComponent<CarBehaviour>().Direction = Vector3.left;
    }

    IEnumerator WaitForCarSpawn()
    {
        while(isSpawning)
        {
            yield return new WaitForSeconds(1);
            SpawnCar();
        }
    }

    private void Update()
    {

    }
}
