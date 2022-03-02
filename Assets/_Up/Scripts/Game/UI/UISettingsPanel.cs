using UnityEngine;
using UnityEngine.SceneManagement;

public class UISettingsPanel : UIPanel
{
    public void OnBackButtonPressed()
    {
        SoundManager.Instance.PlayButtonSFX();

        UIManager.Instance.ActivatePanel(UIPanelType.GamePanel);
        if(GameManager.Instance.isBuildingtime == true && GameManager.Mode == GameMode.Levels)
        {
            GameManager.Instance.State = GameState.Paused;
        }
        else
        {
            GameManager.Instance.State = GameState.Alive;
        }
    }

    public void OnLevelsButtonPressed()
    {
        SoundManager.Instance.PlayButtonSFX();

        GameManager.Mode = GameMode.Levels;

        SceneManager.LoadScene("Game");

        GameData.CurrentScore = 0;
    }

    public void OnEndlessRunButtonPressed()
    {
        GameData.CurrentScore = 0;

        SoundManager.Instance.PlayButtonSFX();

        GameManager.Mode = GameMode.Endless;

        SceneManager.LoadScene("Game");
    }

    public void OnSprintButtonPressed()
    {
        GameData.CurrentScore = 0;

        SoundManager.Instance.PlayButtonSFX();

        GameManager.Mode = GameMode.Sprint;
    }

    public void OnClearSavesPressed()
    {
        SoundManager.Instance.PlayButtonSFX();

        GameData.CurrentScore = 0;

        PlayerPrefs.DeleteAll();

        SceneManager.LoadScene("Game");
    }
}