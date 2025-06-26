using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class Hammer : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 initPosition;
    private Vector3 touchPosition;
    private Touch touch;
    private GameObject target;
    private SkinnedMeshRenderer targetSkinned;
    private Material targetMaterial;
    private Fish fishScript;
    [SerializeField] TextMeshProUGUI priceTMP;
    int price;
    bool flat;
    void Start()
    {
        initPosition = transform.position;
        price = int.Parse(priceTMP.text);
        flat = GameManager.Instance.isAvailableCoin(price);
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
        flat = GameManager.Instance.isAvailableCoin(price);

        // Lấy vị trí
        if (!flat) return;
        else
        {
            touch = Input.GetTouch(0);
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 1));
            transform.position = touchPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.StartsWith("fish"))
        {
            target = other.gameObject;
            Debug.Log("In " + other.tag);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (target)
        {
            fishScript = target.GetComponent<Fish>();
            fishScript.StartFlash();
            Debug.Log("Stay " + other.tag);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (fishScript)
        {
            fishScript.StopFlash();
            Debug.Log("Out " + other.tag);
        }
        target = null;
        fishScript = null;

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = initPosition;
        if (target)
        {
            target.SetActive(false);
            fishScript = target.GetComponent<Fish>();
            fishScript.StopFlash();

        }
        if (!flat) return;
        else
        {
            GameManager.Instance.setCoinText(-price);
        }
        GameManager.Instance.delayState();
    }


}
