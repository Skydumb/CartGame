using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedController : MonoBehaviour
{
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
                gameObject.SetActive(!gameObject.activeSelf);
                break;
            default:
                break;
        }
    }
}
