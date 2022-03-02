using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using DG.Tweening;

namespace CasinoElement
{
    public class SlotManager : MonoBehaviour
    {
        public UISlotItem[] slotItems = new UISlotItem[3];

        public Sprite[] img_slots;

        public Image img_reflection;

        public int[] result = new int[3];

        #region Time Setting
        public float delayTime = 0.5f;
        public float rollingTime = 1.5f;
        //public float showSlotTime = 0.5f;
        #endregion

        public int[] itemPoint = new int[5] { 25, 50, 100, 150, 250 };
        public int multipleValue = 3;

        public AudioClip[] sound_Spins;
        public AudioClip[] sound_match;

        private void Awake()
        {
            for (int i = 0; i < slotItems.Length; i++)
            {
                slotItems[i].Init(this);
            }
        }

        public void ResetInit()
        {
            for (int i = 0; i < 3; i++)
            {
                slotItems[i].ResetSlot();
            }
        }

        public void StartRoll()
        {
            img_reflection.enabled = true;
            for (int i = 0; i < slotItems.Length; i++)
            {
                slotItems[i].ResetSlot();
            }

            //if(GlobalSetting.Instance.isFirstCollectToItem() == true)
            //{
            //    for (int i = 0; i < 3; i++)
            //    {
            //        result[i] = 4;
            //        slotItems[i].StartRoll(result[i]);
            //    }
            //}
            //else
            {
                for (int i = 0; i < 3; i++)
                {
                    int rndNum = Random.Range(0, 100);
                    if (rndNum < 25)
                    {
                        result[i] = 0;
                    }
                    else if (rndNum < 50)
                    {
                        result[i] = 1;
                    }
                    else if (rndNum < 75)
                    {
                        result[i] = 2;
                    }
                    else if (rndNum < 95)
                    {
                        result[i] = 3;
                    }
                    else
                    {
                        result[i] = 4;
                    }

                    slotItems[i].StartRoll(result[i]);
                }
            }
        }

        public void EndRoll(int idx)
        {
            img_reflection.enabled = false;

            GameData.Coins += GetEarnMoneyFromSlot(idx);

            for (int i = 0; i < slotItems.Length; i++)
            {
                if (slotItems[i].IsScroll == true) { return; }
            }

            StartCoroutine(iEndRoll());
        }
        IEnumerator iEndRoll()
        {
            yield return new WaitForSeconds(0.2f);

            if (CheckMatchState() == true)
            {
                GameData.Coins += (GetEarnTotalMoneyFromSlot() * 2 / 3);
            }

            ShowEarnScore();

            if (CheckMatchState())
            {
                transform.GetComponent<AudioSource>().clip = sound_match[1];
                transform.GetComponent<AudioSource>().Play();
            }
            else
            {
                transform.GetComponent<AudioSource>().clip = sound_match[0];
                transform.GetComponent<AudioSource>().Play();
            }

            if (GetEarnTotalMoneyFromSlot() < 100)
            {
                FindObjectOfType<UICasinoPanel>().rewardParticles[0].SetActive(true);
            }
            else
            {
                FindObjectOfType<UICasinoPanel>().rewardParticles[1].SetActive(true);
            }

            yield return new WaitForSeconds(1f);

            CasinoCommon.isPlayingAnim = false;

            Debug.LogError("Show Spin Button");
            FindObjectOfType<UICasinoPanel>().OnFinishCasinoElements();
        }

        public void ShowEarnScore()
        {
            FindObjectOfType<UICasinoPanel>().ShowEarnCoins(GetEarnTotalMoneyFromSlot());
        }

        public bool CheckMatchState()
        {
            int firstVal = result[0];
            for (int i = 1; i < 3; i++)
            {
                if (result[i] != firstVal)
                    return false;
            }

            return true;
        }

        private int GetEarnMoneyFromSlot(int idx)
        {
            int earnMoney = 0;

            earnMoney = itemPoint[result[idx]];

            //if (CheckMatchState() == true)
            //{
            //    earnMoney = earnMoney * multipleValue;
            //}

            return earnMoney;
        }

        private int GetEarnTotalMoneyFromSlot()
        {
            int earnMoney = 0;

            for (int i = 0; i < 3; i++)
            {
                earnMoney += itemPoint[result[i]];
            }

            if (CheckMatchState() == true)
            {
                earnMoney = earnMoney * multipleValue;
            }

            return earnMoney;
        }

        public void Play_Slot()
        {
            CasinoCommon.isPlayingAnim = true;
            StartRoll();
        }

        //public void ShowSlotPanel()
        //{
        //    transform.GetComponent<RectTransform>().DOScale(new Vector3(1, 1, 1), 0.5f);
        //}

        //public void HideSlotPanel()
        //{
        //    transform.GetComponent<RectTransform>().DOScale(new Vector3(0.5f, 0.5f, 1), 0.5f);
        //}

        public void ShowSlot_Event()
        {
            Play_Slot();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
                ShowSlot_Event();
        }
    }

}
