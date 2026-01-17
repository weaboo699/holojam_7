using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    
    [SerializeField]private GameObject[]ppt;
    [SerializeField]private int pptAmounts;
    private int index = 0;
    void Start()
    {
        for(int i = 0; i < pptAmounts; i++)
        {
            ppt[i].SetActive(false);
        }
        ppt[index].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoNext()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        ppt[index].SetActive(false);
        index++;
        if(index >= pptAmounts)
            SceneManager.LoadScene("MainMenu");
        else
            ppt[index].SetActive(true);
    }
}
