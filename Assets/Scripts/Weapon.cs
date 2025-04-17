using UnityEngine;

public class Weapon : MonoBehaviour
{
    
    [SerializeField] private float cooldown = 0.3f;
    public float GetCooldown() { return cooldown; }
    public void SetCooldown(float f) { cooldown= f; }

    [SerializeField] private Transform barrelEnd;
    public Transform GetBarrelEnd() {  return barrelEnd; }

    [SerializeField] private GameObject bullet;
    public GameObject GetBullet() { return bullet; }

    [SerializeField] private bool automatic;
    public bool Automatic => automatic;

}
