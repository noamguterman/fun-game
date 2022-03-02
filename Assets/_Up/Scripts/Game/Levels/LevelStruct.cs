using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class LevelStruct : MonoBehaviour
{
    [HideInInspector] public int Id;

    [HideInInspector] public bool IsTriggered;

    public bool IsLastLevelStruct
    {
        get;
        private set;
    }

    public bool IsClosedStruct
    {
        get
        {
            return isClosedStruct;
        }
    }

    public bool IsSimpleStruct
    {
        get
        {
            return isSimpleStruct;
        }
    }

    public bool HasSolidHorizontalLine
    {
        get
        {
            return hasSolidHorizontalLine;
        }
    }

    public bool HasTelepor
    {
        set;
        get;
    }

    public bool IsTransitionPlatform
    {
        get
        {
            return isTransitionPlatform;
        }
    }

    public Transform CameraBind
    {
        private set;
        get;
    }

    public Transform ParticleBind
    {
        private set;
        get;
    }

    public EndlessLevelPart LevelPart
    {
        get
        {
            return transform.parent.GetComponent<EndlessLevelPart>();
        }
    }

    [SerializeField] private bool isClosedStruct;
    [SerializeField] private bool isSimpleStruct;
    [SerializeField] private bool hasSolidHorizontalLine;
    [SerializeField] private bool isTransitionPlatform;

    private TextMeshPro structText;
    private BoxCollider2D boxCollider2D;

    private List<Transform> vertivalLines;
    private List<Transform> horizontalLines;

    private void Awake()
    {

        boxCollider2D = GetComponent<BoxCollider2D>();
        structText = GetComponentInChildren<TextMeshPro>();

        CameraBind = transform.Find("CameraBind");

        vertivalLines = transform.GetComponentsInChildren<Transform>()
                        .Where(t => t.CompareTag(GameConfig.VERTICAL_LINE_TAG) || t.CompareTag(GameConfig.DISPOSABLE_VERTICAL_LINE_TAG)).ToList();

        horizontalLines = transform.GetComponentsInChildren<Transform>()
                        .Where(t => t.CompareTag(GameConfig.HORIZONTAL_LINE_TAG) || t.CompareTag(GameConfig.DISPOSABLE_HORIZONTAL_LINE_TAG)).ToList();
        
        SetVerticalLinesEnabled(false);
    }

    private void Start()
    {
        structText.fontSizeMax = GameConfig.POINTS_TEXT_SIZE;
    }

    public void DeactiveTrigger()
    {
        IsTriggered = true;
    }

    public void ShowPoints(string points)
    {
        SetTextPosition();

        StartCoroutine(StartShowPoints(points));
    }

    public void SetVerticalLinesEnabled(bool enabled)
    {
        foreach (var vertLine in vertivalLines)
        {
            if (vertLine.GetComponent<DisposableLine>())
            {
                if (vertLine.GetComponent<DisposableLine>().IsActive)
                {
                    vertLine.GetComponent<BoxCollider2D>().enabled = enabled;
                }
            }
            else
            {
                if (enabled)
                {
                    BoxCollider2D lineCollider = vertLine.GetComponent<BoxCollider2D>();
                    BoxCollider2D playerCollider = Player.Instance.CollisionsHandler.BoxCollider2D;

                    lineCollider.enabled = true;

                    if (lineCollider.bounds.Intersects(playerCollider.bounds))
                    {
                        lineCollider.enabled = false;

                        StartCoroutine(DelayedLineActivation(lineCollider));
                    }
                    else
                    {
                        vertLine.GetComponent<BoxCollider2D>().enabled = enabled;
                    }
                }
                else
                {
                    vertLine.GetComponent<BoxCollider2D>().enabled = enabled;
                }
            }
        }
    }

    private IEnumerator DelayedLineActivation(BoxCollider2D lineCollider)
    {
        yield return new WaitForSeconds(0.5f);

        lineCollider.enabled = true;
    }

    public void InitAsLastStruct()
    {
        IsLastLevelStruct = true;

        ParticleBind = transform.Find("ParticleBind");
    }

    private void SetTextPosition()
    {
        var playerPosiionX = Player.Instance.transform.position.x;
        var playerDirection = Player.Instance.Movement.Direction;

        var structTextPosition = structText.transform.position;

        if (playerDirection == Direction.Left)
        {
            structTextPosition.x = playerPosiionX + GameConfig.POINTS_TEXT_OFFSET;
        }
        else
        {
            structTextPosition.x = playerPosiionX - GameConfig.POINTS_TEXT_OFFSET;

        }

        structText.rectTransform.position = structTextPosition;
    }

    private IEnumerator StartShowPoints(string points)
    {
        structText.transform.localScale = Vector3.zero;
        structText.text = points;
        structText.transform.DOScale(1, 0.15f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.65f);
        structText.transform.DOScale(0, 0.35f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(0.35f);
        structText.text = string.Empty;
    }

    public bool IsContains(List<Transform> structHorizontalLines)
    {
        return horizontalLines.Intersect(structHorizontalLines).Count() > 0;
    }

    public List<Transform> GetHorizontalLines()
    {
        return horizontalLines;
    }

    public void SetStructPointTextColor(Color color)
    {
        structText.color = color;
    }
}