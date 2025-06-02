using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Shake : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] GameObject fishTank;
    [SerializeField] float forceAmount = 3f;

    private Rigidbody rb;
    private bool isShaking;
    void Start()
    {
        rb = fishTank.GetComponent<Rigidbody>();
    }

    void Update()
    {

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isShaking)
        {
            rb.AddForce(Vector3.right * forceAmount, ForceMode.Impulse);
            GameManager.Instance.delayState();
            isShaking = true;
            Invoke("deplayShake", 1);
        }
    }

    void deplayShake()
    {
        isShaking = false;
    }

    //IEnumerator ShakeSequence()
    //{
    //    Coroutine move = StartCoroutine(MoveTank());
    //    Coroutine rotate = StartCoroutine(RotateTank());

    //    GameManager.Instance.delayState();

    //    isShaking = true;

    //    yield return move;
    //    yield return rotate;

    //    isShaking = false;
    //}
}
