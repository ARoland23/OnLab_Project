using UnityEngine;

public class GameInput : MonoBehaviour
{
    public PlayerInputActions playerInputActions;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }
    public Vector2 GetMovementNormalVector()
    {
        
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector.Normalize();
        return inputVector;
    }

    public Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint( playerInputActions.Player.Look.ReadValue<Vector2>() );
    }
    private void OnDestroy()
    {
        if (playerInputActions != null)
        {
            playerInputActions.Player.Disable();
        }
    }
}
