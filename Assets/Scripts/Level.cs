using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField] private bool isLocked;
    [SerializeField] private int levelIndex;
    [SerializeField] private TextMeshProUGUI playButtonTMP;
    [SerializeField] private Button levelBtn;

    void Start()
    {
        isLocked = false;
        levelIndex = int.Parse(gameObject.name);
        SetStateOflevel();
    }

    void Update()
    {

    }

    public void SetStateOflevel()
    {
        if (levelIndex <= PlayerPrefsManager.GetUnLockedLevel())
        {
            isLocked = true;

        }
        if (isLocked)
        {
            //disable button
            levelBtn.interactable = true;
        } else
        {
            //enable button
            levelBtn.interactable = false;
        }
    }

    public void test()
    {
        //playButtonTMP.text = levelIndex.ToString();
        PlayerPrefsManager.SetCurrentLevel(levelIndex);
        Debug.Log("CURRENT: " + PlayerPrefsManager.GetCurrentLevel());
        MainMenu.Instance.PlayAdventureMode();
    }

}
