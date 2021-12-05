using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float height = 1;
    public float zoom = 5;
    public float z;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        z = player.transform.position.z;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(-zoom, height, z);
    }
}
