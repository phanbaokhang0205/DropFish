using UnityEngine;
using UnityEngine.EventSystems;

public class Shake : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject fishTank;
    private Vector3 initFishTankPosition;

    public float frequency = 0f;
    public float amplitude = 0.2f;
    private float x;
    void Start()
    {
        initFishTankPosition = fishTank.transform.position;
    }

    void Update()
    {
        x = initFishTankPosition.x + Mathf.Sin(Time.time * frequency) * amplitude;
        fishTank.transform.position = new Vector3(x, initFishTankPosition.y, initFishTankPosition.z);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        frequency = 50f;
        Invoke("setDefault", 1f);

    }

    void setDefault()
    {
        frequency = 0f;
    }


}
