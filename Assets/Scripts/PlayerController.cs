using System.Collections;
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
    public ParticleSystem splashEffect;
    public GameObject water;

    private GameManager gameManager;
    private FishManager fishManager;
    private Vector3 touchPosition;
    private Touch touch;

    private int heightDrop;
    private Renderer rendLine;
    private Vector3 targetCenter;

    private Coroutine createFishCoroutine;

    //fishTank
    private Renderer waterSize;
    private float waterHeight;
    private float waterWidth;
    private float leftSide;
    private float rightSide;

    private void Start()
    {
        gameManager = GameManager.Instance;
        fishManager = FishManager.Instance;

        /// fish tank props
        Vector3 waterScreenPos = Camera.main.WorldToScreenPoint(water.transform.position);
        waterHeight = waterScreenPos.y * 2;
        waterWidth = waterScreenPos.x;

        waterSize = water.GetComponent<Renderer>();
        leftSide = Camera.main.WorldToScreenPoint(new Vector3(waterSize.bounds.min.x, 0, 0)).x;
        rightSide = Camera.main.WorldToScreenPoint(new Vector3(waterSize.bounds.max.x, 0, 0)).x;
        Debug.Log("min" + waterSize.bounds.min.x);
        Debug.Log("max" + waterSize.bounds.max.x);

        /// init position
        touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(waterWidth, waterHeight, 10));
        fishManager.CreateFish(touchPosition);

        ////line to drop fish
        rendLine = line.GetComponent<Renderer>();
        targetCenter = fishManager.chosenFish.GetComponent<Collider>().bounds.center;



        setLinePosition();

    }

    void Update()
    {

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, waterHeight, 10));

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

                Debug.Log(fishManager.chosenFish.transform.position.x);
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
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, waterHeight, 10));
            fishManager.CreateFish(touchPosition);
            //createFishCoroutine = StartCoroutine(DelayCreateFish(touchPosition));
            setLinePosition();
        }

        if (fishManager.fishScript.inWater)
        {
            setsplashEffect();
        }

        // GameState
        if (fishManager.chosenFish.transform.position.y < -5)
        {
            gameManager.onLose();
        }
    }

    void checkPosition()
    {
        /// world to screen
        Vector3 newPosition = fishManager.chosenFish.transform.position;
        float min = waterSize.bounds.min.x + 1;
        float max = waterSize.bounds.max.x - 1;
        /// screen to world
        if (newPosition.x <= min)
        {
            newPosition.x = min;
            fishManager.chosenFish.transform.position = newPosition;
        }
        else if (newPosition.x >= max)
        {
            newPosition.x = max;
            fishManager.chosenFish.transform.position = newPosition;
        }
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

    void setsplashEffect()
    {
        splashEffect.transform.position = fishManager.chosenFish.transform.position;
        splashEffect.Play();
    }

    IEnumerator DelayCreateFish(Vector3 touchPosition)
    {
        yield return new WaitForSeconds(1f);
        fishManager.CreateFish(touchPosition);
    }
}
