using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public void OnXYMove(InputAction.CallbackContext context)
    {
        if (context.performed) // On button press down
        {
            Vector2 value = context.ReadValue<Vector2>();
            if (value == Vector2.up)
            {
                Debug.Log("Forward (+Y) Pressed!");
                // TODO : Publish message
            }
            else if (value == Vector2.down)
            {
                Debug.Log("Backward (-Y) Pressed!");
                // TODO : Publish message
            }
            else if (value == Vector2.left)
            {
                Debug.Log("Left (-X) Pressed!");
                // TODO : Publish message
            }
            else if (value == Vector2.right)
            {
                Debug.Log("Right (+X) Pressed!");
                // TODO : Publish message
            }
        }
    }

    public void OnZWMove(InputAction.CallbackContext context)
    {
        // On button press down
        if (context.performed)
        {
            Vector2 value = context.ReadValue<Vector2>();
            if (value == Vector2.up)
            {
                Debug.Log("Up (+Z) Pressed!");
                // TODO : Publish message
            }
            else if (value == Vector2.down)
            {
                Debug.Log("Down (-Z) Pressed!");
                // TODO : Publish message
            }
            else if (value == Vector2.left)
            {
                Debug.Log("-W Pressed!");
                // TODO : Publish message
            }
            else if (value == Vector2.right)
            {
                Debug.Log("+W Pressed!");
                // TODO : Publish message
            }
        }
    }
}