using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetAxisRaw("Submit") != 0)
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetAxisRaw("Cancel") != 0)
            Application.Quit();
    }
}
