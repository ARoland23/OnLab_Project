using UnityEngine;

public class Rotater : MonoBehaviour
{
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    protected void lookAt(Vector3 target)
    {
        Vector3 lookDirection = (target - transform.position).normalized;

        float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        rb.SetRotation(lookAngle);
    }

}
