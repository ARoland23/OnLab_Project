using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private float cooldownTimer;
    [SerializeField] private Weapon weapon;
    [SerializeField] private bool isPlayer;
    public Weapon Weapon { get { return weapon; } set { weapon = value; } }

    [SerializeField] public GameObject WeaponObject;

    //public string ammoDisplay;
    //public string AmmoDisplay => ammoDisplay;

    private void Awake()
    {
        weapon = WeaponObject.GetComponent<Weapon>();
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    public bool Shoot()
    {
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
        //RefreshAmmoDisplay();
        return true;
    }

    public bool CanShoot()
    {
        if (cooldownTimer < weapon.GetCooldown() || weapon.CurrentAmmo < 0)
            return false;
        else
            return true;
    }

    public void Reload(int ammo)
    {
        weapon.CurrentAmmo += ammo;//weapon.MagazineAmmo;
        if(weapon.CurrentAmmo > weapon.MagazineAmmo)
            weapon.CurrentAmmo = weapon.MagazineAmmo;
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
