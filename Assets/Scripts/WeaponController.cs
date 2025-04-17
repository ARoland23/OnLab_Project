using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private float cooldownTimer;
    //[SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Weapon weapon;
    public Weapon Weapon { get { return weapon; } set {  weapon = value; } }


    private void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    public bool Shoot()
    {
            if (cooldownTimer < weapon.GetCooldown())
                return false;

            GameObject bullet = Instantiate(weapon.GetBullet(), weapon.GetBarrelEnd().position, weapon.GetBarrelEnd().rotation, null);
            bullet.GetComponent<Projectile>().ShootBullet(weapon.GetBarrelEnd());

            cooldownTimer = 0;
            return true;
    }
}
