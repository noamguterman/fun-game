using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI nextLevelText;

    [SerializeField] private Image progressImage;

    private int structsToComleteLevel;
    private int structsAlreadyFinished;

    public void Init()
    {
        structsToComleteLevel = Level.Instance.GetStructsCount();

        currentLevelText.text = (GameManager.Instance.LevelsCompleted + 1).ToString();
        nextLevelText.text = (GameManager.Instance.LevelsCompleted + 2).ToString();
    }

    public void UpdateProgress()
    {
        int lastStructID = Player.Instance.CollisionsHandler.LastTriggeredStruct.Id;

        if (lastStructID > structsAlreadyFinished)
        {
            structsAlreadyFinished = lastStructID;
        }

        float progress = (structsAlreadyFinished + 1) / (float)structsToComleteLevel;

        progressImage.fillAmount = progress;
    }

    public void SetActive(bool enabled)
    {
        gameObject.SetActive(enabled);
    }
}