using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AdventureModeAnim : MonoBehaviour
{
    [SerializeField] GameObject panelSetting;
    [SerializeField] GameObject frameSetting;
    private Image panel_image;
    private RectTransform panel_rt;
    private RectTransform frame_rt;
    private bool isOpenSetting;


    void Start()
    {
        isOpenSetting = false;
        panel_image = panelSetting.GetComponent<Image>();
        panel_rt = panelSetting.GetComponent<RectTransform>();
        frame_rt = frameSetting.GetComponent<RectTransform>();
    }

    void Update()
    {
        
    }

    public void handleSetting()
    {
        isOpenSetting = !isOpenSetting;
        if (isOpenSetting)
        {
            MainMenu.Instance
            .handleSettingAnim(isOpenSetting, panel_rt, frame_rt, panel_image)
            .OnComplete(() =>
            {
                GameManager.Instance.onPauseAdventure();
            });
        }
        else
        {
            GameManager.Instance.onResumeAdventure();
            MainMenu.Instance.handleSettingAnim(isOpenSetting, panel_rt, frame_rt, panel_image);
        }
    }
}
