using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]private int slotID = -1;
    [SerializeField]private bool slotUsed = false;
    [SerializeField]private int slotIndex;
    [SerializeField]private Sprite slotSprite;
    [SerializeField]private Image slotImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetID(int x)
    {
        slotID = x;
    }
    public int GetID()
    {
        return slotID;
    }
    public void SetImage(Sprite x)
    {
        slotSprite = x;
        slotImage.sprite = slotSprite;
        Color col = slotImage.color;
        col.a = 1f;
        slotImage.color = col;
        slotUsed = true;
        Debug.Log(slotIndex + "setIMG");
    }
    public void RemoveItem()
    {
        slotSprite = null;
        slotID = -1;
        slotImage.sprite = slotSprite;
        Color col = slotImage.color;
        col.a = 0.5f;
        slotImage.color = col;
        slotUsed = false;
        Debug.Log(slotIndex + "removeIMG");
    }
    public void SetIndex(int x)
    {
        slotIndex = x;
        Debug.Log(slotIndex + "spawn");
    }
    public bool CheckIfUsed()
    {
        return slotUsed;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        RoomManager.Instance.SelectSlot(slotIndex);
    }
}
