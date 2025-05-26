using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Shake : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject fishTank;
    private Vector3 initFishTankPosition;
    private Rigidbody rb;

    public float speedRotate = 180f;
    public float speedPos = 40f;
    private List<Quaternion> targetList = new List<Quaternion>();
    private List<Vector3> positionList = new List<Vector3>();

    private Quaternion targetRotate;
    private Quaternion target1;
    private Quaternion target2;
    private Quaternion target3;

    private Vector3 targetPos;
    private Vector3 pos1;
    private Vector3 pos2;
    private Vector3 pos3;

    private bool isShaking;
    void Start()
    {
        rb = fishTank.GetComponent<Rigidbody>();
        initFishTankPosition = fishTank.transform.position;
        target1 = Quaternion.Euler(0, 0, -15f);
        target2 = Quaternion.Euler(0, 0, 15f);
        target3 = Quaternion.Euler(0, 0, 0f);

        targetList.Add(target1);
        targetList.Add(target2);
        targetList.Add(target3);

        pos1 = new Vector3(0.8f, initFishTankPosition.y, initFishTankPosition.z);
        pos2 = new Vector3(3.2f, initFishTankPosition.y, initFishTankPosition.z);
        pos3 = initFishTankPosition;
        positionList.Add(pos1);
        positionList.Add(pos2);
        positionList.Add(pos3);

        targetRotate = fishTank.transform.rotation;
        targetPos = fishTank.transform.position;
    }

    void Update()
    {

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeSequence());
            Debug.Log("From shake: " + GameManager.Instance.CurrentState);
        }
    }

    IEnumerator ShakeSequence()
    {
        Coroutine move = StartCoroutine(MoveTank());
        Coroutine rotate = StartCoroutine(RotateTank());

        GameManager.Instance.delayState();

        isShaking = true;

        yield return move;
        yield return rotate;

        isShaking = false;
    }

    IEnumerator RotateTank()
    {
        foreach (Quaternion item in targetList)
        {
            targetRotate = item;
            while (Quaternion.Angle(fishTank.transform.rotation, targetRotate) > 0.1f)
            {
                //fishTank.transform.rotation = Quaternion.RotateTowards(
                //    fishTank.transform.rotation,
                //    targetRotate,
                //    speedRotate * Time.deltaTime
                //);

                rb.MoveRotation(Quaternion.RotateTowards(
                    fishTank.transform.rotation,
                    targetRotate,
                    speedRotate * Time.deltaTime
                ));



                yield return null;
            }
        }
    }

    IEnumerator MoveTank()
    {
        foreach (Vector3 item in positionList)
        {
            targetPos = item;
            while (Vector3.Distance(fishTank.transform.position, targetPos) > 0.01f)
            {
                //fishTank.transform.position = Vector3.Lerp(
                //    fishTank.transform.position,
                //    targetPos,
                //    Time.deltaTime * speedPos
                //);

                rb.MovePosition(Vector3.Lerp(
                    fishTank.transform.position,
                    targetPos,
                    Time.deltaTime * speedPos
                ));

                yield return null;
            }
        }
    }
}
