using System.Collections;
using UnityEngine;

public class Protector : MonoBehaviour
{
    [SerializeField] private float speed;

    public SpriteRenderer SpriteRenderer { get; set; }
    public CircleCollider2D BoxCollider2D { get; set; }

    public bool IsActive { get; private set; }

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        BoxCollider2D = GetComponent<CircleCollider2D>();
        SoundManager.Instance.PlayProtectorActivate();
    }

    private void Start()
    {
        IsActive = true;
    }

    private void Update()
    {
        if (Player.Instance.State == PlayerState.Alive)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, Player.Instance.transform.position.y + 0.25f, 0), speed * Time.deltaTime);
        }
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

        Destroy(gameObject);
    }
}