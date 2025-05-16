using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace TopDownPlayer
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 7.0f;
        [SerializeField] private int health = 100;
        [SerializeField] private GameInput gameInput;
        [SerializeField] private PlayerAnimation playerAnimation;
        [SerializeField] private WeaponController weaponController;
        //[SerializeField] private GameObject pistolPrefab;
        //[SerializeField] private GameObject riflePrefab;
        [SerializeField] private GameObject knifePrefab;
        private Dictionary<string,PlayerWeapon> weaponInventory = new Dictionary<string, PlayerWeapon> ();
        private bool dead = false;
        private GameObject currentWeaponInstance;
        private Vector3 moveDir;
        Rigidbody2D rb;

        private bool isFiring = false;
        public string ammoDisplay;

        private void Start()
        {
            Application.targetFrameRate = 30;
            rb = GetComponent<Rigidbody2D>();
            rb.simulated = true;
            EquipWeapon(knifePrefab);
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
                    {
                        playerAnimation.OnShoot();
                        RefreshAmmoDisplay();
                    }

                }
            }
        }
        public void EquipWeapon(GameObject weaponPrefab)
        {
            string weaponName = weaponPrefab.GetComponent<Weapon>().Name;

            if (currentWeaponInstance != null)
            {
                Weapon current = currentWeaponInstance.GetComponent<Weapon>();
                if (weaponInventory.ContainsKey(current.Name))
                {
                    // Save current weapon state before switching
                    weaponInventory[current.Name].totalAmmo += weaponController.Weapon.CurrentAmmo;
                }
                Destroy(currentWeaponInstance);
            }

            if (weaponInventory.ContainsKey(weaponName))
            {
                currentWeaponInstance = Instantiate(weaponInventory[weaponName].weaponPrefab, transform);
                Weapon weaponComponent = currentWeaponInstance.GetComponent<Weapon>();
                //there is enough the total ammo
                if (weaponInventory[weaponName].totalAmmo >= weaponComponent.MagazineAmmo)
                {
                    weaponComponent.CurrentAmmo = weaponComponent.MagazineAmmo;
                    weaponInventory[weaponName].totalAmmo-= weaponComponent.MagazineAmmo;
                }
                else // not enough total ammo
                {
                    weaponComponent.CurrentAmmo = weaponInventory[weaponName].totalAmmo;
                    weaponInventory[weaponName].totalAmmo = 0;
                }
            }
            else
            {
                // first time pickup

                currentWeaponInstance = Instantiate(weaponPrefab, transform);
                Weapon weapon = currentWeaponInstance.GetComponent<Weapon>();
                weaponInventory[weaponName] = new PlayerWeapon(weaponPrefab, weapon.CurrentAmmo);
            }
            weaponController.Weapon = currentWeaponInstance.GetComponent<Weapon>();

            //currentWeaponInstance = Instantiate(weaponPrefab, transform);
            //Weapon weaponComponent = currentWeaponInstance.GetComponent<Weapon>();
            //weaponController.Weapon = weaponComponent;

            switch (weaponName)
            {
                case ("knife"):
                    playerAnimation.SwitchToKnife();
                    break;
                case ("pistol"):
                    playerAnimation.SwitchToPistol();
                    break;
                case ("rifle"):
                    playerAnimation.SwitchToRifle();
                    break;
                default:
                    playerAnimation.SwitchToKnife();
                    break;
            }
            // weaponController.RefreshAmmoDisplay();
            RefreshAmmoDisplay();
        }

        public void OnSwitchToPistol(CallbackContext ctx)
        {
            if (!ctx.performed)
                return;

            if (!weaponInventory.ContainsKey("pistol"))
                return;
            EquipWeapon(weaponInventory["pistol"].weaponPrefab);
            // weaponController.RefreshAmmoDisplay();
            RefreshAmmoDisplay();
            Debug.Log("Switched to Pistol!");
        }

        public void OnSwitchToRifle(CallbackContext ctx)
        {
            if (!ctx.performed)
                return;

            if (!weaponInventory.ContainsKey("rifle"))
                return;
            EquipWeapon(weaponInventory["rifle"].weaponPrefab);
            //  weaponController.RefreshAmmoDisplay();
            RefreshAmmoDisplay();
            Debug.Log("Switched to Rifle!");
        }
        public void OnSwitchToKnife(CallbackContext ctx)
        {
            if (!ctx.performed)
                return;

            if (!weaponInventory.ContainsKey("knife"))
                return;
            EquipWeapon(weaponInventory["knife"].weaponPrefab);
            //  weaponController.RefreshAmmoDisplay();
           RefreshAmmoDisplay();
            Debug.Log("Switched to Knife!");
        }

        public void Reload(CallbackContext ctx)
        {
            if (!ctx.performed)
                return;
            Weapon weaponComponent = currentWeaponInstance.GetComponent<Weapon>();
            //there is enough the total ammo
            if (weaponInventory[weaponComponent.Name].totalAmmo >= weaponComponent.MagazineAmmo)
            {
                weaponComponent.CurrentAmmo = weaponComponent.MagazineAmmo;
                weaponController.Reload(weaponComponent.MagazineAmmo);
                weaponInventory[weaponComponent.Name].totalAmmo -= weaponComponent.MagazineAmmo;
            }
            else // not enough total ammo
            {
                weaponComponent.CurrentAmmo = weaponInventory[weaponComponent.Name].totalAmmo;
                weaponController.Reload(weaponInventory[weaponComponent.Name].totalAmmo);
                weaponInventory[weaponComponent.Name].totalAmmo = 0;
            }

            //weaponController.Reload();
            RefreshAmmoDisplay();
        }
        public void RefreshAmmoDisplay()
        {
            Weapon weapon = currentWeaponInstance.GetComponent<Weapon>();
            if (weapon.HasAmmo)
                ammoDisplay = $"Ammo: {weapon.CurrentAmmo}/{weaponInventory[weapon.Name].totalAmmo}";
            else
                ammoDisplay = "";
        }

        public void PickupHealth(GameObject healthPrefab)
        {
            Health hpPrefab = healthPrefab.GetComponent<Health>();
            health += hpPrefab.HealthPoint;
            if (health > 100)
                health = 100;
        }
        public void RecieveDamage(int damage)
        {
            health -= damage;
            if (health <= 0 && !dead)
            {
                dead = true;
                rb.simulated = false;
                GetComponentInChildren<SpriteRenderer>().sortingLayerName = "WalkInFront";
                playerAnimation.OnDeath();
                //Destroy(this);
            }
        }

        private void Update()
        {
            if (weaponController.Weapon.Automatic && isFiring)
            {
                bool shootSuccess = weaponController.Shoot();
                if (shootSuccess)
                {
                    playerAnimation.OnShoot();
                    RefreshAmmoDisplay();
                }

            }
        }
        private void FixedUpdate()
        {
            Vector2 inputVector = gameInput.GetMovementNormalVector();
            moveDir = new Vector3(inputVector.x, inputVector.y);

            rb.linearVelocity = moveDir * moveSpeed * Time.fixedDeltaTime;
        }

    }

}

