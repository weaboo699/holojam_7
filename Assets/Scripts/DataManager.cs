using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance {get; private set;}
    void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void GameDataSave()
    {
        string oldItems = PlayerPrefs.GetString("SavedItems", "");
        string itemsString = "";
        for(int i = 0; i < GameManager.Instance.CurrentIndex; i++)
        {
            itemsString += GameManager.Instance.inventoryID[i] + ",";
        }
        string newItems = oldItems + itemsString;
        PlayerPrefs.SetString("SavedItems", newItems);
        PlayerPrefs.Save();
    }
}
