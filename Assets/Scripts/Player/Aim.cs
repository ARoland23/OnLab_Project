using UnityEngine;

public class Aim : MonoBehaviour
{
   [SerializeField] private GameInput gameInput;

    void Update()
    {
        transform.position = gameInput.GetMousePosition();
        //Debug.Log(transform.position);
    }
}
