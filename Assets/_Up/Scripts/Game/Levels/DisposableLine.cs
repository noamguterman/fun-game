using System.Collections;
using UnityEngine;

public class DisposableLine : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; set; }
    public BoxCollider2D BoxCollider2D { get; set; }

    public bool IsActive { get; private set; }

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        IsActive = true;
    }

    public void Dispose()
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;

        StartCoroutine(FadeTo(0, 0.25f));
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = SpriteRenderer.material.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            SpriteRenderer.color = newColor;
            yield return null;
        }

        SpriteRenderer.color = new Vector4(1, 1, 1, 0);
        BoxCollider2D.enabled = false;
    }

    public void ResetDispose()
    {
        IsActive = true;
        SpriteRenderer.color = ColorManager.Instance.CurrentLineColor;
        BoxCollider2D.enabled = true;
    }
}