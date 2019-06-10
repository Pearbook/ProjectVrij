using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    public Vector3 Direction;
    public float Lifetime;

    private void Start()
    {
        StartCoroutine(WaitToDestroy());
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + (Direction.x * 10), transform.position.y, transform.position.z), 0.2f);
    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(Lifetime);
        Destroy(this.gameObject);
    }
}
