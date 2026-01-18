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
        GameSaveData data = new GameSaveData();
        
        // 只存有效的物品
        for(int i = 0; i < currentIndex; i++)
        {
            data.inventoryIDs.Add(inventoryIDs[i]);
        }
        data.currentIndex = currentIndex;
        
        string json = JsonUtility.ToJson(data);
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
    
    public void SaveRoomData(int[] slotIDs)
    {
        RoomSaveData data = new RoomSaveData();
        data.displayedSlots = new List<int>(slotIDs);
        
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(ROOM_SAVE_KEY, json);
        PlayerPrefs.Save();
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
