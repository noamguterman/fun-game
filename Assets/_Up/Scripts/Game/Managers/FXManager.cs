using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance;

    [SerializeField] private GameObject finalExplosion;
    [SerializeField] private ParticleSystem finalParticle;
    [SerializeField] private ParticleSystem lastStructParticle;
    [SerializeField] private List<ParticleSystem> winParticles;
    [SerializeField] private ParticleSystem boostParticles;

    private void Awake()
    {
        Instance = this;
    }

    public void ActivateFinalExplosion()
    {
        finalExplosion.transform.position = Player.Instance.transform.position + Vector3.up / 5;

        finalExplosion.SetActive(true);
    }

    public void SetLastStructParticlePosition()
    {
        LevelStruct lastStruct = Level.Instance.GetLastStruct();

        Vector3 particlePosition = lastStruct.ParticleBind.transform.position + Vector3.up / 3;

        lastStructParticle.transform.position = particlePosition;

        lastStructParticle.gameObject.SetActive(true);
    }

    public void PlayWinVFX(Vector3 position)
    {
        position += Vector3.up;

        transform.position = position;

        foreach (var particle in winParticles)
        {
            particle.Play();
        }

        //SoundManager.Instance.PlayVictorySFX();

        ActivateFinalExplosion();
    }

    public void PlayBoostVFX(Vector3 position)
    {
        position += Vector3.up;
        boostParticles.transform.position = position;
        boostParticles.gameObject.SetActive(true);

        SoundManager.Instance.PlayVictorySFX();
    }

    public List<ParticleSystem> GetFinalParticles()
    {
        return winParticles;
    }

    public GameObject GetFinalExplosionObject()
    {
        return finalExplosion;
    }
}