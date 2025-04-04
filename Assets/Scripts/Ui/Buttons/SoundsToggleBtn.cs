using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundsToggleBtn : MonoBehaviour, IPointerClickHandler
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
        audioMng.isWaterDropOn = !audioMng.isWaterDropOn;
        audioMng.isMergepOn = !audioMng.isMergepOn;
    }

    private void Update()
    {
        buttonImage.sprite = audioMng.isWaterDropOn ? audioOnSprite : audioOffSprite;
    }
}
