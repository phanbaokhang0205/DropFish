using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
/**
 * 1. Tại sao phải gán giá trị trong start mà không gán trực tiếp ngoài ?
 * 2. Tại sao Init Fish không dùng touchPosition mà phải tạo 1 Vector3 mới là spamwnPosition ?
 * 3. Tại sao điền tham số trong vector3 là 0 rồi mà vẫn gán lại cho spawnPosition.z = 0
 */
public class PlayerController : MonoBehaviour
{
    public GameObject line;
    //public GameObject fishTank;

    private FishManager fishManager;
    private Vector3 touchPosition;
    private Touch touch;

    private int heightDrop;
    private Renderer rendLine;
    private Vector3 targetCenter;

    //fishTank
    //private CompositeCollider2D rendTank;
    //private float tankWidth;
    //private float tankHeight;
    //private float minTank;
    //private float maxTank;

    private void Start()
    {
        fishManager = FishManager.Instance;

        heightDrop = Screen.height / 2;

        touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, heightDrop, 10));
        fishManager.CreateFish(touchPosition);

        ////line to drop fish
        rendLine = line.GetComponent<Renderer>();
        targetCenter = fishManager.chosenFish.GetComponent<Collider>().bounds.center;


        ////fishtank
        //rendTank = fishTank.GetComponent<CompositeCollider2D>();
        //tankWidth = rendTank.bounds.size.x - 3;
        //tankHeight = rendTank.bounds.size.y;
        //minTank = -(tankWidth / 2);
        //maxTank = tankWidth / 2;

        setLinePosition();

    }

    void Update()
    {

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, heightDrop, 10));

            if (touch.phase == TouchPhase.Began)
            {
                if (fishManager.fishScript.isDropped) return;

                fishManager.PrepareFish(touchPosition);
                checkPosition();
                setLinePosition();
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (fishManager.fishScript.isDropped) return;

                fishManager.MoveFish(touchPosition);
                checkPosition();
                setLinePosition();
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                fishManager.DropFish();
                line.SetActive(false);
            }

        }

        if (fishManager.chosenFish == null)
        {
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, heightDrop, 10));
            fishManager.CreateFish(touchPosition);

            setLinePosition();
        }
    }

    void checkPosition()
    {
        Vector3 newPosition = fishManager.chosenFish.transform.position;

        //if (newPosition.x <= minTank)
        //{
        //    newPosition.x = minTank;
        //    fishManager.chosenFish.transform.position = newPosition;
        //    Debug.Log("x: " + newPosition.x);

        //}
        //else if (newPosition.x >= maxTank)
        //{
        //    newPosition.x = maxTank;
        //    fishManager.chosenFish.transform.position = newPosition;
        //    Debug.Log("x: " + newPosition.x);

        //}
    }

    void setLinePosition()
    {
        float objectHeight = rendLine.bounds.size.y;
        Vector3 newLinePosition = line.transform.position;
        newLinePosition.y = targetCenter.y - (objectHeight / 2);
        newLinePosition.x = fishManager.chosenFish.transform.position.x;
        line.transform.position = newLinePosition;
        line.SetActive(true);

    }

}
