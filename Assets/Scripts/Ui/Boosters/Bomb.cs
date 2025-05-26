using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class Bomb : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 touchPosition;
    private Touch touch;
    private Vector3 initPosition;
    private Vector3 initBombGridPosition;
    private Fish fishScript;

    [SerializeField] private GameObject BombGrid;

    private List<GameObject> fishList = new List<GameObject>();


    void Start()
    {
        initPosition = transform.position;
        initBombGridPosition = BombGrid.transform.position;
    }

    void Update()
    {

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.Instance.CurrentState = GameManager.GameState.onChosen;
    }

    
    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        touch = Input.GetTouch(0);
        touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 1));
        transform.position = touchPosition;

        BombGrid.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(transform.position);
        transform.position = initPosition;
        BombGrid.transform.position = initBombGridPosition;
        GameManager.Instance.CurrentState = GameManager.GameState.Playing;

        foreach (GameObject fish in fishList)
        {
            fish.SetActive(false);
        }
        fishList.Clear();

        GameManager.Instance.delayState();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.StartsWith("fish"))
        {
            fishList.Add(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        foreach (GameObject fish in fishList)
        {
            fishScript = fish.GetComponent<Fish>();
            if (fish.activeInHierarchy)
            {
                fishScript.StartFlash();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (GameObject fish in fishList)
        {
            fishScript = fish.GetComponent<Fish>();
            fishScript.StopFlash();
        }
        fishList.Remove(other.gameObject);
    }

}
