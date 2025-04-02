using UnityEngine;

public class PlayerRotation : Rotater
{

   [SerializeField] private GameInput gameInput;
   [SerializeField] private Aim aim;

    private Vector3 point = new Vector3(0, 0, 0);
    //private void Awake()
    //{
    //    aim = GetComponent<Aim>();
    //}
    private void FixedUpdate()
    {
        //point = gameInput.GetMousePosition();
        point = aim.transform.position;
        lookAt(point);
        Debug.Log(point);
    }
}
