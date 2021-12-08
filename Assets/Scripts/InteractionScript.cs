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
                gameControllerScript.levelObstacles[isObstacle][duplicateId + instanceTag] = !gameControllerScript.levelObstacles[isObstacle][duplicateId + instanceTag];
                gameControllerScript.Activate(instanceTag);
                return 1;
            default:
                return 0;
        }
    }
}
