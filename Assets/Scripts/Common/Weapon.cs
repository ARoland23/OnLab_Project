using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private string name;
    public string Name => name;

    [SerializeField] private float cooldown = 0.3f;
    public float GetCooldown() { return cooldown; }
    public void SetCooldown(float f) { cooldown= f; }

    [SerializeField] private Transform barrelEnd;
    public Transform GetBarrelEnd() {  return barrelEnd; }

    [SerializeField] private GameObject bullet;
    public GameObject GetBullet() { return bullet; }

    [SerializeField] private bool automatic;
    public bool Automatic => automatic;

    [SerializeField] private bool hasAmmo;
    public bool HasAmmo => hasAmmo;

    [SerializeField] private int currentAmmo;
    public int CurrentAmmo { get { return currentAmmo; } set { currentAmmo = value; } }

    [SerializeField] private int maxAmmo;
    public int MaxAmmo { get { return maxAmmo; } }

    private void Awake()
    {
        currentAmmo = maxAmmo;
    }
}
