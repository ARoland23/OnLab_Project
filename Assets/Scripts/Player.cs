using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private GameInput gameInput;
    Rigidbody2D rb;

    
    private void Start()
    {
            rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = gameInput.GetMovementNormalVector();
        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y);

            //transform.position += moveDir * Time.deltaTime * moveSpeed;
        rb.linearVelocity = moveDir * moveSpeed * Time.fixedDeltaTime;
    }
    //private void Update()
    //{
    //    Vector2 inputVector = gameInput.GetMovementNormalVector();
    //    Vector3 moveDir = new Vector3(inputVector.x, inputVector.y);

    //    //transform.position += moveDir * Time.deltaTime * moveSpeed;
    //    rb.linearVelocity = moveDir * moveSpeed*Time.deltaTime;


    //    Debug.Log(inputVector);

    //}
}
