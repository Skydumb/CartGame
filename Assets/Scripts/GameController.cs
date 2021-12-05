using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<GameObject> worldPrefabs = new List<GameObject>();
    public List<GameObject> currentPrefabs = new List<GameObject>();
    public Dictionary<string, GameObject> Propnames = new Dictionary<string, GameObject>();
    public Dictionary<string, int> propIndexes = new Dictionary<string, int>();
    List<PropPreset> propPresets = new List<PropPreset>() { new PropPreset(0, new Vector3(1, 2, -8), "Tutorial_Gate"), new PropPreset(1, new Vector3(0, 1, -10), "Tutorial_Gate", new Quaternion(), 1) };
    private CartController cartController;
    public Vector3[] cartDestinations = new Vector3[] { new Vector3(1.5f, 1.5f, 3f), new Vector3(1.5f, 1.5f, 20f) };
    public List<Dictionary<string, bool>> levelObstacles = new List<Dictionary<string, bool>>() { new Dictionary<string, bool>(), new Dictionary<string, bool>() };
    public int currentCDest = 1;
    // Start is called before the first frame update
    void Start()
    {
        cartController = GameObject.Find("Cart").GetComponent<CartController>();
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
        if (currentPrefabs[currentPrefabs.Count - 1].TryGetComponent(out InteractionScript interaction))
        {
            interaction.instanceTag = preset.name;
            interaction.isObstacle = preset.isObstacle;
            if (interaction.isObstacle > 0) { levelObstacles[interaction.isObstacle].Add(preset.name, true); }
            Propnames.Add("i" + preset.name, currentPrefabs[currentPrefabs.Count - 1]);
            propIndexes.Add("i" + preset.name, preset.prefabIndex);
        }
        else
        {
            Propnames.Add(preset.name, currentPrefabs[currentPrefabs.Count - 1]);
            propIndexes.Add(preset.name, preset.prefabIndex);
        }
    }
    /// <summary>
    /// Calls the activate function of the targeted instantiated prefab.
    /// </summary>
    /// <param name="targetProp"></param>
    public void Activate(string targetProp)
    {
        Propnames[targetProp].GetComponent<ActivatedController>().Activate(propIndexes[targetProp]);
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
                    print(cartDestinations[currentCDest - 1]);
                    cartController.IMoveTo(cartDestinations[currentCDest - 1]);
                }
            }
            else if (!levelObstacles[currentCDest].ContainsValue(true))
            {
                print(cartDestinations[currentCDest - 1]);
                cartController.IMoveTo(cartDestinations[currentCDest - 1]);
            }
        }
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
    public PropPreset(int PrefabIndex, Vector3 Position, string Name, Quaternion Rotation = new Quaternion(), int IsObstacle = 0)
    {
        position = Position;
        name = Name;
        prefabIndex = PrefabIndex;
        isObstacle = IsObstacle;
    }
}