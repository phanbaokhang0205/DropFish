using UnityEngine;
using UnityEngine.EventSystems;

public class RestartBtn : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.restartGame();
    }
}
