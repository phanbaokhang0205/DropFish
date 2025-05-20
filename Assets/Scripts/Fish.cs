using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Fish : MonoBehaviour
{

    private FishManager fishManager;
    private Rigidbody fishRb;

    public bool inWater;
    public bool isDropped;
    public ParticleSystem splashEffect;

    void Start()
    {
        fishManager = FishManager.Instance;

        fishRb = gameObject.GetComponent<Rigidbody>();

        splashEffect = GameObject.FindGameObjectWithTag("splash").GetComponent<ParticleSystem>();

        prepareToDrop();
    }

    void Update()
    {
        if (!inWater) return;


        if (transform.position.y < -5)
        {
            GameManager.Instance.onLose();
        }

        if (gameObject.tag == "fish_11")
        {
            GameManager.Instance.onWin();
        }
    }

    public void prepareToDrop()
    {
        fishRb = GetComponent<Rigidbody>();
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

}
