using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public WeaponController wc;
    private Animator enemyAnimator;
    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
    }
    public void OnShoot()
    {
        enemyAnimator.SetTrigger("shot");
    }
    public void OnShootStart()
    {
        //wc.Shoot();
    }
    public void OnDeath()
    {
        enemyAnimator.SetTrigger("die");
    }
    public void OnDeathComplete()
    {
        if(gameObject != null)
            Destroy(gameObject);
    }

    void Update()
    {
        enemyAnimator.SetBool("running", !GetComponentInParent<Rigidbody2D>().linearVelocity.Equals(Vector2.zero));
    }
}
