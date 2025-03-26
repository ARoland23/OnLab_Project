using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator playerAnimator;
    [SerializeField] private GameInput gameInput;
    void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerAnimator.SetBool("running", gameInput.GetMovementNormalVector() != Vector2.zero);
    }
}
