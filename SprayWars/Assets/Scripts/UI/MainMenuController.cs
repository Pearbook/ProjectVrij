using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public float LoadingTime;
    private bool isWaiting;

    public CanvasGroup MainMenuGroup;
    public CanvasGroup LoadingGroup;

    public Image BarImage;

    private void Awake()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if(isWaiting)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene(1);
        }

        if (Input.GetAxisRaw("Submit") != 0)
        {
            if(!isWaiting)
                ShowLoadingScreen();
        }

        if (Input.GetAxisRaw("Cancel") != 0)
        {
            if(!isWaiting)
                Application.Quit();
        }
    }

    void ShowLoadingScreen()
    {
        isWaiting = true;

        MainMenuGroup.alpha = 0;
        LoadingGroup.alpha = 1;

        StartCoroutine(WaitForGameStart(LoadingTime));
    }

    IEnumerator WaitForGameStart(float time)
    {
        var i = 0.0f;
        var rate = 1.0f / time;

        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            BarImage.fillAmount = i;
            yield return null;
        }
        SceneManager.LoadScene(1);
    }

}
