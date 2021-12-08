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
    /// <summary>
    /// Makes the object move in and look toward one of eight directions on a two dimensional plane(x, z)
    /// </summary>
    private void Move()
    {
        Vector2 direction = new Vector2(0, 0);
        if (Input.anyKey)
        {
            direction = KeyDirection();
        }
        Vector3 threeDimDirection = new Vector3(direction.y, 0, direction.x);
        transform.Translate(threeDimDirection * speed * Time.deltaTime, Space.World);
        if (direction != new Vector2(0,0))
        {
            Quaternion lookRotation = Quaternion.LookRotation(threeDimDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
        if (Input.GetKeyDown(KeyCode.Space) && interactibles.Count > 0)
        {
            Interact();
        }
    }
    /// <summary>
    /// Returns a vector2 based on wich of the WASD keys are pressed
    /// </summary>
    /// <returns></returns>
    private Vector2 KeyDirection()
    {
        Vector2 vector2 = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            vector2.y++;
        }
        if (Input.GetKey(KeyCode.A))
        {
            vector2.x++;
        }
        if (Input.GetKey(KeyCode.S))
        {
            vector2.y--;
        }
        if (Input.GetKey(KeyCode.D))
        {
            vector2.x--;
        }
        vector2.Normalize();
        return vector2;
    }
    /// <summary>
    /// Makes the object follow and align with the cart object
    /// </summary>
    private void FollowCart()
    {
        transform.position = cart.transform.position + cartRideOffset;
        transform.eulerAngles = cart.transform.eulerAngles + new Vector3(0, 180);
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
                transform.Translate(new Vector3(0, -1.5f, 1));
                moveType = "free";
            }
        }
    }
    /// <summary>
    /// Selects an object to trigger an "interaction" with
    /// </summary>
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
    // Adds or removes a gameobject to the interactibles list
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
