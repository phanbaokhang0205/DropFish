using UnityEngine;
using UnityEngine.EventSystems;

public class Shake : MonoBehaviour, IPointerClickHandler
{
    void Start()
    {

    }

    void Update()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Rung bần bật");
    }



}
