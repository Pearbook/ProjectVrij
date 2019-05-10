using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Properties")]
    public GameObject MyCamera;
   
    [Header ("Shooting")]
    public float ThrowForce;
    public float ThrowSpin;

   // public PlantScriptable CurrentPlantAmmo;

    public GameObject ObjectToShoot;
    public GameObject Eraser;

   // [Header("Ammo")]
   // public List<PlantScriptable> AllAmmo;

    [Header("Other")]
    public CircularMenu Menu;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (Input.GetMouseButtonDown(0))
        {
            //Shoot();
        }

        if (Input.GetKeyDown(KeyCode.E))
            ToggleMenu();

        /*
        if (Input.GetKeyDown(KeyCode.Alpha1))
            CurrentPlantAmmo = AllAmmo[0];

        if (Input.GetKeyDown(KeyCode.Alpha2))
            CurrentPlantAmmo = AllAmmo[1];

        if (Input.GetKeyDown(KeyCode.Alpha3))
            CurrentPlantAmmo = AllAmmo[2];

        if (Input.GetKeyDown(KeyCode.Alpha4))
            CurrentPlantAmmo = AllAmmo[AllAmmo.Count-1];
            */
    }

    // SOME OF THESE THINGS SHOULD BE DONE IN A PLAYER MANAGER
    void ToggleMenu()
    {
        Menu.OpenMenu();

        /*
        if(Menu.isOpen)
            this.GetComponent<PlayerController>().enabled = false;
        else
            this.GetComponent<PlayerController>().enabled = true;*/
    }

    /*
    public void Shoot()
    {
        GameObject obj = null;

        if (CurrentPlantAmmo.isEraser)
        {
            obj = (GameObject)Instantiate(Eraser, MyCamera.transform.position, Quaternion.identity);
        }
        else
        {
            obj = (GameObject)Instantiate(ObjectToShoot, MyCamera.transform.position, Quaternion.identity);
            obj.GetComponent<SeedBehaviour>().MyPlantType = CurrentPlantAmmo;
        }

        Rigidbody rigid = obj.GetComponent<Rigidbody>();

        rigid.velocity = MyCamera.transform.TransformDirection(Vector3.forward * ThrowForce);
        rigid.angularVelocity = new Vector3(ThrowSpin, ThrowSpin, ThrowSpin);
    }

    public void SetAmmo(int index)
    {
        CurrentPlantAmmo = AllAmmo[index];
    }*/
}
