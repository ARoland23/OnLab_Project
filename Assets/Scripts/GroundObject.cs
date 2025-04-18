using UnityEngine;

public class GroundObject : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private Player player;
    [SerializeField] private GameObject healthPrefab;


    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(player != null)
        {
            if(weaponPrefab != null)
            {
                player.EquipWeapon(weaponPrefab);
                Destroy(weaponPrefab);
            }

        }
    }
}
