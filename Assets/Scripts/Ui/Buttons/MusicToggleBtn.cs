using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicToggleBtn : MonoBehaviour, IPointerClickHandler
{
    public Sprite audioOnSprite;
    public Sprite audioOffSprite;

    private Image buttonImage;
    AudioManager audioMng;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        audioMng = AudioManager.Instance;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        audioMng.isBgmOn = !audioMng.isBgmOn;
        audioMng.PlayBGM();
    }

    private void Update()
    {
        buttonImage.sprite = audioMng.isBgmOn ? audioOnSprite : audioOffSprite;
    }
}
