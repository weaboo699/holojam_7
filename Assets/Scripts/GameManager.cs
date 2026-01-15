using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int hintValue;
    private string trueAnswer;
    private string trueAnswer2 = "wadawdawrfawrf";
    private ButtonManager currentButton;
    private bool gameEnded = false;
    private List<ButtonManager> btnObjects = new List<ButtonManager>();

    [SerializeField]private GameObject btnPrefab;
    [SerializeField]private Transform spawnTransform;
    [SerializeField]private GameObject captcha;
    [SerializeField]private int spawnSpacing;
    [SerializeField]private int items;
    [SerializeField]private TMP_Text inventorySlotR0;
    [SerializeField]private TMP_Text inventorySlotR1;
    [SerializeField]private TMP_Text inventorySlotR2;
    [SerializeField]private GameObject EndScreen;
    [SerializeField]private TMP_Text resultText;
    [SerializeField]private TMP_Text rankText;
    [SerializeField]private Sprite[] captchaSprite;

    public int itemSold = 0;
    public int CurrentIndex = 0;
    public int[] inventoryIndex; 
    public int[] inventoryID;
    public TMP_InputField PlayerAnswer;
    public Image HintSprite;
    public static GameManager Instance { get; private set; }
    
    void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EndScreen.SetActive(false);
        captcha.SetActive(false);
        inventoryIndex = new int[3];
        inventoryID = new int[items];
        spawnBlock();
        changeCaptchaSprite();
    }

    // Update is called once per frame
    void Update()
    {
        if(itemSold >= items && !gameEnded)
        {
            gameEnded = true;
            ShowEndScreen();
        }
        if (captcha.activeInHierarchy)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                checkValue();
            }
        }
    }
    void changeCaptchaSprite()
    {
        hintValue = UnityEngine.Random.Range(0,8);
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
                defineAnswer("guyrys","irystocrats");
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
    public void checkValue()
    {
        if(PlayerAnswer.text.ToUpper() == trueAnswer.ToUpper() || PlayerAnswer.text.ToUpper() == trueAnswer2.ToUpper())
        {
            Debug.Log("Corrext!!!");
            AudioManager.Instance.PlaySFX(AudioManager.Instance.captchaCorrect);
            currentButton.OnPurchaseSuccess();
            changeCaptchaSprite();
            DisplayCaptcha(false,null);
        }
        else
        {
            Debug.Log("Incorrect!!!");
            AudioManager.Instance.PlaySFX(AudioManager.Instance.captchaWrong);
            changeCaptchaSprite();
            DisplayCaptcha(false,null);
        }
        PlayerAnswer.text = null;
    }
    void defineAnswer(String x,String y)
    {
        trueAnswer = x;
        trueAnswer2 = y;
    }
    void spawnBlock()
    {
        GameObject[] block =new GameObject[items];
        for(int i = 0; i < items; i++)
        {
            int x = spawnSpacing * (i%3-1);
            int y = spawnSpacing * (1-i/3);
            block[i] = Instantiate(btnPrefab, spawnTransform);
            block[i].transform.localPosition = new Vector3(x,y,0);
            ButtonManager btnSetup = block[i].GetComponent<ButtonManager>();
            btnSetup.SetIndex(i);
            btnObjects.Add(btnSetup);
        }
    }
    public void ShowEndScreen()
    {
        string oldItems = PlayerPrefs.GetString("SavedItems", "");
        string itemsString = "";
        for(int i = 0; i < CurrentIndex; i++)
        {
            itemsString += inventoryID[i] + ",";
        }
        string newItems = oldItems + itemsString;
        PlayerPrefs.SetString("SavedItems", newItems);
        PlayerPrefs.Save();

        string result = "Result: " + CurrentIndex +"/"+ items + "\n";
        for(int i = 0; i < 3; i++)
        {
            switch (i)
            {
                case 0:
                    result += "Rare: ";
                    break;
                case 1:
                    result += "Epic: ";
                    break;
                case 2:
                    result += "Legendary: ";
                    break;
                default:
                    break;
            }
            result += inventoryIndex[i] + "\n";
        }
        resultText.text = result;

        string rank = GetRank();
        rankText.text = rank;
        Color col = rankText.color;
        col.a = 0;
        rankText.color = col;

        EndScreen.SetActive(true);
        StartCoroutine(FadeInRank());
    }
    string GetRank()
    {
        float successRate = (float)CurrentIndex / items;
        if(successRate >= 0.9f) return "S";
        if(successRate >= 0.7f) return "A";
        if(successRate >= 0.5f) return "B";
        if(successRate >= 0.3f) return "C";
        return "D";
    }
    IEnumerator FadeInRank()
    {
        yield return new WaitForSeconds(0.5f);
        
        float duration = 1.0f;
        float elapsed = 0;
        
        Color col = rankText.color;
        RectTransform rect = rankText.GetComponent<RectTransform>();
        Vector3 targetScale = rect.localScale;
        rect.localScale = targetScale * 0.5f; 
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            col.a = Mathf.Lerp(0, 1, t);
            rankText.color = col;

            rect.localScale = Vector3.Lerp(targetScale * 0.5f, targetScale, t);
            
            yield return null;
        }
        
        col.a = 1;
        rankText.color = col;
        rect.localScale = targetScale;
    }
    public void UpdateUI()
    {
        inventorySlotR0.text = inventoryIndex[0].ToString();
        inventorySlotR1.text = inventoryIndex[1].ToString();
        inventorySlotR2.text = inventoryIndex[2].ToString();
    }
    public void DisplayCaptcha(bool x, ButtonManager y)
    {
        captcha.SetActive(x);
        if(x)
            PlayerAnswer.ActivateInputField();
        if(y != null)
            currentButton = y;
    }
    public void GoToMenu()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        SceneManager.LoadScene("MainMenu");
    }
}
