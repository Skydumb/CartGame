using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private Light environmentLight;
    public float dayLenght = 80;
    // Start is called before the first frame update
    void Start()
    {
        environmentLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        float A = (CartController.totalDistanceTravelled % dayLenght) / dayLenght;
        transform.eulerAngles = new Vector3(Mathf.Abs(Mathf.Cos(Mathf.PI * (A)) * 65) - 15, Mathf.Sin(Mathf.PI * 2 * (A)) * -90);
    }
}
