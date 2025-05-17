using UnityEngine;

public class EnemyRotation : Rotater
{
    [SerializeField] private EnemyBase eb;
    private Transform target;
    private void Start ()
    {
        eb = GetComponentInParent<EnemyBase>();
        target = eb.PlayerTransfrom;
    }
    private void FixedUpdate()
    {
     if(eb.canSeePlayer)   
        lookAt(target.position);
    }
}
