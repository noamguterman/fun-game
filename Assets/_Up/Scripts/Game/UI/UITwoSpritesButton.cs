using UnityEngine;
using UnityEngine.UI;

public class UITwoSpritesButton : MonoBehaviour
{
    public Button Button
    {
        get;
        private set;
    }

    [SerializeField] private Sprite firstSprite;
    [SerializeField] private Sprite secondSprite;

    private bool isFirstSpriteActive = true;

    private Button button;

    private void Awake()
    {
        Button = GetComponent<Button>();

        Button.onClick.AddListener(SwitchSprite);
    }

    public void UpdateState(bool state)
    {
        isFirstSpriteActive = state;

        Button.image.sprite = isFirstSpriteActive ? firstSprite : secondSprite;
    }

    private void SwitchSprite()
    {
        Button.image.sprite = isFirstSpriteActive? secondSprite : firstSprite;

        isFirstSpriteActive = !isFirstSpriteActive;
    }
}