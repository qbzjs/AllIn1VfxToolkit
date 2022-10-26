using UnityEngine;
using UnityEngine.InputSystem;
using Snake4D;

public class InputManager : CustomMonoBehaviour
{
    protected override void SubscribeToMessageHubEvents() { }

    public void OnXYMove(InputAction.CallbackContext context)
    {
        if (context.performed) // On button press down
        {
            Vector2 value = context.ReadValue<Vector2>();
            if (value == Vector2.up)
            {
                PublishMessageHubEvent<UserInputEvent>(
                    new UserInputEvent(UserInputType.Y_Positive)
                );
            }
            else if (value == Vector2.down)
            {
                PublishMessageHubEvent<UserInputEvent>(
                    new UserInputEvent(UserInputType.Y_Negative)
                );
            }
            else if (value == Vector2.left)
            {
                PublishMessageHubEvent<UserInputEvent>(
                    new UserInputEvent(UserInputType.X_Negative)
                );
            }
            else if (value == Vector2.right)
            {
                PublishMessageHubEvent<UserInputEvent>(
                    new UserInputEvent(UserInputType.X_Positive)
                );
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
                PublishMessageHubEvent<UserInputEvent>(
                    new UserInputEvent(UserInputType.Z_Positive)
                );
            }
            else if (value == Vector2.down)
            {
                PublishMessageHubEvent<UserInputEvent>(
                    new UserInputEvent(UserInputType.Z_Negative)
                );
            }
            else if (value == Vector2.left)
            {
                PublishMessageHubEvent<UserInputEvent>(
                    new UserInputEvent(UserInputType.W_Negative)
                );
            }
            else if (value == Vector2.right)
            {
                PublishMessageHubEvent<UserInputEvent>(
                    new UserInputEvent(UserInputType.W_Positive)
                );
            }
        }
    }
}

public class UserInputEvent
{
    public UserInputType InputType;
    public UserInputEvent(UserInputType inputType)
    {
        InputType = inputType;
    }
}