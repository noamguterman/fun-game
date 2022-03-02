using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace CasinoElement
{
    public class WheelScript : MonoBehaviour
    {
        public GameObject wheel_Root;

        private int[] wheelPoints = new int[21] { 25, 70, 35, 50, 25, 10, 50, 100, 1000, 100, 50, 10, 25, 50, 35, 70, 25, 200, 1, 500, 1 };

        public Material lampMat;

        private AudioSource audioSource;
        public AudioClip wheelRotateSound;
        void Start()
        {
            audioSource = transform.GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                StartWheel();

            lampMat.SetColor("_Color", Color.Lerp(Color.yellow, Color.blue, Mathf.PingPong(Time.time, 0.5f)));
        }
        public void StartWheel()
        {
            ShowWheelPanel();

            CasinoCommon.queueCount++;
            if (CasinoCommon.queueCount == 1)
            {
                Invoke("RotateWheel", 0.5f);
                Debug.Log(CasinoCommon.queueCount);
            }
        }

        private void ShowWheelPanel()
        {
            transform.GetComponent<Animator>().SetInteger("Param", 1);
        }

        private void HideWheelPanel()
        {
            transform.GetComponent<Animator>().SetInteger("Param", 2);
        }

        private void RotateWheel()
        {
            audioSource.pitch = 1;
            audioSource.Play();
            StartCoroutine(cor_RotateWheel());
        }

        int increaseVal = 0;
        private float getTargetAngle()
        {
            float angle = 0;

            int rnd = Random.Range(0, 21);
            increaseVal = wheelPoints[rnd];

            angle += 360 * 3 + rnd * 17.142857f;

            return angle;
        }

        private bool isFistPlayer()
        {
            if (PlayerPrefs.HasKey("NotFirst"))
            {
                return false;
            }
            else
            {
                PlayerPrefs.SetInt("NotFirst", 1);
                return true;
            }
        }

        IEnumerator cor_RotateWheel()
        {
            CasinoCommon.isPlayingAnim = true;


            if (isFistPlayer())
            {
                wheel_Root.transform.DOLocalRotate(new Vector3(0, 1080 + 17.142857f * 8, 0), 3, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);
                increaseVal = 1000;
            }
            else
            {
                wheel_Root.transform.DOLocalRotate(new Vector3(0, getTargetAngle(), 0), 3, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);
            }

            Sequence seq = DOTween.Sequence()
                .AppendInterval(2f);
                //.Append(audioSource.DOPitch(0.3f, 1f));

            yield return new WaitForSeconds(3);
            audioSource.Stop();

            yield return new WaitForSeconds(0.5f);

            ScoreManager.Instance.Cash += increaseVal * ScoreManager.Instance.Multiplier;
            ScoreManager.Instance.Save();
            Debug.Log(increaseVal);

            if (increaseVal == 1000)
            {
                Debug.Log("Jackpot");
                CasinoCommon.jackPotCount++;
                ChallengeController.Instance.SetJackpotCount();

                yield return new WaitForSeconds(0.5f);
                //UIManager.Instance.ShowJackpotEffects();
                //yield return new WaitForSeconds(1.5f);
            }

            else if (increaseVal < 100)
            {
                ScoreManager.Instance.rewardParticles[0].SetActive(true);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                //UIManager.Instance.ShowBigWinEffects();
                //yield return new WaitForSeconds(1.5f);

                ScoreManager.Instance.rewardParticles[1].SetActive(true);
            }

            if(increaseVal * ScoreManager.Instance.Multiplier >= 500)
                ChallengeController.Instance.SetMultiple500Count();

            CasinoCommon.isPlayingAnim = false;

            CasinoCommon.queueCount--;
            if (CasinoCommon.queueCount < 0)
                CasinoCommon.queueCount = 0;


            if (CasinoCommon.queueCount <= 0)
            {
                HideWheelPanel();
            }
            else if (CasinoCommon.queueCount > 0)
                RotateWheel();
        }
    }

}
