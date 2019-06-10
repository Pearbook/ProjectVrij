using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public int SpawnInterfall;

    private int allowSpawn;
    

    [Header("Bounds")]
    public List<GameObject> BoundsObject;
    public Vector2 Size;

    public Vector2 test;

    private void Update()
    {
        if (allowSpawn == 0)
            StartCoroutine(WaitForSpawn());
        if (allowSpawn == 1)
        {
            SpawnPowerUp();
            allowSpawn = 2;
        }
    }

    void SpawnPowerUp()
    {
        test = GetSpawnPos();
    }

    Vector2 GetSpawnPos()
    {
        float minPosX = BoundsObject[0].transform.position.z - Size.x / 2;
        float maxPosX = BoundsObject[0].transform.position.z + Size.x / 2;
        float minPosY = BoundsObject[0].transform.position.y - Size.y / 2;
        float maxPosY = BoundsObject[0].transform.position.y + Size.y / 2;

        Vector2 result = new Vector2(Random.Range(minPosX, maxPosX), Random.Range(minPosY, maxPosY));

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(new Vector3(BoundsObject[0].transform.position.x, test.y, test.x), 0.2f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(BoundsObject[0].transform.position, new Vector3(0.01f, Size.y, Size.x));
    }

    IEnumerator WaitForSpawn()
    {
        allowSpawn = 1;
        yield return new WaitForSeconds(SpawnInterfall);
        allowSpawn = 0;
    }

}
