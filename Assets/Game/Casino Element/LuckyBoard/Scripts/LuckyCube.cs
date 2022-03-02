using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace CasinoElement
{
    public class LuckyCube : MonoBehaviour
    {
        public static LuckyCube Instance;

        public GameObject[] panels;

        public Sprite[] panel_sprites;

        private int CurrentPanel, LastPanel, Rand;

        private bool Activated;
        public bool Stopped;

        private float timer;

        public bool blinkFlag = true;

        private AudioSource audioSource;
        //public AudioClip luckyOpenSound;
        //public AudioClip luckyResultSound;
        public AudioClip luckyChange;
        public AudioClip luckySelect;
        public AudioClip luckyWin;
        public AudioClip luckyBankrupt;
        public AudioClip luckyJackpot;

        private float itemBlinkTime = 0.3f;

        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            Activated = true;
            Stopped = false;
            timer = 0;

            transform.GetComponent<Image>().sprite = panel_sprites[0];

            audioSource = transform.GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (blinkFlag)
            {
                timer += Time.deltaTime;

                if (timer > itemBlinkTime)
                {
                    SelectRandomPanel();
                    timer = 0;
                }
            }
        }
        private void SelectRandomPanel()
        {
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].GetComponent<LuckyImage>().OffImage();
            }

            Rand = Random.Range(0, panels.Length);

            while (Rand == LastPanel)
            {
                Rand = Random.Range(0, panels.Length);
            }

            panels[Rand].GetComponent<LuckyImage>().OnImage();
            Activated = true;
        }

        void BlinkItem()
        {
            panels[Rand].GetComponent<Animator>().Play("IdleBlink");
            panels[Rand].transform.SetAsLastSibling();
        }
        IEnumerator StopBingo()
        {
            yield return null;
            if (panels[Rand].tag == "15")
            {
                GameData.Coins += 15;
                FindObjectOfType<UICasinoPanel>().ShowEarnCoins(15);
                FindObjectOfType<UICasinoPanel>().rewardParticles[0].SetActive(true);
                audioSource.PlayOneShot(luckyWin);
            }
            else if (panels[Rand].tag == "25")
            {
                GameData.Coins += 25;
                FindObjectOfType<UICasinoPanel>().ShowEarnCoins(25);
                FindObjectOfType<UICasinoPanel>().rewardParticles[0].SetActive(true);
                audioSource.PlayOneShot(luckyWin);
            }
            else if (panels[Rand].tag == "100")
            {
                GameData.Coins += 100;
                FindObjectOfType<UICasinoPanel>().ShowEarnCoins(100);
                FindObjectOfType<UICasinoPanel>().rewardParticles[1].SetActive(true);
                audioSource.PlayOneShot(luckyWin);
            }
            else if (panels[Rand].tag == "150")
            {
                GameData.Coins += 150;
                FindObjectOfType<UICasinoPanel>().ShowEarnCoins(150);
                FindObjectOfType<UICasinoPanel>().rewardParticles[1].SetActive(true);
                audioSource.PlayOneShot(luckyWin);
            }
            else if (panels[Rand].tag == "250")
            {
                GameData.Coins += 250;
                FindObjectOfType<UICasinoPanel>().ShowEarnCoins(250);
                FindObjectOfType<UICasinoPanel>().rewardParticles[1].SetActive(true);
                audioSource.PlayOneShot(luckyWin);
            }
            else if (panels[Rand].tag == "Jackpot")
            {
                GameData.Coins += 500;
                FindObjectOfType<UICasinoPanel>().ShowEarnCoins(500);
                FindObjectOfType<UICasinoPanel>().rewardParticles[1].SetActive(true);
                audioSource.PlayOneShot(luckyJackpot);
            }
            else if (panels[Rand].tag == "Bankrupt")
            {
                GameData.Coins -= GameData.Coins / 4;
                audioSource.PlayOneShot(luckyBankrupt);
                //SoundManager.instance.PlayBankruptSound();
            }
        }

        public void ShowLuckyCubePanel()
        {
            transform.GetComponent<Image>().sprite = panel_sprites[1];
            StartCoroutine(BoardLightEffect());
        }

        IEnumerator BoardLightEffect()
        {
            for (int i = 0; i < 15; i++)
            {
                yield return new WaitForSeconds(0.1f);
                transform.GetComponent<Image>().sprite = panel_sprites[0];
                yield return new WaitForSeconds(0.1f);
                transform.GetComponent<Image>().sprite = panel_sprites[1];
            }
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(0.25f);
                transform.GetComponent<Image>().sprite = panel_sprites[0];
                yield return new WaitForSeconds(0.25f);
                transform.GetComponent<Image>().sprite = panel_sprites[1];
            }
        }

        public void HideLuckyCubePanel()
        {
            FindObjectOfType<UICasinoPanel>().OnFinishCasinoElements();
            transform.GetComponent<Image>().sprite = panel_sprites[0];
        }

        IEnumerator cor_ShowLuckyCubeEffect()
        {
            yield return null;
            CasinoCommon.isPlayingAnim = true;
            itemBlinkTime = 0.15f;
            yield return new WaitForSeconds(2.5f);
            blinkFlag = false;
            itemBlinkTime = 0.3f;
            BlinkItem();
            yield return new WaitForSeconds(1f);

            yield return StartCoroutine(StopBingo());

            yield return new WaitForSeconds(1.5f);

            blinkFlag = true;
            CasinoCommon.isPlayingAnim = false;

            HideLuckyCubePanel();
        }

        void ShowLuckyCubeEffect()
        {
            StartCoroutine(cor_ShowLuckyCubeEffect());

            //audioSource.clip = luckyOpenSound;
            audioSource.PlayDelayed(0.5f);
            //Invoke("PlayLuckyResultSound", audioSource.clip.length + 0.5f);
        }
        void PlayLuckyResultSound()
        {
            //audioSource.PlayOneShot(luckyResultSound);
        }

        public void PlayLuckyCube()
        {
            ShowLuckyCubePanel();

            CasinoCommon.isPlayingAnim = true;
            ShowLuckyCubeEffect();
        }
    }

}
