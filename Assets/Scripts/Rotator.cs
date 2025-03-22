using UnityEngine;

public class Rotater : MonoBehaviour
{
    protected void lookAt(Vector3 target)
    {
        Vector3 lookDirection = (target - transform.position).normalized;

        float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        // rotate on Z axis
        transform.eulerAngles = new Vector3(0, 0, lookAngle );
    }

}
