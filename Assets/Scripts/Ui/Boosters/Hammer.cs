using UnityEngine;
using UnityEngine.EventSystems;

public class Hammer : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 initPosition;
    private Vector3 touchPosition;
    private Touch touch;
    private GameObject target;
    private SkinnedMeshRenderer targetSkinned;
    private Material targetMaterial;
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
        // Tạo hiệu ứng nhấp nháy cho cá - chưa được
        Debug.Log(other.tag);
        target = other.gameObject;
        targetSkinned = target.GetComponentInChildren<SkinnedMeshRenderer>();
        targetMaterial = targetSkinned.material;
        targetMaterial.SetFloat("_trigger", 10f);
    }

    private void OnTriggerExit(Collider other)
    {
        // tắt hiệu ứng nhấp nháy - chưa được
        targetMaterial.SetFloat("_trigger", 0f);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(transform.position);
        transform.position = initPosition;
        GameManager.Instance.CurrentState = GameManager.GameState.Playing;
        if (target)
        {
            target.SetActive(false);
        }
    }


}
