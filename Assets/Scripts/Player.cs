using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Animator playerAnimator;
    private Vector3 moveDir;
    //private Aim aim;
    Rigidbody2D rb;

    private void Start()
    {
        Application.targetFrameRate = 30;
        //aim = GetComponent<Aim>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = gameInput.GetMovementNormalVector();
        moveDir = new Vector3(inputVector.x, inputVector.y);

        //transform.position += moveDir * Time.deltaTime * moveSpeed;
        rb.linearVelocity = moveDir * moveSpeed * Time.fixedDeltaTime;
        //rb.MovePosition(rb.transform.position + moveDir * moveSpeed * Time.deltaTime);
    }
    //private void Update()
    //{
    //    Vector2 inputVector = gameInput.GetMovementNormalVector();
    //    Vector3 moveDir = new Vector3(inputVector.x, inputVector.y);

    //    //transform.position += moveDir * Time.deltaTime * moveSpeed;
    //    //rb.linearVelocity = moveDir * moveSpeed*Time.deltaTime;
    //    //rb.MovePosition(rb.transform.position + moveDir *moveSpeed*Time.deltaTime);


    //    Debug.Log(inputVector);

    //}
}
