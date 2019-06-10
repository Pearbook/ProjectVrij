using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject DecalContainer;

    public void AddToDecalContainer(GameObject obj)
    {
        obj.transform.parent = DecalContainer.transform;
    }
}
