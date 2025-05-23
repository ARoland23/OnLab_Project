using UnityEngine;
using TopDownPlayer;
public class GroundObject : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private GameObject healthPrefab;

    public void SetWeapon(int ammo)
    {
        weaponPrefab.GetComponent<Weapon>().CurrentAmmo = ammo;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if(player != null && weaponPrefab != null)          // weapon pickup
        {
                player.PickupWeapon(weaponPrefab);
                Destroy(gameObject);
        }

        else if (player != null && healthPrefab != null)         // health pickup
        {
            player.PickupHealth(healthPrefab);
            Destroy(gameObject);

        }
    }
}
