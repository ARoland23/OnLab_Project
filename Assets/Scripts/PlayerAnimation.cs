using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class PlayerAnimation : MonoBehaviour
{
    private Animator playerAnimator;
    [SerializeField] private GameInput gameInput;

    //public void SetFiring(bool firing) => playerAnimator.SetBool("firing", firing);

    public void SwitchToKnife()
    {
        playerAnimator.SetTrigger("switchedToKnife");
    }
   public void SwitchToPistol()
   {
        playerAnimator.SetTrigger("switchedToPistol");
   }
    public void SwitchToRifle()
    {
        playerAnimator.SetTrigger("switchedToRifle");
    }

    public void OnShoot() 
    {
        playerAnimator.SetTrigger("shot");
        Debug.Log("Used weapon!");
    }

    void Awake()
    {
        playerAnimator = GetComponentInParent<Animator>();
    }

    void Update()
    {
        playerAnimator.SetBool("running", gameInput.GetMovementNormalVector() != Vector2.zero);
    }
}
