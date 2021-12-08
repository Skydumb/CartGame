using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    //Interaction script must be implemented by all game objects tagged as "Interactible"
    public int interactibleId;
    public string instanceTag;
    private GameController gameControllerScript;
    public int isObstacle;
    public int duplicateId;
    public bool state = false;

    // Start is called before the first frame update
    void Start()
    {
        gameControllerScript = GameObject.Find("GameController").GetComponent("GameController") as GameController;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Interact()
    {
        switch (interactibleId)
        {
            case 0:

                return 0;
            case 1:
                //Lever
                GameController.levelObstacles[isObstacle][duplicateId + instanceTag] = state;
                if (state) gameControllerScript.PlaySound(0, 0.8f);
                else gameControllerScript.PlaySound(0);
                state = !state;
                gameControllerScript.Activate(instanceTag, state);
                return 1;
            default:
                return 0;
        }
    }
}
