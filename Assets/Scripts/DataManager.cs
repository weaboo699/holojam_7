using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public List<int> inventoryIDs = new List<int>();
    public int currentIndex;
}

[System.Serializable]
public class RoomSaveData
{
    public List<int> displayedSlots = new List<int>();
}
public class DataManager : MonoBehaviour
{
    public static DataManager Instance {get; private set;}
    
    private const string GAME_SAVE_KEY = "GameSaveData";
    private const string ROOM_SAVE_KEY = "RoomSaveData";
    
    void Awake()
    {
        Instance = this;
    }
    
    public void SaveGameData(int[] inventoryIDs, int currentIndex)
    {
        // 載入舊資料
        GameSaveData existingData = LoadGameData();
        
        // 追加新物品
        for(int i = 0; i < currentIndex; i++)
        {
            existingData.inventoryIDs.Add(inventoryIDs[i]);
        }
        existingData.currentIndex = existingData.inventoryIDs.Count;
        
        string json = JsonUtility.ToJson(existingData);
        PlayerPrefs.SetString(GAME_SAVE_KEY, json);
        PlayerPrefs.Save();
        
        Debug.Log("Game data saved: " + json);
    }
    
    public GameSaveData LoadGameData()
    {
        if (PlayerPrefs.HasKey(GAME_SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(GAME_SAVE_KEY);
            return JsonUtility.FromJson<GameSaveData>(json);
        }
        
        return new GameSaveData();
    }
    
    public void SaveRoomData(List<int> inventoryIDs, int[] displayedSlots)
    {
        // 保存完整的資料
        GameSaveData gameData = new GameSaveData();
        gameData.inventoryIDs = inventoryIDs;
        gameData.currentIndex = inventoryIDs.Count;
        
        RoomSaveData roomData = new RoomSaveData();
        roomData.displayedSlots = new List<int>(displayedSlots);
        
        string gameJson = JsonUtility.ToJson(gameData);
        string roomJson = JsonUtility.ToJson(roomData);
        
        PlayerPrefs.SetString(GAME_SAVE_KEY, gameJson);
        PlayerPrefs.SetString(ROOM_SAVE_KEY, roomJson);
        PlayerPrefs.Save();
        
        Debug.Log("Room data saved - Inventory: " + gameJson);
        Debug.Log("Room data saved - Display: " + roomJson);
    }
    
    public RoomSaveData LoadRoomData()
    {
        if (PlayerPrefs.HasKey(ROOM_SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(ROOM_SAVE_KEY);
            return JsonUtility.FromJson<RoomSaveData>(json);
        }
        
        return new RoomSaveData();
    }
}
