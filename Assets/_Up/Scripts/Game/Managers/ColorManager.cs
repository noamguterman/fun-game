using Assets._Scripts.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;

    [SerializeField] private List<Color> availableLinesColors;
    [SerializeField] private List<Color> availableBackgroundColors;
    [SerializeField] private List<Color> availablePointsTextColors;
    [SerializeField] private List<Color> finalWinExplosionColors;
    [SerializeField] private List<Color> finalParticlesColors;

    public Color CurrentLineColor { get; private set; }
    public Color CurrentBackgroundColor { get; private set; }
    public Color CurrentWinExplosionColor { get; private set; }

    private static int currentColorState;

    private void Awake()
    {
        Instance = this;
    }

    public void SetLevelColors()
    {
        CurrentLineColor = availableLinesColors.ElementAt(currentColorState);
        CurrentBackgroundColor = availableBackgroundColors.ElementAt(currentColorState);
        CurrentWinExplosionColor = finalWinExplosionColors.ElementAt(currentColorState);

        Color structsPointTextColor = availablePointsTextColors.ElementAt(currentColorState);

        Level.Instance.SetLinesColor(CurrentLineColor);
        Level.Instance.SetStructPointsTextsColor(structsPointTextColor);

        CameraFollow.Instance.SetBackgroundColor(CurrentBackgroundColor);

        Player.Instance.PlayerEffects.SetColor(CurrentLineColor);

        UIManager.Instance.SetPanelsBackgroundsColor();

        SetFinalExplosionColor();
        SetFinalParticlesColor();

        currentColorState++;

        if (currentColorState == availableLinesColors.Count)
        {
            currentColorState = 0;
        }
    }

    private void SetFinalExplosionColor()
    {
        GameObject finalExplosionObject = FXManager.Instance.GetFinalExplosionObject();

        SpriteRenderer secondCircle = finalExplosionObject.transform.GetChild(1).GetComponent<SpriteRenderer>();

        secondCircle.color = CurrentWinExplosionColor;
    }

    private void SetFinalParticlesColor()
    {
        Color finalParticlesColor = finalParticlesColors.ElementAt(currentColorState);

        List<ParticleSystem> finalParticles = FXManager.Instance.GetFinalParticles();

        foreach (var particle in finalParticles)
        {
            particle.startColor = finalParticlesColor;
        }
    }

    public void SetEndlessLevelColors(List<EndlessLevelPart> endlessLevelParts)
    {
        CurrentLineColor = availableLinesColors.ElementAt(currentColorState);
        Color levelBackgroundColor = availableBackgroundColors.ElementAt(currentColorState);

        foreach (var levelPart in endlessLevelParts)
        {
            levelPart.SetLinesColor(CurrentLineColor);
        }

        CameraFollow.Instance.SetBackgroundColor(levelBackgroundColor);

        Player.Instance.PlayerEffects.SetColor(CurrentLineColor);

        currentColorState++;

        if (currentColorState == availableLinesColors.Count)
        {
            currentColorState = 0;
        }
    }
}