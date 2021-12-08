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
    public static float totalDistanceTravelled = 0;

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
    /// <summary>
    /// Moves the object toward the destination and adds the distance travelled to the totalDistanceTravelled variable
    /// </summary>
    private void MoveTo()
    {
        position = transform.position;
        Vector3 heading = destination - transform.position;
        transform.position += (heading / heading.magnitude) * speed * Time.deltaTime;
        if (heading.magnitude < tolerance) { transform.position = destination; print(Mathf.Round(totalDistanceTravelled) + " Meters travelled in total!"); }
        totalDistanceTravelled += Vector3.Distance(position, transform.position);
    }
}
