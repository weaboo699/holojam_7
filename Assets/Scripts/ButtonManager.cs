using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]private int startTime;
    [SerializeField]private SpriteRenderer spr;
    [SerializeField]private Sprite active;
    [SerializeField]private Sprite unactive;
    [SerializeField]private SpriteRenderer border;
    [SerializeField]private SpriteRenderer itemPicRend;
    [SerializeField]private Sprite[] itemPic;
    [SerializeField]private TMP_Text timeText;
    [SerializeField]private int timeScale = 1;

    private int index;
    private int ItemID;
    private int rare;
    private float startSec;
    private float endSec;
    private bool limitTimeSet = false;
    private bool alreadyCounted = false;
    public int zzz;

    private bool isActive;
    private bool blockChecked = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isActive = false;
        rare = UnityEngine.Random.Range(0,3);
        startTime = UnityEngine.Random.Range(12,18);
        ItemID = UnityEngine.Random.Range(0,20);
        changeSprite();
        timeText.text = "start at \n"+ startTime + ":00";
        itemPicRend.sprite = itemPic[ItemID];
    }

    // Update is called once per frame
    void Update()
    {
        zzz = ClockControler.CurrentHour;
        checkStartTime();
        if(isActive)
            checkLimitTime();
    }
    void checkStartTime()
    {
        if(ClockControler.CurrentHour == startTime && !blockChecked)
        {
            isActive = true;
            changeSprite();
            setLimitTime();
            blockChecked = true;
            timeText.text = "time remain \n" + (int)((endSec - ClockControler.CurrentSec)/60);
        }
    }
    void changeSprite()
    {
        if (isActive)
        {
            spr.sprite = active;
        }
        else
        {
            spr.sprite = unactive;
        }
        switch (rare)
        {
            case 0:
                border.color = new Color(0.5f, 0.5f, 0.5f);
                break;
            case 1:
                border.color = new Color(0, 0.5f, 1f);
                break;
            case 2:
                border.color = new Color(1f, 0.84f, 0);
                break;
            default:
                break;
        }
    }
    void setLimitTime()
    {
        if(limitTimeSet)
            return;
        startSec = ClockControler.CurrentSec;
        switch (rare)
        {
            case 0:
                endSec = startSec + (600f * timeScale);
                break;
            case 1:
                endSec = startSec + (400f * timeScale);
                break;
            case 2:
                endSec = startSec + (200f * timeScale);
                break;
            default:
                break;
        }
        limitTimeSet = true;
    }
    void checkLimitTime()
    {
        if(ClockControler.CurrentSec >= endSec && !alreadyCounted)
        {
            alreadyCounted = true;
            isActive = false;
            changeSprite();
            timeText.text ="sold out";
            AudioManager.Instance.PlaySFX(AudioManager.Instance.itemSoldOut);
            GameManager.Instance.itemSold++;
        }
        else
        {
            int remainMin = (int)((endSec - ClockControler.CurrentSec) / 60);
            timeText.text = "time remain \n" + remainMin;
        }

    }
    public void OnPurchaseSuccess()
    {
        if (isActive)
        {
            isActive = false;
            changeSprite();
            timeText.text = "purchased";
            GameManager.Instance.inventoryID[GameManager.Instance.CurrentIndex] = ((rare+1)*100)+ItemID;
            GameManager.Instance.CurrentIndex ++;
            GameManager.Instance.inventoryIndex[rare] ++;
            GameManager.Instance.UpdateUI();
            AudioManager.Instance.PlaySFX(AudioManager.Instance.purchaseSuccess);
            Debug.Log("Purchased!");
            GameManager.Instance.itemSold++;
        }
        else
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.purchaseFail);
            Debug.Log("Sold Out");
        }

    }
    public int GetRare()
    {
        return rare;
    }
    public void SetIndex(int x)
    {
        index = x;
    }
    void OnMouseDown()
    {
        if (isActive)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
            Debug.Log("Start Captcha");
            GameManager.Instance.DisplayCaptcha(true,this);
        }
            
    }
}
