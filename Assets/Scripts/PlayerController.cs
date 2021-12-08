using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public float rotationSpeed = 45;
    public List<GameObject> interactibles = new List<GameObject>();
    public string moveType = "free";
    GameObject cart;
    public Vector3 cartRideOffset = new Vector3(0, 1, -2);
    GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        cart = GameObject.Find("Cart");
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (moveType)
        {
            case "free":
                Move();
                break;
            case "cart":
                FollowCart();
                break;
            default:
                break;
        }
    }

    private void Move()
    {
        float depthAcc = 0;
        float horizontalAcc = 0;
        float desiredAngle = 0;
        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.W)){
                depthAcc = 1;
                desiredAngle = 90;
            }
            if (Input.GetKey(KeyCode.D))
            {
                horizontalAcc = 1;
                desiredAngle = 180;
            }
            if (Input.GetKey(KeyCode.A))
            {
                horizontalAcc = -1;
                desiredAngle = 0;
            }
            if (Input.GetKey(KeyCode.S))
            {
                depthAcc = -1;
                desiredAngle = -90;
            }
            switch(depthAcc, horizontalAcc)
            {
                case (1f, 1f):
                    desiredAngle = 135;
                    break;
                case (1f, -1f):
                    desiredAngle = 45;
                    break;
                case (-1f, 1f):
                    desiredAngle = -135;
                    break;
                case (-1f, -1f):
                    desiredAngle = -45;
                    break;
                default:
                    break;
            }
        }
        float totAcc = depthAcc * Mathf.Sign(depthAcc) + horizontalAcc * Mathf.Sign(horizontalAcc);
        if (totAcc == 2)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime * 0.7f * depthAcc, Space.World);
            transform.Translate(Vector3.back * speed * Time.deltaTime * 0.7f * horizontalAcc, Space.World);
        }
        else
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime * depthAcc, Space.World);
            transform.Translate(Vector3.back * speed * Time.deltaTime * horizontalAcc, Space.World);
        }
        if (totAcc != 0)
        {
            float yRotation = transform.eulerAngles.y;
            if ((yRotation - desiredAngle) * Mathf.Sign(yRotation - desiredAngle) < rotationSpeed * Time.deltaTime)
                transform.eulerAngles = new Vector3(0, desiredAngle);
            else if (yRotation - desiredAngle > 0)
                transform.eulerAngles = new Vector3(0, yRotation - rotationSpeed * Time.deltaTime);
            else
                transform.eulerAngles = new Vector3(0, yRotation + rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space) && interactibles.Count > 0)
        {
            Interact();
        }
    }
    private void FollowCart()
    {
        transform.position = cart.transform.position + cartRideOffset;
        if (Input.anyKey)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                gameController.UpdateCart(-1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                gameController.UpdateCart(1);
            }
            else if (Input.GetKeyDown(KeyCode.Space) && !cart.GetComponent<CartController>().isCartMoving)
            {
                transform.Translate(new Vector3(0, -1.5f, -1));
                moveType = "free";
            }
        }
    }
    private void Interact()
    {
        GameObject currentInteractee = interactibles[0];
        InteractionScript interactionScript = currentInteractee.GetComponent<InteractionScript>();
        switch (interactionScript.Interact())
        {
            case 0:
                moveType = "cart";
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactible"))
        {
            interactibles.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactible"))
        {
            interactibles.Remove(other.gameObject);
        }
    }
}
