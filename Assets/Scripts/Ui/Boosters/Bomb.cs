using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bomb : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 touchPosition;
    private Touch touch;
    private Vector3 initPosition;
    private Vector3 initBombGridPosition;
    private Fish fishScript;

    [SerializeField] private GameObject BombGrid;
    [SerializeField] TextMeshProUGUI priceTMP;
    private List<GameObject> fishList = new List<GameObject>();
    int price;
    bool flat;
    void Start()
    {
        initPosition = transform.position;
        initBombGridPosition = BombGrid.transform.position;
        price = int.Parse(priceTMP.text);
        flat = GameManager.Instance.isAvailableCoin(price);
    }

    void Update()
    {

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.onChosen) return;
        GameManager.Instance.CurrentState = GameManager.GameState.onChosen;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        flat = GameManager.Instance.isAvailableCoin(price);
        //lấy vị trí
        if (!flat) return;
        else
        {
            touch = Input.GetTouch(0);
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 1));
            transform.position = touchPosition;

            BombGrid.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.Instance.CurrentState = GameManager.GameState.Playing;
        //Xóa cá
        //explore(5f, 10f);
        if (fishList.Count != 0)
        {
            foreach (GameObject fish in fishList)
            {
                fish.SetActive(false);
            }
            CameraScript.Instance.shake();
            fishList.Clear();
            if (!flat) return;
            else
            {
                GameManager.Instance.setCoinText(-price);
            }
        }
        else
        {
            Debug.Log("KHông cxoasa");
        }

        //Trừ tiền
        GameManager.Instance.delayState(0.1f);
        transform.position = initPosition;
        BombGrid.transform.position = initBombGridPosition;
    }

    //void explore(float radius, float power)
    //{
    //    Vector3 explosionPos = BombGrid.transform.position;
    //    Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
    //    foreach (Collider hit in colliders)
    //    {
    //        Rigidbody rb = hit.GetComponent<Rigidbody>();

    //        if (rb != null)
    //        {
    //            rb.AddExplosionForce(power, explosionPos, radius, 1f, ForceMode.Impulse);
    //            Debug.Log("ke");
    //        }
    //        Debug.DrawLine(explosionPos, hit.transform.position, Color.red, 1f);

    //    }
    //}
    //void explore(float radius, float power)
    //{
    //    Vector3 explosionPos = BombGrid.transform.position;
    //    Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

    //    foreach (Collider hit in colliders)
    //    {
    //        Rigidbody rb = hit.attachedRigidbody;
    //        if (rb != null)
    //        {
    //            rb.AddExplosionForce(power, explosionPos, radius, 0.1f, ForceMode.Impulse);

    //            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, 3f); // giới hạn vận tốc

    //            rb.linearDamping = 2f;
    //            rb.angularDamping = 2f;

    //            Debug.DrawLine(explosionPos, hit.transform.position, Color.red, 1f);
    //            Debug.Log(hit.name);
    //        }
    //    }
    //}

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
            if (fish.activeInHierarchy && fishScript)
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
