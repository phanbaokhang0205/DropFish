using TMPro;
using UnityEngine;

public class NextFishText : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private FishPooler fishPooler;

    void Start()
    {
        fishPooler = FishPooler.Instance;
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        int nextFish = fishPooler.rs + 1;
        textMeshPro.text = "Next fish: " + nextFish.ToString();
    }
}
