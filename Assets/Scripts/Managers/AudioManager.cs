using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip waterDropClip;
    public AudioClip mergeAudioClip;
    public AudioClip backgroundClip;

    private AudioSource sfxSource;
    private AudioSource bgmSource;

    //public bool isWaterDropOn = true;
    //public bool isMergepOn = true;
    public bool isBgmOn;
    public bool isSoundOn;
    [SerializeField] Image MusicImageHome;
    [SerializeField] Image SoundImageHome;

    [SerializeField] Image MusicImageNormal;
    [SerializeField] Image SoundImageNornaml;

    [SerializeField] Image MusicImageAdventure;
    [SerializeField] Image SoundImageAdventure;

    void Awake()
    {
        Instance = this;
        sfxSource = gameObject.AddComponent<AudioSource>();
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        isBgmOn = PlayerPrefsManager.GetMusic();

    }

    private void Start()
    {
        setColorForMusic(MusicImageHome);
        setColorForSound(SoundImageHome);
        setColorForMusic(MusicImageNormal);
        setColorForSound(SoundImageNornaml);
        setColorForMusic(MusicImageAdventure);
        setColorForSound(SoundImageAdventure);
        PlayBGM();
    }

    public void setColorForMusic(Image img)
    {
        bool isFaded = PlayerPrefsManager.GetMusic();
        Color c = img.color;
        c.a = isFaded ? 1f : 0.3f;
        img.color = c;
    }
    public void handleMusic(Image img)
    {
        isBgmOn = !isBgmOn;
        PlayerPrefsManager.SetMusic(isBgmOn);
        setColorForMusic(img);
        PlayBGM();
    }

    public void setColorForSound(Image img)
    {
        bool isFaded = PlayerPrefsManager.GetSound();
        Color c = img.color;
        c.a = isFaded ? 1f : 0.3f;
        img.color = c;

    }
    public void handleSound(Image img)
    {
        isSoundOn = !isSoundOn;
        PlayerPrefsManager.SetSound(isSoundOn);
        setColorForSound(img);

    }
    public void PlayWaterDrop()
    {
        if (PlayerPrefsManager.GetSound())
        {
            sfxSource.PlayOneShot(waterDropClip);
        }
    }

    public void PlayMergeAudio()
    {
        if (PlayerPrefsManager.GetSound())
            sfxSource.PlayOneShot(mergeAudioClip);
    }

    public void PlayBGM()
    {
        if (isBgmOn)
        {
            if (!bgmSource.isPlaying)
            {
                bgmSource.clip = backgroundClip;
                bgmSource.volume = 1f;
                bgmSource.Play();
            }
        }
        else
        {
            if (bgmSource.isPlaying)
            {
                bgmSource.Stop();
            }
        }
    }


}
