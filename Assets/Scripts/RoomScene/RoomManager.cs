using System;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RoomManager : MonoBehaviour
{
    private List<ItemManager> itemObjects = new List<ItemManager>();
    private List<SlotManager> slotObjects = new List<SlotManager>();
    [SerializeField]private Transform itemsParent;
    [SerializeField]private Transform slotsParent;
    [SerializeField]private GameObject slotPrefab; 
    [SerializeField]private GameObject itemPrefab;
    [SerializeField]private List<Sprite> DisplayImage;
    [SerializeField]private List<Sprite> DisplayImageR1;
    [SerializeField]private List<Sprite> DisplayImageR2;
    [SerializeField]private int spawnSpacing;
    [SerializeField]private int slotAmount;
    
    private List<int> itemsID = new List<int>();
    private int selectedItemIndex = -1;
    
    private int[] displaySlots;      
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static RoomManager Instance { get; private set; }
    
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        RoomSaveData roomData = DataManager.Instance.LoadRoomData();
        GameSaveData gameData = DataManager.Instance.LoadGameData();

        
        displaySlots = new int[slotAmount];
        for(int i = 0; i < slotAmount; i++)
        {
            displaySlots[i] = -1;
        }
        spawnSlots();

        itemsID = gameData.inventoryIDs;
        if (roomData.displayedSlots.Count > 0)
        {
            for(int i = 0; i < roomData.displayedSlots.Count; i++)
            {
                displaySlots[i] = roomData.displayedSlots[i];
                
                if(itemsID[i] > 0)
                {
                    slotObjects[i].SetID(itemsID[i]);
                    if(itemsID[i] /100 == 1)
                        slotObjects[i].SetImage(DisplayImage[itemsID[i]%100]);
                    else if(itemsID[i] / 100 == 2)
                        slotObjects[i].SetImage(DisplayImageR1[itemsID[i]%100]);
                    else
                        slotObjects[i].SetImage(DisplayImageR2[itemsID[i]%100]);
                }
            }
        }

        InitializeItemList();
    }

    // Update is called once per frame
    void InitializeItemList()
    {
        string itemsString = PlayerPrefs.GetString("SavedItems", "");

        if (string.IsNullOrEmpty(itemsString))
        {
            itemsID = new List<int>(0); 
            return;
        }

        string[] ID = itemsString.Split(',');
        itemsID = new List<int>(ID.Length - 1);
        for(int i = 0; i < ID.Length - 1; i++)
        {
            itemsID.Add(Int32.Parse(ID[i]));
            Debug.Log(itemsID[i]);
        }
        spawnItems();        
    }
    void spawnSlots()
    {
        for(int i = 0; i < slotAmount; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotsParent);
            SlotManager slotSetup = slot.GetComponent<SlotManager>();
            slotSetup.SetIndex(i);
            slotObjects.Add(slotSetup);
        }
    }
    void spawnItems()
    {
        for(int i = 0; i < itemsID.Count; i++)
        {
            GameObject item;
            item = Instantiate(itemPrefab, itemsParent);
            ItemManager itemSetup = item.GetComponent<ItemManager>();
            itemSetup.SetID(itemsID[i]);
            itemSetup.SetIndex(i);
            itemObjects.Add(itemSetup);
        }
    }
    void ReturnItemToInventory(int itemID)
    {
        itemsID.Add(itemID);
        
        GameObject item;
        item = Instantiate(itemPrefab, itemsParent);    
        ItemManager itemSetup = item.GetComponent<ItemManager>();
        itemSetup.SetID(itemID);
        itemSetup.SetIndex(itemsID.Count - 1);  
        itemObjects.Add(itemSetup);
    }
    public void GoToMainMenu()
    {
        DataManager.Instance.SaveGameData(itemsID.ToArray(), itemsID.Count);
        DataManager.Instance.SaveRoomData(displaySlots);
        
        SceneManager.LoadScene("MainMenu");
    }
    public void SelectItem(int index)
    {
        if(selectedItemIndex >= 0 && selectedItemIndex < itemObjects.Count)
        {
            itemObjects[selectedItemIndex].SetSelected(false);
        }
        selectedItemIndex = index;
        itemObjects[index].SetSelected(true);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        Debug.Log("Selected item: " + itemsID[index]);
    }
    public void SelectSlot(int index)
    {
        if(selectedItemIndex < 0)
        {
            if (slotObjects[index].CheckIfUsed())
            {
                int itemID = displaySlots[index];
                ReturnItemToInventory(itemID);

                slotObjects[index].RemoveItem();
                displaySlots[index] = -1;
            }
        }
        else if(selectedItemIndex >= 0 && selectedItemIndex < itemObjects.Count)
        {
            if (slotObjects[index].CheckIfUsed())
            {
                int oldItemID = displaySlots[index];
                ReturnItemToInventory(oldItemID);
            }
            int currentSelectedID = itemsID[selectedItemIndex];
            slotObjects[index].SetID(currentSelectedID);
            if(currentSelectedID/100==1)
                slotObjects[index].SetImage(DisplayImage[currentSelectedID%100]);
            else if(currentSelectedID/100==2)
                slotObjects[index].SetImage(DisplayImageR1[currentSelectedID%100]);
            else  
                slotObjects[index].SetImage(DisplayImageR2[currentSelectedID%100]);
            displaySlots[index] = currentSelectedID;

            Destroy(itemObjects[selectedItemIndex].gameObject);
            itemObjects.RemoveAt(selectedItemIndex);
            itemsID.RemoveAt(selectedItemIndex);

            for(int i = selectedItemIndex; i < itemObjects.Count; i++)
            {
                itemObjects[i].SetIndex(i);
            }
            selectedItemIndex = -1;                
        }
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
    }
}
