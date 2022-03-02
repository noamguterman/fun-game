using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private AudioSource sfxSource;

    public AudioClip sfxJump;
    public AudioClip sfxGem;
    public AudioClip sfxButton;
    public AudioClip sfxVictory;
    public AudioClip sfxTeleport;
    public AudioClip sfxReward;
    public AudioClip sfxBoost;
    public AudioClip sfxEndJump;
    public AudioClip sfxTimeOver;
    public AudioClip sfxOpenTimePanel;
    public AudioClip sfxProtectorActivate;
    public AudioClip sfxProtectorSave;

    public static SoundManager Instance;

    private void Awake()
    {
        Instance = this;

        sfxSource = GetComponent<AudioSource>();
    }

    public void PlayJumpSFX()
    {
        sfxSource.PlayOneShot(sfxJump);
    }

    public void PlayGemSFX()
    {
        sfxSource.PlayOneShot(sfxGem);
    }

    public void PlayOpenTimePanel()
    {
        sfxSource.PlayOneShot(sfxOpenTimePanel);
    }

    public void PlayButtonSFX()
    {
        sfxSource.PlayOneShot(sfxButton);
    }

    public void PlayTimeOverSFX()
    {
        sfxSource.PlayOneShot(sfxTimeOver);
    }

    public void PlayVictorySFX()
    {
        sfxSource.PlayOneShot(sfxVictory);
    }

    public void PlayTeleportSFX()
    {
        sfxSource.PlayOneShot(sfxTeleport);
    }

    public void PlayRewardSFX()
    {
        sfxSource.PlayOneShot(sfxReward);
    }

    public void PlayBoostSFX()
    {
        sfxSource.PlayOneShot(sfxBoost);
    }

    public void PlayEndJump()
    {
        sfxSource.PlayOneShot(sfxEndJump);
    }

    public void PlayProtectorActivate()
    {
        sfxSource.PlayOneShot(sfxProtectorActivate);
    }

    public void PlayProtectorSave()
    {
        sfxSource.PlayOneShot(sfxProtectorSave);
    }

    public void AddPitch()
    {
        sfxSource.pitch += 1.5f / 10; 
    }

    public void ResetPitch()
    {
        sfxSource.pitch = 1f;
    }

    public void ActiveSounds(bool enabled)
    {
        sfxSource.volume = enabled ? 1 : 0;
    }
}