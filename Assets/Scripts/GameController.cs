using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<GameObject> worldPrefabs = new List<GameObject>();
    public List<GameObject> currentPrefabs = new List<GameObject>();
    public Dictionary<string, GameObject> propNames = new Dictionary<string, GameObject>();
    public Dictionary<string, int> propIndexes = new Dictionary<string, int>();
    List<PropPreset> propPresets = new List<PropPreset>() 
    { new PropPreset(0, new Vector3(1, 2, -8), "Tutorial_Gate"), new PropPreset(1, new Vector3(0, 1, -10), "Tutorial_Gate", 1), new PropPreset(2, new Vector3(1.5f, 7.25f, -33), "Tutorial_Drawbridge", 3), new PropPreset(1, new Vector3(0, 1, -29), "Tutorial_Drawbridge", 3),
    new PropPreset(1, new Vector3(3, 1, -29), "Tutorial_Drawbridge", 3) };
    private CartController cartController;
    public Vector3[] cartDestinations = new Vector3[] { new Vector3(1.5f, 1.5f, 3f), new Vector3(1.5f, 1.5f, 20f) };
    public static List<Dictionary<string, bool>> levelObstacles = new List<Dictionary<string, bool>>() { };
    public AudioClip[] audioClips = new AudioClip[0];
    public int currentCDest = 1;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cartController = GameObject.Find("Cart").GetComponent<CartController>();
        for (int i = 0; i < cartDestinations.Length; i++)
        {
            levelObstacles.Add(new Dictionary<string, bool>());
        }
        for (int i = 0; i < propPresets.Count; i++)
        {
            AddToScene(propPresets[i]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddToScene(PropPreset preset)
    {
        currentPrefabs.Add(Instantiate(worldPrefabs[preset.prefabIndex], preset.position, worldPrefabs[0].transform.rotation));
        GameObject currentObject = currentPrefabs[currentPrefabs.Count - 1];
        if (currentObject.TryGetComponent(out InteractionScript interaction))
        {
            int i = 0;
            while (propNames.ContainsKey("i" + i + preset.name))
            {
                i++;
            }
            propNames.Add("i" + i + preset.name, currentObject);
            propIndexes.Add("i" + i + preset.name, preset.prefabIndex);
            interaction.duplicateId = i;
            interaction.instanceTag = preset.name;
            interaction.isObstacle = preset.isObstacle;
            if (interaction.isObstacle > 0) { levelObstacles[interaction.isObstacle].Add(interaction.duplicateId + preset.name, true); }
        }
        else if (currentObject.TryGetComponent(out ActivatedController activatedController))
        {
            activatedController.obstacleDependent = preset.isObstacle;
            activatedController.instanceTag = preset.name;
            propNames.Add("a" + preset.name, currentObject);
            propIndexes.Add("a" + preset.name, preset.prefabIndex);
            if (activatedController.obstacleDependent > 0) { levelObstacles[activatedController.obstacleDependent].Add(preset.name, true); }
        }
        else
        {
            propNames.Add(preset.name, currentPrefabs[currentPrefabs.Count - 1]);
            propIndexes.Add(preset.name, preset.prefabIndex);
        }
        currentObject.name += preset.name;
    }
    /// <summary>
    /// Calls the activate function of the targeted instantiated prefabs ActivatedController script.
    /// </summary>
    /// <param name="targetProp"></param>
    public void Activate(string targetProp, bool activate)
    {
        ActivatedController activatedController = propNames["a" + targetProp].GetComponent<ActivatedController>();
        int obstaclesIndex = activatedController.obstacleDependent;
        if (!activate && activatedController.State) activatedController.Activate(propIndexes["a" + targetProp]);
        else 
        {
            activatedController.Blocking = false;
            if (!levelObstacles[obstaclesIndex].ContainsValue(true))
            {
                activatedController.Blocking = true;
                activatedController.Activate(propIndexes["a" + targetProp]);
            }
            else activatedController.Blocking = true;
        }
    }
    public void UpdateCart(int direction)
    {
        if (currentCDest + direction >= 1 && currentCDest + direction <= cartDestinations.Length)
        {
            currentCDest += direction;
            if (System.Math.Sign(direction) == 1)
            {
                if (!levelObstacles[currentCDest - 1].ContainsValue(true))
                {
                    cartController.destination = cartDestinations[currentCDest - 1];
                }
                else currentCDest -= direction;
            }
            else if (!levelObstacles[currentCDest].ContainsValue(true))
            {
                cartController.destination = cartDestinations[currentCDest - 1];
            }
            else currentCDest -= direction;
        }
    }
    public void PlaySound(int soundIndex, float pitch = 1)
    {
        audioSource.clip = audioClips[soundIndex];
        audioSource.pitch = pitch;
        audioSource.Play();
    }
}
/// <summary>
/// Class for presets to be used in prefab placement.
/// </summary>
class PropPreset
{
    public readonly Vector3 position;
    public readonly int prefabIndex;
    public readonly string name;
    public readonly int isObstacle;
    public PropPreset(int PrefabIndex, Vector3 Position, string Name, int IsObstacle = 0)
    {
        position = Position;
        name = Name;
        prefabIndex = PrefabIndex;
        isObstacle = IsObstacle;
    }
}