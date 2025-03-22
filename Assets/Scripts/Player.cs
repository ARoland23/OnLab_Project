using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private GameInput gameInput;

    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementNormalVector();
        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y);

        transform.position += moveDir * Time.deltaTime * moveSpeed;
        Debug.Log(inputVector);

    }
}
