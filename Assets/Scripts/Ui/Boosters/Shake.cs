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

    public float speedRotate = 110f;
    public float speedPos = 30f;
    private List<Quaternion> targetList = new List<Quaternion>();
    private List<Vector3> positionList = new List<Vector3>();

    private Quaternion targetRotate;
    private Quaternion target1;
    private Quaternion target2;
    private Quaternion target3;
    private Quaternion target4;
    private Quaternion target5;

    private Vector3 targetPos;
    private Vector3 pos1;
    private Vector3 pos2;
    private Vector3 pos3;
    private Vector3 pos4;
    private Vector3 pos5;

    private bool isShaking;
    void Start()
    {
        rb = fishTank.GetComponent<Rigidbody>();
        initFishTankPosition = fishTank.transform.position;
        target1 = Quaternion.Euler(0, 0, -10f);
        target2 = Quaternion.Euler(0, 0, 10f);
        target3 = Quaternion.Euler(0, 0, -10f);
        target4 = Quaternion.Euler(0, 0, 10f);
        target5 = Quaternion.Euler(0, 0, 0f);

        targetList.Add(target1);
        targetList.Add(target2);
        targetList.Add(target3);
        targetList.Add(target4);
        targetList.Add(target5);

        pos1 = new Vector3(1.3f, initFishTankPosition.y, initFishTankPosition.z);
        pos2 = new Vector3(2.7f, initFishTankPosition.y, initFishTankPosition.z);
        pos3 = new Vector3(1.3f, initFishTankPosition.y, initFishTankPosition.z);
        pos4 = new Vector3(2.7f, initFishTankPosition.y, initFishTankPosition.z);
        pos5 = initFishTankPosition;
        positionList.Add(pos1);
        positionList.Add(pos2);
        positionList.Add(pos3);
        positionList.Add(pos4);
        positionList.Add(pos5);

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
