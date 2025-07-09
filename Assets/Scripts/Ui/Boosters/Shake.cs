using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Shake : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] GameObject fishTank;
    [SerializeField] float forceAmount;
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
            GameManager.Instance.delayState(3f);
            isShaking = true;

            GameManager.Instance.setCoinText(-price);

            // Tạo sequence tween
            Sequence camSeq = DOTween.Sequence();

            camSeq.AppendCallback(() =>
            {
                CameraScript.Instance.zoomOut();
            });

            camSeq.AppendInterval(0.5f);

            camSeq.AppendCallback(() =>
            {
                rb.AddForce(Vector3.right * forceAmount, ForceMode.Impulse);
            });

            camSeq.AppendInterval(2f);

            camSeq.AppendCallback(() =>
            {
                CameraScript.Instance.zoomIn();
                Invoke(nameof(deplayShake), 2);
            });

        }
    }

    void deplayShake()
    {
        isShaking = false;
    }
}
