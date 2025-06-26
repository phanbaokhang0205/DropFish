using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Shake : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] GameObject fishTank;
    [SerializeField] float forceAmount = 3f;
    [SerializeField] TextMeshProUGUI priceTMP;
    int price;
    bool flat;
    private Rigidbody rb;
    private bool isShaking;
    void Start()
    {
        rb = fishTank.GetComponent<Rigidbody>();
        price = int.Parse(priceTMP.text);
        flat = GameManager.Instance.isAvailableCoin(price);
    }

    void Update()
    {

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        flat = GameManager.Instance.isAvailableCoin(price);

        if (!isShaking && flat)
        {
            rb.AddForce(Vector3.right * forceAmount, ForceMode.Impulse);
            GameManager.Instance.delayState();
            isShaking = true;
            
            GameManager.Instance.setCoinText(-price);
            Invoke("deplayShake", 2);
        }
    }

    void deplayShake()
    {
        isShaking = false;
    }
}
