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
        fishRb.constraints =  RigidbodyConstraints.FreezePositionZ |
                              RigidbodyConstraints.FreezeRotationZ |
                              RigidbodyConstraints.FreezeRotationY;

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
