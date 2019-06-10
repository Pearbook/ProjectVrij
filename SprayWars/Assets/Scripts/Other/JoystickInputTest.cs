using UnityEngine;

public class JoystickInputTest : MonoBehaviour
{
    private KeyCode[] keycodes = new KeyCode[] {
        KeyCode.JoystickButton0,
        KeyCode.JoystickButton1,
        KeyCode.JoystickButton2,
        KeyCode.JoystickButton3,
        KeyCode.JoystickButton4,
        KeyCode.JoystickButton5,
        KeyCode.JoystickButton6,
        KeyCode.JoystickButton7,
        KeyCode.JoystickButton8,
        KeyCode.JoystickButton9,
        KeyCode.JoystickButton10,
        KeyCode.JoystickButton11,
        KeyCode.JoystickButton12,
        KeyCode.JoystickButton13,
        KeyCode.JoystickButton14,
        KeyCode.JoystickButton15,
        KeyCode.JoystickButton16,
        KeyCode.JoystickButton17,
        KeyCode.JoystickButton18,
        KeyCode.JoystickButton19
        };

    void Start()
    {
        Debug.Log("Catching " + keycodes.Length + " input from joystick");
    }

    void Update()
    {
        foreach (KeyCode kc in keycodes)
        {
            if (Input.GetKeyDown(kc))
            {
                Debug.Log("Detected: " + kc.ToString());
            }
        }
    }
}
