using System.Collections;
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

    public bool inWater;
    public bool isDropped;
    public bool isMerge;
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
            if (isMerge)
            {
                dropped();
            }
        } else
        {
            if (transform.position.y < -5)
            {
                GameManager.Instance.onLose();
            }

            if (gameObject.tag == "fish_11")
            {
                GameManager.Instance.onWin();
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "water")
        {
            inWater = true;
            dropped();
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
    }

    private void OnCollisionStay(Collision collision)
    {
        fishManager.MergeFish(gameObject, collision.gameObject);
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
            color.a = toggle ? flashAlpha : 1f;
            mat1.color = color;

            toggle = !toggle;

            yield return new WaitForSeconds(flashInterval);
        }
    }
}
