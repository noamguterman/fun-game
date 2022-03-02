using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager1 : MonoBehaviour
{
    public AudioSource jumpSFX;
    public AudioSource gameOverSFX;
    public AudioSource victorySFX;
    public AudioSource powerupSFX;
    public AudioSource buttonSFX;

    public static SoundManager1 instance;

    public AudioSource moneySource, soundSource, timerSource;

    //Soundtracks
    public AudioClip GameOver, Coin, SmallReward, BigReward, MoneyCounter, Bankruptcy, Gem, Revive, ButtonClick, OpenPopup, ClosePopup;

    public AudioClip[] Jump;

    private void Awake()
    {
        instance = this;
    }

    public void PlayPowerupSound()
    {
        powerupSFX.Play();
    }

    public void JumpSound()
    {
        int rand = Random.Range(0, Jump.Length);
        soundSource.PlayOneShot(Jump[rand]);
    }
    public void PlayCoinSound()
    {
        soundSource.PlayOneShot(Coin);
    }
    public void PlaySmallReward()
    {
        soundSource.PlayOneShot(SmallReward);
    }
    public void PlayBigReward()
    {
        soundSource.PlayOneShot(BigReward);
    }

    public void PlaySoundSource(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }
    public void PlayBankruptSound()
    {
        soundSource.PlayOneShot(Bankruptcy);
    }
    public void PlayGameOverSound()
    {
        soundSource.PlayOneShot(GameOver);
    }
    public void PlayGemSound()
    {
        soundSource.PlayOneShot(Gem);
    }
    public void PlayReviveSound()
    {
        soundSource.PlayOneShot(Revive);
    }

    public void PlayOpenPopupSound()
    {
        soundSource.PlayOneShot(OpenPopup);
    }

    public void PlayClosePopupSound()
    {
        soundSource.PlayOneShot(ClosePopup);
    }

    public void PlayTimerSound()
    {
        timerSource.Play();
    }

    public void StopTimerSound()
    {
        if(timerSource != null)
            timerSource.Stop();
    }

    public void PlayButtonClickSound()
    {
        soundSource.PlayOneShot(ButtonClick);
    }

    //Money sound
    public void PlayMoneySound()
    {
        if (moneySource.clip == null)
        {
            moneySource.clip = MoneyCounter;
        }

        if (!moneySource.isPlaying)
        {
            moneySource.Play();
        }
    }
    public void StopMoneySound()
    {
        moneySource.Stop();
        moneySource.clip = null;
    }
}
