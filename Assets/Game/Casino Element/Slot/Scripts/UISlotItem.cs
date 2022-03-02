using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace CasinoElement
{
    public class UISlotItem : MonoBehaviour
    {
        public Ease easeType = Ease.InOutBack;

        public GameObject[] slotObjs;
        Image[] slotImgs;

        Image[] startSlotImg = new Image[3];
        Image[] endSlotImg = new Image[3];

        SlotManager eventSender = null;

        public int curIndex = 0;
        public bool IsScroll = false;

        private void Start()
        {

        }

        public void Init(SlotManager inEventSender)
        {
            eventSender = inEventSender;

            slotImgs = new Image[slotObjs.Length];

            for (int i = 0; i < slotObjs.Length; i++)
            {
                slotImgs[i] = slotObjs[i].transform.Find("slot_item").GetComponent<Image>();
            }

            startSlotImg[0] = slotImgs[slotImgs.Length - 3];
            startSlotImg[1] = slotImgs[slotImgs.Length - 2];
            startSlotImg[2] = slotImgs[slotImgs.Length - 1];
            endSlotImg[0] = slotImgs[0];
            endSlotImg[1] = slotImgs[1];
            endSlotImg[2] = slotImgs[2];

            for (int i = 0; i < slotImgs.Length; i++)
            {
                int ii = Random.Range(0, eventSender.img_slots.Length);
                slotImgs[i].sprite = eventSender.img_slots[ii];
            }
        }

        public void ResetSlot()
        {
            Sprite sp0, sp1, sp2;
            sp0 = endSlotImg[0].sprite;
            sp1 = endSlotImg[1].sprite;
            sp2 = endSlotImg[2].sprite;
            for (int i = 0; i < slotImgs.Length; i++)
            {
                int ii = Random.Range(0, eventSender.img_slots.Length);
                slotImgs[i].sprite = eventSender.img_slots[ii];
            }

            startSlotImg[0].sprite = sp0;
            startSlotImg[1].sprite = sp1;
            startSlotImg[2].sprite = sp2;
        }

        public void StartRoll(int resultIdx)
        {
            IsScroll = true;
            endSlotImg[1].sprite = eventSender.img_slots[resultIdx];

            StartCoroutine("cor_StartRoll");
            GetComponent<AudioSource>().clip = eventSender.sound_Spins[0];
            GetComponent<AudioSource>().Play();
        }

        IEnumerator cor_StartRoll()
        {
            transform.GetComponent<Animator>().SetTrigger("start");
            yield return new WaitForSeconds((curIndex % 3) * eventSender.delayTime + eventSender.rollingTime);
            EndScroll();
        }

        void EndScroll()
        {
            IsScroll = false;
            GetComponent<AudioSource>().clip = eventSender.sound_Spins[1];
            GetComponent<AudioSource>().Play();

            eventSender.EndRoll(curIndex);
        }
    }

}
