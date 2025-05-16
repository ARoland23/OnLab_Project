using UnityEngine;

public class PlayerWeapon
{
    public GameObject weaponPrefab;
    public int totalAmmo = 0;

    public PlayerWeapon(GameObject weaponPrefab, int totalAmmo)
    {
        this.weaponPrefab = weaponPrefab;
        this.totalAmmo = totalAmmo;
    }
}
