using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceManager : MonoBehaviour
{
    static InterfaceManager manager;

    public static InterfaceManager UI
    {
        get
        {
            if (manager == null)
            {
                manager = FindObjectOfType<InterfaceManager>();
                if (manager == null)
                {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    manager = obj.AddComponent<InterfaceManager>();
                }
            }
            return manager;
        }
    }

    public CanvasGroup WinScreen;

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OpenUICanvas(CanvasGroup group, bool showCursor)
    {
        group.alpha = 1;
        group.blocksRaycasts = true;
        group.interactable = true;

        if (showCursor)
            ShowCursor();
    }

    public void CloseUICanvas(CanvasGroup group)
    {
        group.alpha = 0;
        group.blocksRaycasts = false;
        group.interactable = false;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
