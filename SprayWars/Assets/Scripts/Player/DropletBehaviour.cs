using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropletBehaviour : MonoBehaviour
{
    public float MovementSpeed = 0.2f;
    public LayerMask Mask;

    public List<GameObject> Decal;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y - 5f, transform.position.z), MovementSpeed);
        CheckForGround();
    }

    void CheckForGround()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        if(Physics.Raycast(ray, out hit, 0.5f, Mask))
        {
            Destroy(this.gameObject);
            GameObject obj = (GameObject)Instantiate(Decal[Random.Range(0, Decal.Count)], hit.point, Quaternion.Euler(0,Random.Range(0, 360),0));

            GameManager.Level.AddToDecalContainer(obj);
        }
    }
}
