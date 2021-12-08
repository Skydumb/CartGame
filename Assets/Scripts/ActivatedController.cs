using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedController : MonoBehaviour
{
    private Vector3 destination;

    public bool State { get; private set; } = false;
    private bool active = false;
    public int obstacleDependent = 0;
    public string instanceTag;
    public bool Blocking { get { return GameController.levelObstacles[obstacleDependent][instanceTag]; } set { GameController.levelObstacles[obstacleDependent][instanceTag] = value; } }
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
                Blocking = gameObject.activeSelf;
                break;
            case 2:
                //Dropping platform
                if (!active)
                {
                    if (State)
                        destination = transform.position - Vector3.down * 7;
                    else
                        destination = transform.position - Vector3.up * 7;
                    State = !State;
                    StartCoroutine(Movement());
                }
                break;
            default:
                Blocking = !Blocking;
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
        Blocking = !State;
        active = false;
    }
}
