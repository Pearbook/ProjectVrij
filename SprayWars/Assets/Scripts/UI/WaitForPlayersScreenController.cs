using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitForPlayersScreenController : MonoBehaviour
{
    private bool oneIsReady;
    private bool twoIsReady;

    public GameObject WaitTextOne, ReadyTextOne;
    public GameObject WaitTextTwo, ReadyTextTwo;


    private void Awake()
    {
        GameManager.Player.DisableContol();
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Jump_p1") != 0)
        {
            oneIsReady = true;
            WaitTextOne.SetActive(false);
            ReadyTextOne.SetActive(true);
        }

        if (Input.GetAxisRaw("Jump_p2") != 0)
        {
            twoIsReady = true;
            WaitTextTwo.SetActive(false);
            ReadyTextTwo.SetActive(true);
        }

        if (Input.GetAxisRaw("Cancel") != 0)
            SceneManager.LoadScene(0);

        if (oneIsReady && twoIsReady)
        {
            StartCoroutine(WaitForMatchStart());
        }
    }

    IEnumerator WaitForMatchStart()
    {
        yield return new WaitForSeconds(1);
        if (!GameManager.Gameplay.GameHasStarted)
        {
            GameManager.Gameplay.StartMatch();
            GameManager.UI.CloseWaitScreen();

            GetComponent<WaitForPlayersScreenController>().enabled = false;
        }
    }
}
