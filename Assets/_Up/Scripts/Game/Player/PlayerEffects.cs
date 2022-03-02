using System.Collections;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private TrailRenderer jumpTrail;
    [SerializeField] private ParticleSystem trail;
    [SerializeField] private ParticleSystem teleportTrail;
    [SerializeField] private ParticleSystem splash;

    private Gradient gradient;

    public void SetColor(Color color)
    {
        var trailColor = trail.colorOverLifetime;
        var teleportTrailColor = teleportTrail.colorOverLifetime;

        gradient = new Gradient();

        gradient.SetKeys(
        new GradientColorKey[] 
        {
            new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f)
        },
        new GradientAlphaKey[] 
        {
            new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f)
        });

        trailColor.color = gradient;
        teleportTrailColor.color = gradient;
    }

    public void SetTrailActive(bool enabled)
    {
        trail.gameObject.SetActive(enabled);
    }

    public void SetTeleportTrailActive(bool enabled)
    {
        teleportTrail.gameObject.SetActive(enabled);
    }

    public void CreateSplash()
    {
        StartCoroutine(CreateSplashCoroutine());
    }

    private IEnumerator CreateSplashCoroutine()
    {
        var splashObject = Instantiate(splash.gameObject, Player.Instance.transform.position - 0.1f * Vector3.up, Quaternion.identity).GetComponent<ParticleSystem>();

        var splashColor = splashObject.colorOverLifetime;
        splashColor.color = gradient;

        yield return new WaitWhile(splashObject.IsAlive);

        Destroy(splashObject.gameObject);
    }

    float lastPosY;
    private void Update()
    {
        if(transform.position.y == lastPosY)
        {
            jumpTrail.enabled = false;
        }
        else
        {
            lastPosY = transform.position.y;
            jumpTrail.enabled = true;
        }
    }
}