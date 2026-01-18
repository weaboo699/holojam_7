using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ProductItemManager : MonoBehaviour
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

    private int GameStartTime = 12; //遊戲開始時 時鐘的時間會是12:00
    private int GameEndTime = 18; //時鐘的時間到18:00(6:00pm)時 遊戲結束
    private int index;
    private int ItemID;
    private int rare;
    private float startSec;
    private float endSec;
    private bool limitTimeSet = false;
    private bool alreadyCounted = false;

    private bool isActive;
    private bool blockChecked = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isActive = false;
        rare = UnityEngine.Random.Range(0,3);
        startTime = UnityEngine.Random.Range(GameStartTime,GameEndTime);
        ItemID = UnityEngine.Random.Range(0,20);
        changeSprite();
        timeText.text = "start at \n"+ startTime + ":00";
        itemPicRend.sprite = itemPic[ItemID];
    }

    // Update is called once per frame
    void Update()
    {
        checkStartTime();
        if(isActive)
            checkLimitTime();
    }
    void checkStartTime()
    {
        if(ClockManager.Instance.CurrentHour == startTime && !blockChecked)
        {
            isActive = true;
            changeSprite();
            setLimitTime();
            blockChecked = true;
            timeText.text = "time remain \n" + (int)((endSec - ClockManager.Instance.CurrentSec)/60);
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
        startSec = ClockManager.Instance.CurrentSec;
        switch (rare)
        {
            case 0:
                endSec = startSec + (600f * timeScale); //稀有度最低 搶購時間遊戲內60秒
                break;
            case 1:
                endSec = startSec + (400f * timeScale); //稀有度中等 搶購時間遊戲內40秒
                break;
            case 2:
                endSec = startSec + (200f * timeScale); //稀有度最高 搶購時間遊戲內20秒
                break;
            default:
                break;
        }
        limitTimeSet = true;
    }
    void checkLimitTime()
    {
        if(ClockManager.Instance.CurrentSec >= endSec && !alreadyCounted)
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
            int remainMin = (int)((endSec - ClockManager.Instance.CurrentSec) / 60);
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
            CaptchaManager.Instance.DisplayCaptcha(true,this);
        }
            
    }
}
