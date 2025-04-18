using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private float cooldownTimer;
    //[SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Weapon weapon;
    public Weapon Weapon { get { return weapon; } set { weapon = value; } }

    public string ammoDisplay;
    //public string AmmoDisplay => ammoDisplay;


    private void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    public void RefreshAmmoDisplay()
    {
        if (weapon.HasAmmo)
            ammoDisplay = $"Ammo: {weapon.CurrentAmmo}/{weapon.MaxAmmo}";
        else
            ammoDisplay = "";
    }

    public bool Shoot()
    {
        if (cooldownTimer < weapon.GetCooldown())
            return false;

        if (weapon.HasAmmo)
        {
            if (!UseAmmo())
                return false; 
        }

        GameObject bullet = Instantiate(weapon.GetBullet(), weapon.GetBarrelEnd().position, weapon.GetBarrelEnd().rotation, null);
        bullet.GetComponent<Projectile>().ShootBullet(weapon.GetBarrelEnd());

        cooldownTimer = 0;
        RefreshAmmoDisplay();
        return true;
    }

    public void Reload()
    {
        weapon.CurrentAmmo = weapon.MaxAmmo;
        RefreshAmmoDisplay();
    }

    public bool UseAmmo()
    {
        if (weapon.CurrentAmmo > 0)
        {
            weapon.CurrentAmmo--;
            return true;
        }
        else
        {
            return false;
        }
    }



}
