using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public int SpawnInterfall;

    private int allowSpawn = 2;

    [Header("Power Ups")]
    public List<GameObject> AllPowerUpPrefabs;

    [Header("Bounds")]
    public List<GameObject> BoundsObject;
    public Vector2 Size;

    private void Update()
    {
        if (GameManager.Gameplay.timerIsOn)
        {
            if (allowSpawn == 0)
                StartCoroutine(WaitForSpawn());
            if (allowSpawn == 1)
            {
                SpawnPowerUp();
                allowSpawn = 2;
            }
        }
        else
        {
            StopAllCoroutines();
            Destroy(BoundsObject[0].GetComponent<PowerUpSpawnPointBehaviour>().MyPowerUp.gameObject);
            Destroy(BoundsObject[1].GetComponent<PowerUpSpawnPointBehaviour>().MyPowerUp.gameObject);
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(FirstSpawnDelay());
    }

    IEnumerator FirstSpawnDelay()
    {
        yield return new WaitForSeconds(SpawnInterfall);
        allowSpawn = 0;
    }

    void SpawnPowerUp()
    {
        /*
        float redScore = GameManager.Gameplay.GetPixels(1);
        float blueScore = GameManager.Gameplay.GetPixels(2);
        */


        float redScore = 1;
        float blueScore = 1;

        if (redScore > blueScore)
        {
            if (Random.value < 0.7f)
                Spawn(2); // BLUE
            else
                Spawn(1); // RED
        }

        if (blueScore > redScore)
        {
            if (Random.value < 0.7f)
                Spawn(1); // RED
            else
                Spawn(2); // BLUE
        }

        if (blueScore == redScore)
        {
            if (Random.value < 0.5f)
                Spawn(1); // RED
            else
                Spawn(2); // BLUE
        }
    }

    void Spawn(int id)
    {
        if (BoundsObject[id - 1].GetComponent<PowerUpSpawnPointBehaviour>().MyPowerUp == null)
        {
            Vector2 pos = GetSpawnPos(id - 1);

            GameObject obj = (GameObject)Instantiate(SelectPowerUp(), new Vector3(BoundsObject[id - 1].transform.position.x, pos.y, pos.x), Quaternion.identity);

            BoundsObject[id - 1].GetComponent<PowerUpSpawnPointBehaviour>().MyPowerUp = obj;
        }
    }

    GameObject SelectPowerUp()
    {
        return AllPowerUpPrefabs[Random.Range(0, AllPowerUpPrefabs.Count)];
    }

    Vector2 GetSpawnPos(int id)
    {
        float minPosX = BoundsObject[id].transform.position.z - Size.x / 2;
        float maxPosX = BoundsObject[id].transform.position.z + Size.x / 2;
        float minPosY = BoundsObject[id].transform.position.y - Size.y / 2;
        float maxPosY = BoundsObject[id].transform.position.y + Size.y / 2;

        Vector2 result = new Vector2(Random.Range(minPosX, maxPosX), Random.Range(minPosY, maxPosY));

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(BoundsObject[0].transform.position, new Vector3(0.01f, Size.y, Size.x));
        Gizmos.DrawWireCube(BoundsObject[1].transform.position, new Vector3(0.01f, Size.y, Size.x));
    }

    IEnumerator WaitForSpawn()
    {
        allowSpawn = 1;
        yield return new WaitForSeconds(SpawnInterfall);
        allowSpawn = 0;
    }

    public void RemovePowerUp(int id)
    {
        Destroy(BoundsObject[id - 1].GetComponent<PowerUpSpawnPointBehaviour>().MyPowerUp);
        BoundsObject[id - 1].GetComponent<PowerUpSpawnPointBehaviour>().MyPowerUp = null;
    }

}
