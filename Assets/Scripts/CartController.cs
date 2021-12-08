using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartController : MonoBehaviour
{
    public float speed = 1f;
    public bool isCartMoving = false;
    private float tolerance;
    public Vector3 destination;
    private Vector3 position;
    public static float distanceTravelled = 0;

    // Start is called before the first frame update
    void Start()
    {
        tolerance = speed * Time.deltaTime;
        destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (destination != transform.position)
        {
            isCartMoving = true;
            MoveTo();
        }
        else isCartMoving = false;
    }
    private void MoveTo()
    {
        position = transform.position;
        Vector3 heading = destination - transform.position;
        transform.position += (heading / heading.magnitude) * speed * Time.deltaTime;
        if (heading.magnitude < tolerance) { transform.position = destination; print(Mathf.Round(distanceTravelled) + " Meters travelled in total!"); }
        distanceTravelled += Vector3.Distance(position, transform.position);
    }
}
