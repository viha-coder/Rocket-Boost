using UnityEngine;
using UnityEngine.InputSystem;

public class QuitApplication : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.escapeKey.isPressed)
        {
            Application.Quit();
            Debug.Log("Application has been closed");
        }
    }
}   