using UnityEngine;
using DG.Tweening;

public class CameraScript : MonoBehaviour
{
    public static CameraScript Instance;

    [SerializeField] float duration;
    [SerializeField] float strength;
    [SerializeField] int vibrato;
    [SerializeField] float randomness;
    [SerializeField] float zoomOutSize;
    [SerializeField] float zoomInSize;
    [SerializeField] float zoomDuration;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void shake()
    {
        transform.DOShakePosition(
            duration: duration,       // Thời gian rung
            strength: strength,       // Độ mạnh rung theo Vector3
            vibrato: vibrato,          // Số lần rung mỗi giây
            randomness: randomness       // Ngẫu nhiên hướng rung
        );
    }

    public void zoomOut()
    {
        GetComponent<Camera>().DOOrthoSize(zoomOutSize, zoomDuration).SetEase(Ease.InOutQuad);
    }

    public void zoomIn()
    {
        GetComponent<Camera>().DOOrthoSize(zoomInSize, zoomDuration).SetEase(Ease.InOutQuad);
    }
}
