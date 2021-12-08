using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedController : MonoBehaviour
{
    private Vector3 destination;
    private bool state = false;
    private bool active = false;
    public int obstacleDependent = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate(int propIndex)
    {
        switch (propIndex)
        {
            case 0:
                //Gate
                gameObject.SetActive(!gameObject.activeSelf);
                break;
            case 2:
                //Dropping platform
                if (!active)
                {
                    if (state)
                        destination = transform.position - Vector3.down * 7;
                    else
                        destination = transform.position - Vector3.up * 7;
                    state = !state;
                    StartCoroutine(Movement());
                }
                break;
            default:
                break;
        }
    }

    private IEnumerator Movement()
    {
        active = true;
        float speed = 2f;
        float tolerance = speed * Time.deltaTime;
        while (destination != transform.position)
        {
            active = true;
            Vector3 heading = destination - transform.position;
            transform.position += (heading / heading.magnitude) * speed * Time.deltaTime;
            if (heading.magnitude < tolerance) { transform.position = destination; }
            yield return null;
        }
        active = false;
    }
}
