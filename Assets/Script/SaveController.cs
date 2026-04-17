using System.IO;
using UnityEngine;
using Unity.Cinemachine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController; 
    private HotbarController  hotbarController; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindObjectOfType<InventoryController>();
        hotbarController = FindObjectOfType<HotbarController>();
        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            mapBoundary = FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D.name,
            inventoryData = inventoryController.GetInventoryItem(),
            hotbarSaveData = hotbarController.GetHotbarItem()
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
        Debug.Log("Game saved to: " + saveLocation);
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = saveData.playerPosition;

            CinemachineConfiner2D confiner = FindFirstObjectByType<CinemachineConfiner2D>();
            confiner.BoundingShape2D = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();

            inventoryController.SetInventoryItem(saveData.inventoryData);
            hotbarController.SetHotbarItem(saveData.hotbarSaveData);

            Debug.Log("Game loaded from: " + saveLocation);
        }
        else
        {
            SaveGame();
        }
    }
}
