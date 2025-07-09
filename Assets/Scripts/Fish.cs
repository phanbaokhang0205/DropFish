using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] public bool isDropped;
    [SerializeField] public bool isJustMerge;

    public bool isDataOfLevel = false;
    ParticleSystem splashEffect;
    ParticleSystem mergeSplashEffect;
    bool isFlashing;
    private Tween flashTween;


    void Start()
    {
        fishRb = gameObject.GetComponent<Rigidbody>();
        fishRb.mass = 0;
        fishManager = FishManager.Instance;

        splashEffect = GameObject.FindGameObjectWithTag("splash").GetComponent<ParticleSystem>();
        mergeSplashEffect = GameObject.FindGameObjectWithTag("mergeSplash").GetComponent<ParticleSystem>();

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
                GameManager.Instance.onLoseAdventure();
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
        isJustMerge = false;
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
            foreach (GameObject obs in breakableObs)
            {
                obs.SetActive(false);
            }
            breakableObs.Clear();
        }
        LevelManager.Instance.setObstacleAmount();

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
            if (!isJustMerge)
            {
                setsplashEffect(splashEffect);
                AudioManager.Instance.PlayWaterDrop();
            }
            else {
                setsplashEffect(mergeSplashEffect);
            }

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
        //fishManager.MergeFish(gameObject, collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "BreakableObstacle")
        {
            breakableObs.Remove(collision.gameObject);

        }
    }

    void setsplashEffect(ParticleSystem effect)
    {
        effect.transform.position = transform.position;
        effect.Play();
    }

    public void StartFlash()
    {
        if (!isFlashing && mats.Length > 1)
        {
            isFlashing = true;

            Material mat1 = mats[1];

            // Tạo tween alpha lặp lại
            flashTween = DOTween.ToAlpha(
                () => mat1.color,
                x => mat1.color = x,
                flashAlpha,
                flashInterval
            )
            .SetLoops(-1, LoopType.Yoyo) // Lặp vô hạn, qua lại
            .SetEase(Ease.Linear);       // Giữ tốc độ đều
        }
    }

    public void StopFlash()
    {
        if (isFlashing)
        {
            isFlashing = false;

            if (flashTween != null && flashTween.IsActive())
            {
                flashTween.Kill();
            }

            // Reset alpha về 0
            Material mat1 = mats[1];
            Color color = mat1.color;
            color.a = 0f;
            mat1.color = color;
        }
    }
    
}
