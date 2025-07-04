﻿using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] GameObject line;
    [SerializeField] GameObject water;

    private FishManager fishManager;
    
    private Vector3 touchPosition;
    private Touch touch;

    private Renderer rendLine;
    private Vector3 targetCenter;
    private bool isDrop;

    //fishTank
    private Renderer waterSize;
    public static float waterHeight;
    public static float waterWidth;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        fishManager = FishManager.Instance;

        /// fish tank props
        Vector3 waterScreenPos = Camera.main.WorldToScreenPoint(water.transform.position);
        waterHeight = waterScreenPos.y * 2;
        waterWidth = waterScreenPos.x;

        waterSize = water.GetComponent<Renderer>();

        /// init position
        touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(waterWidth, waterHeight, 10));
        fishManager.CreateFish(touchPosition);

        ////line to drop fish
        rendLine = line.GetComponent<Renderer>();
        targetCenter = fishManager.chosenFish.GetComponent<Collider>().bounds.center;

        isDrop = true;

        setLinePosition();
    }

    void Update()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Playing && Input.touchCount > 0 )
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

                fishManager.MoveFish(touchPosition);
                checkPosition();
                setLinePosition();

            }
            else if (touch.phase == TouchPhase.Ended && isDrop && !LevelManager.Instance.isWaiting)
            {
                isDrop = false;
                fishManager.DropFish();
                line.SetActive(false);
                GameManager.Instance.updateStep();
                GameManager.Instance.isCancleDelayDrop = true;
                Invoke(nameof(delayDrop), 1f);
            }
        }
        

    }
    void delayDrop()
    {
        fishManager.chosenFish = null;
        isDrop = true;
        touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, waterHeight, 10));
        fishManager.CreateFish(touchPosition);
        setLinePosition();
        GameManager.Instance.isCancleDelayDrop = false;

    }

    [SerializeField] List<GameObject> ab;

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

    public void setLinePosition()
    {
        float objectHeight = rendLine.bounds.size.y;
        Vector3 newLinePosition = line.transform.position;
        newLinePosition.y = targetCenter.y - (objectHeight / 2);
        newLinePosition.x = fishManager.chosenFish.transform.position.x;
        line.transform.position = newLinePosition;
        line.SetActive(true);
    }

}
