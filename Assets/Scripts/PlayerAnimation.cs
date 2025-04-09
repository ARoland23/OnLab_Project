using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class PlayerAnimation : MonoBehaviour
{
    private Animator playerAnimator;
    [SerializeField] private GameInput gameInput;

    public void OnSwitchToKnife(CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        playerAnimator.SetTrigger("switchedToKnife");
        Debug.Log("Switched to Knife!");
    }
   public void OnSwitchToPistol(CallbackContext ctx)
   {
        if (!ctx.performed)
            return;

        playerAnimator.SetTrigger("switchedToPistol");
        Debug.Log("Switched to Pistol!");
   }

    public void OnShoot(/*CallbackContext ctx*/) 
    {
        //if (!ctx.performed)
        //    return;


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
