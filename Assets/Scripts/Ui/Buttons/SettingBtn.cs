using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingBtn : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.onPause();
    }
}
