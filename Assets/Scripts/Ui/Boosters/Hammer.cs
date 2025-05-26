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
    void Start()
    {
        initPosition = transform.position;
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.StartsWith("fish"))
        {
            target = other.gameObject;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (target)
        {
            fishScript = target.GetComponent<Fish>();
            fishScript.StartFlash();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (fishScript)
        {
            fishScript.StopFlash();
        }
        target = null;

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = initPosition;
        if (target)
        {
            target.SetActive(false);
        }
        GameManager.Instance.delayState();
    }


}
