using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] GameObject[] levels;
    public TextMeshProUGUI stepMoveText;

    private GameObject currentObj;
    private int levelIndex;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            levelIndex = MainMenu.Instance.levelIndex;
            currentObj = Instantiate(levels[levelIndex]);
            foreach (Transform child in levels[levelIndex].transform)
            {
                if (child.CompareTag("stepMove"))
                {
                    stepMoveText.text = child.name;
                    break;
                }
            }
        }
        
    }

    void Update()
    {
        stepMoveText.text = GameManager.Instance.step.ToString();
    }

    public void onLoadNextLevel()
    {
        FishPooler.Instance.ClearPool();
        FishPooler.Instance.InitializePool();
        levelIndex++;
        if (currentObj)
            Destroy(currentObj);

        if (levelIndex >= levels.Length)
        {
            levelIndex = 0;
        }
        currentObj = Instantiate(levels[levelIndex]);

    }

}
