using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaptchaManager : MonoBehaviour
{
    [SerializeField]private Image HintSprite;
    [SerializeField]private Sprite[] captchaSprite;
    [SerializeField]TMP_InputField PlayerAnswer;

    private ProductItemManager currentItem;
    private int hintValue;
    private string trueAnswer;
    private string trueAnswer2 = "wadawdawrfawrf";
    public static CaptchaManager Instance {get; set;}
    void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ChangeCaptchaSprite()
    {
        hintValue = UnityEngine.Random.Range(0,captchaSprite.Length);
        switch (hintValue)
        {
            case 0 :
                HintSprite.sprite = captchaSprite[0];
                defineAnswer("chattino","chattini");
                break;
            case 1:
                HintSprite.sprite = captchaSprite[1];
                defineAnswer("fuwawa","fuwawa abyssgard");
                break;
            case 2:
                HintSprite.sprite = captchaSprite[2];
                defineAnswer("gigi","gigi murin");
                break;
            case 3 :
                HintSprite.sprite = captchaSprite[3];
                defineAnswer("guyrys","irystocrat");
                break;
            case 4:
                HintSprite.sprite = captchaSprite[4];
                defineAnswer("irys","irys");
                break;
            case 5:
                HintSprite.sprite = captchaSprite[5];
                defineAnswer("jailbird","jailbirds");
                break;
            case 6 :
                HintSprite.sprite = captchaSprite[6];
                defineAnswer("kfp","kfps");
                break;
            case 7:
                HintSprite.sprite = captchaSprite[7];
                defineAnswer("mococo","mococo abyssgard");
                break;
            case 8:
                HintSprite.sprite = captchaSprite[8];
                defineAnswer("otomo","otomos");
                break;                
            default:
                break;
        }
    }
    public void CheckValue()
    {
        if(PlayerAnswer.text.ToUpper() == trueAnswer.ToUpper() || PlayerAnswer.text.ToUpper() == trueAnswer2.ToUpper())
        {
            Debug.Log("Corrext!!!");
            AudioManager.Instance.PlaySFX(AudioManager.Instance.captchaCorrect);
            currentItem.OnPurchaseSuccess();
            ChangeCaptchaSprite();
            DisplayCaptcha(false,null);
        }
        else
        {
            Debug.Log("Incorrect!!!");
            AudioManager.Instance.PlaySFX(AudioManager.Instance.captchaWrong);
            ChangeCaptchaSprite();
            DisplayCaptcha(false,null);
        }
        PlayerAnswer.text = null;
    }
    public void DisplayCaptcha(bool x, ProductItemManager y)
    {
        GameManager.Instance.captcha.SetActive(x);
        if(x)
            PlayerAnswer.ActivateInputField();
        if(y != null)
            currentItem = y;
    }
    void defineAnswer(String x,String y)
    {
        trueAnswer = x;
        trueAnswer2 = y;
    }
}
