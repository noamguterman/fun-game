using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace CasinoElement
{
    public class CardController : MonoBehaviour
    {
        // cards images
        public GameObject card_root;
        public Sprite[] cardSprites;
        public Sprite backSpr;
        public Image[] cards;
        private int resultValue;
        private int[] idxs = new int[3];

        public AudioClip flipCard1Sound;
        public AudioClip flipCard2Sound;
        public AudioClip flipCard3Sound;
        public AudioClip rewardSound;

        private AudioSource audioSource;

        public Text txt_result;
        string[] CardResult_Str = new string[7]{
            "ACE HIGH",
            "PAIR",
            "THREE OF A KIND!",
            "STRAIGHT",
            "FLUSH",
            "STRAIGHT FLUSH",
            "NOTHING WON"
        };

        void Start()
        {
            foreach (Image item in cards)
            {
                item.sprite = backSpr;
            }

            audioSource = transform.GetComponent<AudioSource>();
            txt_result.text = "";
        }

        private void OnEnable()
        {
            txt_result.text = "";
        }

        private void OnDisable()
        {
            
        }

        public void Start_Cards()
        {
            ShowCardAnim();
        }

        private void ShowCardAnim()
        {
            cards[0].GetComponent<Animator>().Rebind();
            cards[1].GetComponent<Animator>().Rebind();
            cards[2].GetComponent<Animator>().Rebind();

            CasinoCommon.isPlayingAnim = true;
            SetBackCards();

            CreateRandomCards();
            showPanel();
        }

        private void SetBackCards()
        {

        }

        public void OnFinsihCardAnim()
        {
            CasinoCommon.isPlayingAnim = false;

            GameData.Coins += resultValue;
            FindObjectOfType<UICasinoPanel>().ShowEarnCoins(resultValue);

            if (resultValue > 0 && resultValue < 100)
            {
                FindObjectOfType<UICasinoPanel>().rewardParticles[0].SetActive(true);
            }
            else if (resultValue >= 100)
            {
                FindObjectOfType<UICasinoPanel>().rewardParticles[1].SetActive(true);
            }
        }

        IEnumerator startAnimation()
        {
            yield return new WaitForSeconds(0.5f);
            cards[0].GetComponent<Animator>().SetFloat("speed", 1.0f);
            audioSource.PlayOneShot(flipCard1Sound);

            yield return new WaitForSeconds(0.25f);
            cards[0].sprite = cardSprites[idxs[0]];
            cards[1].GetComponent<Animator>().SetFloat("speed", 1.0f);
            audioSource.PlayOneShot(flipCard2Sound);

            yield return new WaitForSeconds(0.25f);
            cards[1].sprite = cardSprites[idxs[1]];

            yield return new WaitForSeconds(0.5f);
            cards[2].GetComponent<Animator>().SetFloat("speed", 1.0f);
            audioSource.PlayOneShot(flipCard3Sound);

            yield return new WaitForSeconds(0.25f);
            cards[2].sprite = cardSprites[idxs[2]];
            if (resultValue > 0)
                audioSource.PlayOneShot(rewardSound);

            yield return new WaitForSeconds(1);
            OnFinsihCardAnim();
            txt_result.text = result;

            yield return new WaitForSeconds(2f);
            FindObjectOfType<UICasinoPanel>().ShowBackButton();
            yield return new WaitForSeconds(1f);
            FindObjectOfType<UICasinoPanel>().OnFinishCasinoElements();

            txt_result.text = "";
            foreach (Image item in cards)
            {
                item.sprite = backSpr;
            }
            yield return null;
        }

        string result = string.Empty;
        private void CreateRandomCards()
        {
            int i;
            int[] keys = new int[3];
            int[] values = new int[3];
            for (i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    keys[i] = UnityEngine.Random.Range(0, 10000) % 4;
                    values[i] = UnityEngine.Random.Range(0, 10000) % 5;
                }
                else if(i==1)
                {
                    while (true)
                    {
                        keys[i] = UnityEngine.Random.Range(0, 10000) % 4;
                        values[i] = UnityEngine.Random.Range(0, 10000) % 5;
                        if (keys[i] == keys[i - 1] && values[i] == values[i - 1]) continue;
                        else break;
                    }
                }
                else if (i == 2)
                {
                    while (true)
                    {
                        keys[i] = UnityEngine.Random.Range(0, 10000) % 4;
                        values[i] = UnityEngine.Random.Range(0, 10000) % 5;
                        if ((keys[i] == keys[0] && values[i] == values[0]) || (keys[i] == keys[1] && values[i] == values[1])) continue;
                        else break;
                    }
                }
                idxs[i] = keys[i] * 5 + values[i];
            }
            if (isFlash(keys))
            {
                if (isStright(values))
                {
                    resultValue = 1500;
                    result = CardResult_Str[5];
                }
                else
                {
                    resultValue = 100;
                    result = CardResult_Str[4];
                }
            }
            else
            {
                if (is3Cards(values))
                {
                    resultValue = 500;
                    result = CardResult_Str[2];
                }
                else if (isStright(values))
                {
                    resultValue = 250;
                    result = CardResult_Str[3];
                }
                else if (isPair(values))
                {
                    resultValue = 50;
                    result = CardResult_Str[1];
                }
                else if (isAce(values))
                {
                    resultValue = 25;
                    result = CardResult_Str[0];
                }
                else
                {
                    resultValue = 0;
                    result = CardResult_Str[6];
                }
            }
        }

        private int GetResultValue()
        {
            return 0;
        }

        private void showPanel()
        {
            StartCoroutine(startAnimation());
        }

        private bool isFlash(int[] keys)
        {
            int[] temp = new int[] { keys[0], keys[0], keys[0] };
            return temp.SequenceEqual(keys);
        }

        private bool isStright(int[] values)
        {
            int min = values.Min();
            int[] temp = new int[] { Mathf.Abs(values[0] - min), Mathf.Abs(values[1] - min), Mathf.Abs(values[2] - min) };
            return temp.SequenceEqual(new int[] { 0, 1, 2 })
                || temp.SequenceEqual(new int[] { 0, 2, 1 })
                || temp.SequenceEqual(new int[] { 1, 0, 2 })
                || temp.SequenceEqual(new int[] { 1, 2, 0 })
                || temp.SequenceEqual(new int[] { 2, 0, 1 })
                || temp.SequenceEqual(new int[] { 2, 1, 0 });
        }

        private bool is3Cards(int[] values)
        {
            int[] temp = new int[] { values[0], values[0], values[0] };
            return temp.SequenceEqual(values);
        }

        public bool isPair(int[] values)
        {
            if (values[1] - values[0] == 0 || values[2] - values[0] == 0) return true;
            else if (values[2] - values[1] == 0 || values[0] - values[1] == 0) return true;
            else if (values[1] - values[2] == 0 || values[0] - values[2] == 0) return true;
            else return false;
        }

        public bool isAce(int[] values)
        {
            return Array.IndexOf(values, 4) >= 0 ? true : false;
        }

    }

}
