using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MonoBehaviour
{
    [SerializeField] private List<int> skipedLinesPerTouch;

    private int currentStep;

    public void MoveToNextPoint()
    {
        if (currentStep < skipedLinesPerTouch.Count)
        {
            int amountOfLinesToSkip = skipedLinesPerTouch[currentStep];

            Vector3 position = transform.position;

            position.y += amountOfLinesToSkip * GameConfig.MAGIC_BALL_STEP_HEIGHT;
            transform.position = position;

            currentStep++;
        }
    }
}