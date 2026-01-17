using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoToTutorial()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        SceneManager.LoadScene("Tutorial");
    }
    public void GoToGame()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        SceneManager.LoadScene("SampleScene");
    }
    public void GoToRoom()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        SceneManager.LoadScene("RoomScene");
    } 
    public void ClearTestData()
    {
        PlayerPrefs.DeleteAll();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        Debug.Log("測試資料已清除");
        Application.Quit();
    }
}
