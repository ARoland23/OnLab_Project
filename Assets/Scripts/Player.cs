using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private GameObject pistolPrefab;
    [SerializeField] private GameObject riflePrefab;
    [SerializeField] private GameObject knifePrefab;
    private GameObject currentWeaponInstance;
    private Vector3 moveDir;
    //private Aim aim;
    Rigidbody2D rb;

    private bool isFiring = false;

    private void Start()
    {
        Application.targetFrameRate = 30;
        //aim = GetComponent<Aim>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void Shoot(CallbackContext ctx)
    {
        if (weaponController.Weapon == null)
            return;

        if (weaponController.Weapon.Automatic)
        {
            if (ctx.started)
                isFiring = true;
            else if (ctx.canceled)
                isFiring = false;
        }
        else
        {
            if (ctx.performed)
            {
                bool shootSuccess = weaponController.Shoot();
                if (shootSuccess)
                    playerAnimation.OnShoot(); 
            }
        }
    }
    public void EquipWeapon(GameObject weaponPrefab)
    {
        if(currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance);
        }

        currentWeaponInstance  = Instantiate(weaponPrefab,transform);
        Weapon weaponComponent = currentWeaponInstance.GetComponent<Weapon>();
        weaponController.Weapon = weaponComponent;
    }

    public void OnSwitchToPistol(CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        EquipWeapon(pistolPrefab);
        playerAnimation.SwitchToPistol();
        Debug.Log("Switched to Pistol!");
    }

    public void OnSwitchToRifle(CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        EquipWeapon(riflePrefab);
        playerAnimation.SwitchToRifle();
        Debug.Log("Switched to Rifle!");
    }
    public void OnSwitchToKnife(CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        EquipWeapon(knifePrefab);
        playerAnimation.SwitchToKnife();
        Debug.Log("Switched to Knife!");
    }
    private void Update()
    {
        if (weaponController.Weapon.Automatic && isFiring)
        {
            bool shootSuccess = weaponController.Shoot();
            if (shootSuccess)
                playerAnimation.OnShoot();
        }
    }
    private void FixedUpdate()
    {
        Vector2 inputVector = gameInput.GetMovementNormalVector();
        moveDir = new Vector3(inputVector.x, inputVector.y);

        rb.linearVelocity = moveDir * moveSpeed * Time.fixedDeltaTime;
    }

}
