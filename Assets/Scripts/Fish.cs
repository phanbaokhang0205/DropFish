using Unity.VisualScripting;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private FishManager fishManager;
    private Rigidbody fishRb;

    public bool inWater;
    public bool isDropped;

    void Start()
    {
        fishManager = FishManager.Instance;

        fishRb = gameObject.GetComponent<Rigidbody>();

        prepareToDrop();
    }

    void Update()
    {
        if (inWater)
        {
            dropped();
        }
    }

    public void prepareToDrop()
    {
        fishRb = GetComponent<Rigidbody>();
        fishRb.useGravity = false;
        inWater = false;
        isDropped = false;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        fishRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        fishRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

    }

    public void dropped()
    {
        isDropped = true;
        fishRb.useGravity = true;
        fishRb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
    }

    public void setState()
    {
        prepareToDrop();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "water")
        {
            inWater = true;
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



}
