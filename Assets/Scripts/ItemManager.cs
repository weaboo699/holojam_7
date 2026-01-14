using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour, IPointerClickHandler
{
    private int itemIndex;
    private int id;
    [SerializeField]private Image displayImage;
    [SerializeField]private Sprite[] sourceImageR0;
    [SerializeField]private Sprite[] sourceImageR1;
    [SerializeField]private Sprite[] sourceImageR2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetSelected(false);
        if(id/100 == 1)
            displayImage.sprite = sourceImageR0[id%100];
        else if(id/100 == 2)
            displayImage.sprite = sourceImageR1[id%100];
        else
            displayImage.sprite = sourceImageR2[id%100];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetIndex(int x)
    {
        itemIndex = x;        
    }
    public void SetID(int x)
    {
        id = x;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        RoomManager.Instance.SelectItem(itemIndex);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        Debug.Log(itemIndex + " got clicked");
    }
    public void SetSelected(bool selected)
    {
        Image img = GetComponent<Image>();
        Color col = img.color;
        col.a = selected ? 1f : 0.5f;
        img.color = col;
    }
}
