using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private float moveSpeed = 2.0f;
    private Rigidbody2D rb;
    private Transform playerTransform;
    private EnemyAnimation enemyAnimation;
    [SerializeField] private GameObject WeaponObject;
    private GameObject currentWeaponInstance;
    //private Weapon weapon;
    private WeaponController wc;
    [SerializeField] private GameObject groundObjectPrefab;
    private bool dead = false;
    public Transform PlayerTransfrom => playerTransform;
    System.Random random = new System.Random();
    private void Awake()
    {
       
        enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindWithTag("Player")?.transform;
    }
    private void Start()
    {

        wc = GetComponentInChildren<WeaponController>();
        enemyAnimation.wc = wc;
        currentWeaponInstance = Instantiate(wc.WeaponObject, transform);
        wc.Weapon = currentWeaponInstance.GetComponent<Weapon>();
        StartCoroutine(Reload());
    }

    private void MoveTowardsPlayer()
    {
        Vector2 moveDir = (playerTransform.position - transform.position).normalized;
        rb.linearVelocity = moveDir * moveSpeed * Time.fixedDeltaTime;
    }

    public void RecieveDamage(int damage)
    {
        health -= damage;
        if(health <= 0 && !dead) 
        {
            dead = true;
            rb.simulated = false;
            GetComponentInChildren<SpriteRenderer>().sortingLayerName = "WalkInFront";
            enemyAnimation.OnDeath();
            DropWeapon();
            Destroy(this);
        }
    }

    private void DropWeapon()
    {
        if (groundObjectPrefab == null)
            return;

        GameObject drop = Instantiate(groundObjectPrefab, transform.position,Quaternion.identity);
        GroundObject groundObj = drop.GetComponent<GroundObject>();

        if (groundObj == null)
            return;

        groundObj.SetWeapon(wc.Weapon.CurrentAmmo);
    }

    public void Shoot()
    {
        //Debug.Log("AmmoCount in EnemyBNase: "+ wc.Weapon.CurrentAmmo);
        if (currentWeaponInstance != null)
        {
            if(wc.Shoot())
                enemyAnimation.OnShoot();
        }
    }
    // 10 másodpercenként újratölt
    private IEnumerator Reload()
    {
        for(; ; )
        {
            wc.Reload(wc.Weapon.MagazineAmmo);
            yield return new WaitForSeconds(10);
        }
    }

    private void FixedUpdate()
    {
        float distance = (transform.position - playerTransform.position).magnitude;
        MoveTowardsPlayer();
        if (distance <  5 || dead)
        {
            rb.linearVelocity = Vector2.zero;
        }
        Shoot();

    }
}
