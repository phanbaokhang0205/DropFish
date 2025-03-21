﻿using Unity.VisualScripting;
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
        Debug.Log(transform.position);

        inWater = false;
        isDropped = false;

        prepareToDrop();
    }

    void Update()
    {
        //Debug.Log(transform.position);
    }

    public void prepareToDrop()
    {
        fishRb = GetComponent<Rigidbody>();

        fishRb.useGravity = false;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        fishRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    public void dropped()
    {
        isDropped = true;
        fishRb.useGravity = true;
        fishRb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
    }

    public void inActive()
    {
        Debug.Log(gameObject.tag+" Out");
        inWater = false;
        isDropped = false;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        fishRb.constraints = RigidbodyConstraints.FreezePosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "water")
        {
            Debug.Log("In Water");
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
