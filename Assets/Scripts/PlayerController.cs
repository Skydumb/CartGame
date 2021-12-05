using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public List<string> interactibles = new List<string>();
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
        float keysDown = 0;
        bool horizontal = false;
        bool depth = false;
        float depthAcc = 0;
        float horizontalAcc = 0;
        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.W)){
                keysDown++;
                depth = true;
                depthAcc = 1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                keysDown++;
                horizontal = true;
                horizontalAcc = 1;
            }
            if (Input.GetKey(KeyCode.A))
            {
                keysDown++;
                horizontal = true;
                horizontalAcc = -1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                keysDown++;
                depth = true;
                depthAcc = -1;
            }
        }
        if (depth && horizontal)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime * 0.7f * depthAcc, Space.World);
            transform.Translate(Vector3.back * speed * Time.deltaTime * 0.7f * horizontalAcc, Space.World);
        }
        else
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime * depthAcc, Space.World);
            transform.Translate(Vector3.back * speed * Time.deltaTime * horizontalAcc, Space.World);
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
        GameObject currentInteractee = GameObject.Find(interactibles[0]);
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
            interactibles.Add(other.name);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactible"))
        {
            interactibles.Remove(other.name);
        }
    }
}
