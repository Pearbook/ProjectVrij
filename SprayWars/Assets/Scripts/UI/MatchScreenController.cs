using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchScreenController : MonoBehaviour
{
    private bool allowControl;

    void Update()
    {
        if (GameManager.UI.matchIsOpen)
        {
            if (allowControl)
            {
                if (Input.GetAxisRaw("Submit") != 0)
                {
                    if(GameManager.Gameplay.redWins || GameManager.Gameplay.blueWins)
                    {
                        PlayerPrefs.SetInt("RedMatchPoints", 0);
                        PlayerPrefs.SetInt("BlueMatchPoints", 0);

                        SceneManager.LoadScene(0);

                        return;
                    }

                    PlayerPrefs.SetInt("RedMatchPoints", GameManager.Gameplay.RedMatchPoints);
                    PlayerPrefs.SetInt("BlueMatchPoints", GameManager.Gameplay.BlueMatchPoints);

                    SceneManager.LoadScene(1);
                }

                if (Input.GetAxisRaw("Cancel") != 0)
                {
                    PlayerPrefs.SetInt("RedMatchPoints", 0);
                    PlayerPrefs.SetInt("BlueMatchPoints", 0);

                    SceneManager.LoadScene(0);
                }
            }
            else
            {
                StartCoroutine(WaitForControl());
            }
        }
    }

    IEnumerator WaitForControl()
    {
        yield return new WaitForSeconds(0.5f);
        allowControl = true;
    }
}
