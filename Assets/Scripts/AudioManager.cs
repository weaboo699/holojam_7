using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

    [Header("音量設定")]
    [Range(0f, 1f)]
    public float bgmVolume = 0.5f;
    [Range(0f, 1f)]
    public float sfxVolume = 1.0f;
    
    [Header("遊戲音效")]
    public AudioClip buttonClick;
    public AudioClip captchaCorrect;
    public AudioClip captchaWrong;
    public AudioClip purchaseSuccess;
    public AudioClip purchaseFail;
    public AudioClip itemSoldOut;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        bgmSource.volume = bgmVolume;
    }
    
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip, sfxVolume); 
    }
    
    public void PlaySFX(AudioClip clip, float volumeMultiplier)
    {
        sfxSource.PlayOneShot(clip, sfxVolume * volumeMultiplier);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
