using System;
using System.Collections;
using TopDownPlayer;
using Unity.VisualScripting;
using UnityEditor.Purchasing;
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
    [SerializeField] private LayerMask obstacleMask;

    private GameObject currentWeaponInstance;
    //private Weapon weapon;
    private WeaponController wc;
    [SerializeField] private GameObject groundObjectPrefab;
    private bool dead = false;
    //private bool chasing = false;
    public bool canSeePlayer = false;
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
        StartCoroutine(LookForPlayer());
    }

    private void MoveTowardsPlayer(Vector3 playerPosition)
    {
        Vector2 moveDir = (playerPosition - transform.position).normalized;
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
            StopAllCoroutines();
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
    private IEnumerator LookForPlayer()
    {
        Debug.Log("Look For Player started");
        for (; ; )
       {
            Debug.Log("Looking for player");
            Debug.Log($"ObstacleMask value: {obstacleMask.value}");
            Vector2 directionToPlyer = (playerTransform.position - transform.position).normalized;
            float distance = (playerTransform.position - transform.position).magnitude;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlyer, distance, obstacleMask);
            if (hit.collider != null && hit.collider.gameObject.GetComponent<Player>() != null)
            {
                canSeePlayer = true;
                //DrawRaycast(transform.position, directionToPlyer, distance, Color.red);
                MoveTowardsPlayer(playerTransform.position);
            }
            else
            {
                canSeePlayer = false;
                //DrawRaycast(transform.position, directionToPlyer, distance, Color.white);
                rb.linearVelocity = Vector2.zero;
            }
            Debug.Log("Can see player?: "+canSeePlayer);
            yield return new WaitForSeconds(1f);
        }
    }
    private void DrawRaycast(Vector3 origin,Vector2 direction, float distance, Color color)
    {
        Debug.DrawRay(origin, direction * distance, color,1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null)
            return;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null)
            return;
    }

    private void FixedUpdate()
    {
        float distance = (transform.position - playerTransform.position).magnitude;
        if (canSeePlayer)
        {
            if (distance < 5 || dead)
            {
                rb.linearVelocity = Vector2.zero;
            }
            Shoot();
        }
    }
}
