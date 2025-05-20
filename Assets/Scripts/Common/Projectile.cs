using TopDownPlayer;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private int damage;
    private GameObject shotBy;
    public GameObject ShotBy { get => shotBy; set => shotBy = value;  }
    public int Damage { get { return damage; } }

    private float timer;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ShootBullet(Transform barrelEnd)
    {
        timer = 0;
        rb.linearVelocity = Vector2.zero;
        transform.position = barrelEnd.position;
        transform.rotation = barrelEnd.rotation;

        gameObject.SetActive(true);

        rb.AddForce(-transform.up * speed, ForceMode2D.Impulse);
    }
    // tölténynek
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == shotBy)
            return;

        EnemyBase eb = collision.gameObject.GetComponent<EnemyBase>();
        Player player = collision.gameObject.GetComponent<Player>();
        if (eb != null)
        {
            eb.RecieveDamage(damage);
            Destroy(gameObject);
        }else if(player != null)
        {
            player.RecieveDamage(damage);
            Destroy(gameObject);
        }
        
    }
    // késnek
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == shotBy)
            return;

        EnemyBase eb = collision.gameObject.GetComponent<EnemyBase>();
        if (eb != null)
        {
            eb.RecieveDamage(damage);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
            gameObject.SetActive(false);
    }
}
