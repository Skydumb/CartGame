using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartController : MonoBehaviour
{
    public float speed = 1f;
    public Transform destination;
    public bool isCartMoving = false;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IMoveTo(Vector3 destination)
    {
        StartCoroutine(MoveTo(destination));
    }
    private IEnumerator MoveTo(Vector3 destination)
    {
        isCartMoving = true;
        int z;
        if(transform.position.z - destination.z < 0) { z = 1; }
        else { z = -1; }
        while (transform.position.z - destination.z > 0.1f )
        {
            transform.Translate(new Vector3(0, 0, z) * speed * Time.deltaTime);
            yield return null;
        }
        isCartMoving = false;
    }
}
