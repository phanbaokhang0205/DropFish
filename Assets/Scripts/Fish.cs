using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private FishManager fishManager;
    private Rigidbody fishRb;
    private SkinnedMeshRenderer smr;
    private Material[] mats;
    private Coroutine flashCoroutine;

    [SerializeField] float flashAlpha = 130f;
    [SerializeField] float flashInterval = 0.2f;
    [SerializeField] List<GameObject> breakableObs = new List<GameObject>();

    public bool inWater;
    public bool isDropped;
    public bool isDataOfLevel = false;
    ParticleSystem splashEffect;
    bool isFlashing;


    void Start()
    {
        fishRb = gameObject.GetComponent<Rigidbody>();
        fishRb.mass = 0;
        fishManager = FishManager.Instance;


        splashEffect = GameObject.FindGameObjectWithTag("splash").GetComponent<ParticleSystem>();
        
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        mats = smr.materials;
        isFlashing = false;
    }

    void Update()
    {
        if (!inWater)
        {

        }
        else
        {
            if (transform.position.y < -5)
            {
                GameManager.Instance.onLoseNormal();
            }

            if (gameObject.tag == "fish_11")
            {
                GameManager.Instance.onWinNormal();
            }
        }
    }

    public void prepareToDrop()
    {
        fishRb = gameObject.GetComponent<Rigidbody>();
        inWater = false;
        isDropped = false;
        fishRb.useGravity = false;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        fishRb.constraints =
            RigidbodyConstraints.FreezePositionX |
            RigidbodyConstraints.FreezePositionY |
            RigidbodyConstraints.FreezePositionZ |
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezeRotationZ;
    }

    public void dropped()
    {
        fishRb = gameObject.GetComponent<Rigidbody>();
        inWater = true;
        isDropped = true;
        fishRb.useGravity = true;
        fishRb.constraints = RigidbodyConstraints.FreezePositionZ |
                              RigidbodyConstraints.FreezeRotationZ |
                              RigidbodyConstraints.FreezeRotationY;
    }

    public void handleBreakableObs()
    {
        if (breakableObs.Count != 0)
        {
            Debug.Log("Fish: " + tag + ", total = " + breakableObs.Count);
            foreach (GameObject obs in breakableObs)
            {
                obs.SetActive(false);
                LevelManager.Instance.targetObstacleAmount--;
                Debug.Log("Fish: " + tag + ", obs = " + breakableObs.Count);
                if (LevelManager.Instance.targetObstacleAmount < 0)
                {
                    LevelManager.Instance.targetObstacleAmount = 0;
                }
                LevelManager.Instance.targetObstacleTMP.text = LevelManager.Instance.targetObstacleAmount.ToString();
            }
            
            breakableObs.Clear();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "water")
        {
            inWater = true;
            if (!isDropped)
            {
                dropped();
            }
            setsplashEffect();
        }

        if (other.gameObject.tag == "test_1")
        {
            Material myMaterial = other.gameObject.GetComponent<Renderer>().material;
            myMaterial.SetFloat("_position_X", transform.position.x);
            myMaterial.SetFloat("_startTime", Time.time);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        fishManager.MergeFish(gameObject, collision.gameObject);
        if (collision.gameObject.tag == "BreakableObstacle")
        {
            breakableObs.Add(collision.gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        fishManager.MergeFish(gameObject, collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "BreakableObstacle")
        {
            breakableObs.Remove(collision.gameObject);
            
        }
    }

    void setsplashEffect()
    {
        splashEffect.transform.position = transform.position;
        splashEffect.Play();
        AudioManager.Instance.PlayWaterDrop();
    }
    public void StartFlash()
    {
        if (!isFlashing && mats.Length > 1)
        {
            isFlashing = true;
            flashCoroutine = StartCoroutine(FlashEffect());
        }
    }

    public void StopFlash()
    {
        if (isFlashing)
        {
            isFlashing = false;
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }

            // Reset alpha về giá trị ban đầu
            Material mat1 = mats[1];
            Color color = mat1.color;
            color.a = 0f;
            mat1.color = color;
        }
    }

    IEnumerator FlashEffect()
    {
        Material mat1 = mats[1];
        bool toggle = false;

        while (isFlashing)
        {
            Color color = mat1.color;
            color.a = toggle ? flashAlpha : 0.7f;
            mat1.color = color;

            toggle = !toggle;

            yield return new WaitForSeconds(flashInterval);
        }
    }
}
