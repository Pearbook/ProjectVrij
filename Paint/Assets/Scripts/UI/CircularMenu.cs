using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CircularMenu : MonoBehaviour
{
    public List<MenuButton> Buttons = new List<MenuButton>();
    private Vector2 mousePosition;
    private Vector2 fromVector2M = new Vector2(0.5f, 1.0f);
    private Vector2 centerCircle = new Vector2(0.5f, 0.5f);
    private Vector2 toVector2M;

    public int MenuItems;
    public int CurrentMenuItem;
    private int OldMenuItem;

    public bool isOpen;

    public CanvasGroup MyGroup;
    public bool UseController;

    private void Start()
    {
        MenuItems = Buttons.Count;

        foreach(MenuButton button in Buttons)
        {
            button.sceneImage.color = button.NormalColor;
        }

        CurrentMenuItem = 0;
        OldMenuItem = 0;
    }

    private void Update()
    {
        if (isOpen)
        {
            GetCurrentMenuItem();

            if (Input.GetButtonDown("Fire1"))
                ButtonAction();
        }
    }

    public void OpenMenu()
    {
        if (!isOpen)
        {
            isOpen = true;
            MyGroup.alpha = 1;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            PlayerManager.Player.DisablePlayer();

        }
        else
        {
            isOpen = false;
            MyGroup.alpha = 0;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            PlayerManager.Player.EnablePlayer();
        }

    }

    public void GetCurrentMenuItem()
    {
        float angle = 0;

        if (UseController)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            if (x != 0.0f || y != 0.0f)
            {
                angle = (Mathf.Atan2(x, y) * Mathf.Rad2Deg); // Do something with the angle here.
            }
        }
        else
        { 
            mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            toVector2M = new Vector2(mousePosition.x/Screen.width, mousePosition.y/Screen.height);

            angle = (Mathf.Atan2(fromVector2M.y - centerCircle.y, fromVector2M.x - centerCircle.x) - Mathf.Atan2(toVector2M.y - centerCircle.y, toVector2M.x - centerCircle.x)) * Mathf.Rad2Deg;
        }

        if (angle < 0)
            angle += 360;

        CurrentMenuItem = (int)(angle / (360 / MenuItems));

        if (CurrentMenuItem != OldMenuItem)
        {
            if(OldMenuItem < Buttons.Count)
                Buttons[OldMenuItem].sceneImage.color = Buttons[OldMenuItem].NormalColor;

            OldMenuItem = CurrentMenuItem;

            if(CurrentMenuItem < Buttons.Count)
                Buttons[CurrentMenuItem].sceneImage.color = Buttons[CurrentMenuItem].HighlightColor;
        }
    }

    public void ButtonAction()
    {
        Buttons[CurrentMenuItem].sceneImage.color = Buttons[CurrentMenuItem].PressColor;

        if(Buttons[CurrentMenuItem].OnClicked != null)
            Buttons[CurrentMenuItem].OnClicked.Invoke();

        OpenMenu();

        /*
        if (CurrentMenuItem == 0)
            print("BUTTON 1");*/
    }
}

[System.Serializable]
public class MenuButton
{
    public string name;
    public Image sceneImage;
    public Color NormalColor = Color.white;
    public Color HighlightColor = Color.grey;
    public Color PressColor = Color.gray;
    public UnityEvent OnClicked;
}