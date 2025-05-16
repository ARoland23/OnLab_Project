using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private float cooldownTimer;
    [SerializeField] private Weapon weapon;
    [SerializeField] private bool isPlayer;
    public Weapon Weapon { get { return weapon; } set { weapon = value; } }

    [SerializeField] public GameObject WeaponObject;

    public string ammoDisplay;
    //public string AmmoDisplay => ammoDisplay;

    private void Awake()
    {
        weapon = WeaponObject.GetComponent<Weapon>();
    }
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
        //Debug.Log("AmmoCount in WeaponController: " + Weapon.CurrentAmmo);
        if(!CanShoot())
            return false;

        if (weapon.HasAmmo)
        {
            if (!UseAmmo())
                return false;
        }

        GameObject bullet = Instantiate(weapon.GetBullet(), weapon.GetBarrelEnd().position, weapon.GetBarrelEnd().rotation, null);
        Projectile projectile = bullet.GetComponent<Projectile>();
        projectile.ShotBy = transform.root.gameObject;
        projectile.ShootBullet(weapon.GetBarrelEnd());
        //bullet.GetComponent<Projectile>().ShootBullet(weapon.GetBarrelEnd());


        cooldownTimer = 0;
        RefreshAmmoDisplay();
        return true;
    }

    public bool CanShoot()
    {
        Debug.Log("Current Ammo: "+weapon.CurrentAmmo);
        if (cooldownTimer < weapon.GetCooldown() || weapon.CurrentAmmo < 0)
            return false;
        else
            return true;
    }

    public void Reload()
    {
        weapon.CurrentAmmo = weapon.MaxAmmo;
        if(isPlayer)
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
