using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CasinoElement
{
    public class BingoController : MonoBehaviour
    {
        public static BingoController instance;

        public BingoBoard board1, board2, board3, board4;

        private int[] generatedNumbers = new int[20];

        public int GeneratedNumber;

        private int index;

        private Animator anim;

        private AudioSource audioSource;
        public AudioClip bingoOpenSound;
        void Awake()
        {
            instance = this;
            index = 0;

            anim = this.GetComponent<Animator>();

            CasinoCommon.queueCount = 0;

            audioSource = transform.GetComponent<AudioSource>();
        }

        private void Start()
        {
            ResetBingoBoard();

            //board1.GenerateBingoBoard();
            //board2.GenerateBingoBoard();
            //board3.GenerateBingoBoard();
            //board4.GenerateBingoBoard();
        }

        public void StartDraw()
        {
            audioSource.clip = bingoOpenSound;
            audioSource.PlayDelayed(1.4f);
            anim.Play("LuckyHighlight", 1, 0f);
        }

        public void GenerateBingoNumber()
        {
            GeneratedNumber = Random.Range(1, 22);

            for (int i = 0; i < generatedNumbers.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    while (GeneratedNumber == generatedNumbers[j])
                    {
                        GeneratedNumber = Random.Range(1, 22);
                    }
                }
            }

            generatedNumbers[index] = GeneratedNumber;
            index++;

            CheckNumber(GeneratedNumber);
        }

        public void CheckNumber(int number)
        {
            board1.CheckBingoNumber(number);
            board2.CheckBingoNumber(number);
            board3.CheckBingoNumber(number);
            board4.CheckBingoNumber(number);
        }

        public void ResetBingoBoard()
        {
            CasinoCommon.queueCount--;
            CasinoCommon.isPlayingAnim = false;

            board1.ResetBoard();
            board1.GenerateBingoBoard();

            board2.ResetBoard();
            board2.GenerateBingoBoard();

            board3.ResetBoard();
            board3.GenerateBingoBoard();

            board4.ResetBoard();
            board4.GenerateBingoBoard();

            generatedNumbers = new int[20];
            index = 0;

            RecheckSpins();
        }

        public void DeactivateBoards()
        {
            board1.gameObject.SetActive(true);
            board2.gameObject.SetActive(true);
            board3.gameObject.SetActive(true);
            board4.gameObject.SetActive(true);
        }

        public void RecheckSpins()
        {
            if (CasinoCommon.queueCount <= 0)
            {
                anim.Play("Idle");
                CasinoCommon.queueCount = 0;
            }
            else
            {
                CasinoCommon.isPlayingAnim = true;
                audioSource.clip = bingoOpenSound;
                audioSource.PlayDelayed(1.4f);
                transform.GetComponent<Animator>().Play("LuckyHighlight");
            }
        }

        public void Lightning()
        {
            LightningHit();
        }

        public void LightningHit()
        {
            ScoreManager.Instance.lightingParticle.SetActive(true);
            lighttint.SetActive(true);
        }
        public GameObject lighttint;

        public void CheckRewards()
        {
            board1.CheckReward();
            board2.CheckReward();
            board3.CheckReward();
            board4.CheckReward();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                PlayBingoBoard();
            }
        }

        public void PlayBingoBoard()
        {
            CasinoCommon.queueCount++;

            if (CasinoCommon.queueCount == 1)
            {
                CasinoCommon.isPlayingAnim = true;

                audioSource.clip = bingoOpenSound;
                audioSource.PlayDelayed(1.4f);
                transform.GetComponent<Animator>().Play("LuckyHighlight");
            }
        }

    }

}
