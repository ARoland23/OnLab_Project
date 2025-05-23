using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            PickupWeapon(knifePrefab);
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
            SaveCurrentAmmo();

            if (weaponInventory.ContainsKey(weaponName))
            {
                currentWeaponInstance = Instantiate(weaponInventory[weaponName].weaponPrefab, transform);
                Weapon weaponComponent = currentWeaponInstance.GetComponent<Weapon>();
                // there is enough total ammo
                if (weaponInventory[weaponName].totalAmmo >= weaponComponent.MagazineAmmo)
                {
                    weaponComponent.CurrentAmmo = weaponComponent.MagazineAmmo;
                    weaponInventory[weaponName].totalAmmo -= weaponComponent.MagazineAmmo;
                }
                else // not enough total ammo
                {
                    weaponComponent.CurrentAmmo = weaponInventory[weaponName].totalAmmo;
                    weaponInventory[weaponName].totalAmmo = 0;
                }
                PlayEquipAnimation(weaponName);
                RefreshAmmoDisplay();
            }
            weaponController.Weapon = currentWeaponInstance.GetComponent<Weapon>();

        }

        private void PlayEquipAnimation(string weaponName)
        {
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
        }

        private void SaveCurrentAmmo()
        {
            if (currentWeaponInstance != null)
            {
                Weapon current = currentWeaponInstance.GetComponent<Weapon>();
                if (weaponInventory.ContainsKey(current.Name))
                {
                    // Save current weapon ammo before switching
                    weaponInventory[current.Name].totalAmmo += weaponController.Weapon.CurrentAmmo;
                }
                Destroy(currentWeaponInstance);
            }
        }

        public void PickupWeapon(GameObject weaponPrefab)
        {
            Weapon weapon = weaponPrefab.GetComponent<Weapon>();
            if (weapon == null)
                return;

            string weaponName = weapon.Name;
            if (weaponInventory.ContainsKey(weaponName))
            {
                weaponInventory[weaponName].totalAmmo += weapon.CurrentAmmo;
            }
            else  // new pickup
            {
                SaveCurrentAmmo();
                weaponInventory[weaponName] = new PlayerWeapon(weaponPrefab, 0);
                currentWeaponInstance = Instantiate(weaponPrefab, transform);
                weaponController.Weapon = currentWeaponInstance.GetComponent<Weapon>();
                weaponController.Weapon.CurrentAmmo = weapon.CurrentAmmo;
                PlayEquipAnimation(weaponName);
            }
            RefreshAmmoDisplay();
        }

        public void OnSwitchToPistol(CallbackContext ctx)
        {
            if (!ctx.performed)
                return;

            if (!weaponInventory.ContainsKey("pistol"))
                return;
            EquipWeapon(weaponInventory["pistol"].weaponPrefab);
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
           RefreshAmmoDisplay();
            Debug.Log("Switched to Knife!");
        }

        public void Reload(CallbackContext ctx)
        {
            if (!ctx.performed)
                return;
            Weapon weaponComponent = currentWeaponInstance.GetComponent<Weapon>();
            int ammoToReload = weaponComponent.MagazineAmmo - weaponComponent.CurrentAmmo;
            //there is enough the total ammo
            if (weaponInventory[weaponComponent.Name].totalAmmo >= ammoToReload)
            {
                
                weaponController.Reload(ammoToReload);
                weaponInventory[weaponComponent.Name].totalAmmo -= ammoToReload;
            }
            else // not enough total ammo
            {
                ammoToReload = weaponInventory[weaponComponent.Name].totalAmmo;
                weaponController.Reload(weaponInventory[weaponComponent.Name].totalAmmo);
                weaponInventory[weaponComponent.Name].totalAmmo = 0;
            }

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
                playerAnimation.OnDeath();

                // not used ammo as pts
                AddAmmoToPoints();
                StartCoroutine(DieAndLoadScene());
            }
        }

        public void AddAmmoToPoints()
        {
            int pointsToAdd = 0;
            foreach (var entry in weaponInventory)
            {
                pointsToAdd += entry.Value.totalAmmo;
            }
            pointsToAdd += weaponController.Weapon.CurrentAmmo;
            GameLogic.AddScore(pointsToAdd);
        }

        private IEnumerator DieAndLoadScene()
        {
            yield return new WaitForSeconds(1f); 
            SceneManager.LoadSceneAsync("GameOverSceneL");
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

