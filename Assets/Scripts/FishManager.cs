﻿using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
/**
    * Bug:
    * 1. Khi merge nhiều cá liên tục thì lỗi cá bị mất
 */


public class FishManager : MonoBehaviour
{
    public static FishManager Instance;

    public GameObject chosenFish;
    public Fish fishScript;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        
    }

    private void Update()
    {
        if (fishScript.inWater)
        {
            chosenFish = null;
        }
    }

    public void CreateFish(Vector3 spawnPosition)
    {

        chosenFish = FishPooler.Instance.GetFish(spawnPosition, null);
        fishScript = chosenFish.GetComponent<Fish>();
        fishScript.prepareToDrop();
    }

    public void PrepareFish(Vector3 touchPosition)
    {
        chosenFish.transform.position = touchPosition;

    }

    public void DropFish()
    {
        fishScript.dropped();
    }

    public void MoveFish(Vector3 touchPosition)
    {
        chosenFish.transform.position = touchPosition;
        fishScript.prepareToDrop();
    }

    public void MergeFish(GameObject collision1, GameObject collision2)
    {
        if (collision1.tag == collision2.tag)
        {
            int level = GetFishLevel(collision1.tag) - 1;
            Debug.Log("level của 2 con vừa rồi: " + level);

            if (collision1.transform.position.y > collision2.transform.position.y)
            {
                collision1.GetComponent<Fish>().setState();
                collision2.GetComponent<Fish>().setState();
                FishPooler.Instance.ReturnFish(collision1, level);
                FishPooler.Instance.ReturnFish(collision2, level);
                

                GameObject nextFish = FishPooler.Instance.GetFish(collision2.transform.position, level+1);
            }
        }
    }

    private int GetFishLevel(string tag)
    {
        string[] parts = tag.Split('_');
        return int.Parse(parts[1]);
    }

}
