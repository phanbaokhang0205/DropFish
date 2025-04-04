using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip waterDropClip;
    public AudioClip mergeAudioClip;
    public AudioClip backgroundClip;

    private AudioSource sfxSource;
    private AudioSource bgmSource;

    public bool isWaterDropOn = true;
    public bool isMergepOn = true;
    public bool isBgmOn = true;

    void Awake()
    {
        Instance = this;
        sfxSource = gameObject.AddComponent<AudioSource>();

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        PlayBGM();
    }
    public void PlayWaterDrop()
    {
        if (isWaterDropOn)
            sfxSource.PlayOneShot(waterDropClip);
    }

    public void PlayMergeAudio()
    {
        if (isMergepOn)
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
