using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer { get; set; }
    public CircleCollider2D BoxCollider2D { get; set; }

    [SerializeField] private float fadeDuration;
    [SerializeField] private float upSpeed;

    private void Awake()
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        BoxCollider2D = GetComponent<CircleCollider2D>();
    }

    public void Dispose()
    {
        BoxCollider2D.enabled = false;
        StartCoroutine(FadeTo(0, fadeDuration));
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = SpriteRenderer.material.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.up, upSpeed * Time.deltaTime);

            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            SpriteRenderer.color = newColor;
            yield return null;
        }

        SpriteRenderer.color = new Vector4(1, 1, 1, 0);

        Destroy(gameObject);
    }
}
