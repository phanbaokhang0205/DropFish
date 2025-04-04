using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    private GameManager gameMng;

    void Start()
    {
        gameMng = GameManager.Instance;
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        textMeshPro.text = "Score: " + gameMng.score.ToString();
    }
}
