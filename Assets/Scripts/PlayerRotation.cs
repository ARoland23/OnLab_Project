using UnityEngine;

public class PlayerRotation : Rotater
{

   [SerializeField] private GameInput gameInput;

    private Vector3 point = new Vector3(0, 0, 0);
    private void Update()
    {
        point = gameInput.GetMousePosition();
        lookAt(point);
    }
}
