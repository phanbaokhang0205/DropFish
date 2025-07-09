using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class Hammer : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
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
        //if (GameManager.Instance.CurrentState == GameManager.GameState.onChosen) return;

        //flat = GameManager.Instance.isAvailableCoin(price);
        //if (!flat) return;
        //else
        //{
        //    GameManager.Instance.CurrentState = GameManager.GameState.onChosen;
        //}

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.onChosen) return;

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

    private void OnTriggerStay(Collider other)
    {
        if (!other.tag.StartsWith("fish")) return;

        if (target == other.gameObject) return;

        if (fishScript)
            fishScript.StopFlash();

        target = other.gameObject;
        fishScript = target.GetComponent<Fish>();
        fishScript.StartFlash();

        Debug.Log("StartFlash on new fish: " + target.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (fishScript)
        {
            fishScript.StopFlash();
        }
        target = null;
        fishScript = null;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.onChosen) return;

        transform.position = initPosition;
        if (target)
        {
            target.SetActive(false);
            fishScript = target.GetComponent<Fish>();
            fishScript.StopFlash();

            if (!flat) return;
            else
            {
                GameManager.Instance.setCoinText(-price);
            }
        }
        
        GameManager.Instance.delayState(0.1f);
    }


}
