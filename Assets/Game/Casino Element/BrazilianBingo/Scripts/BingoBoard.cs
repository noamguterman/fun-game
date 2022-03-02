using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CasinoElement
{
    public class BingoBoard : MonoBehaviour
    {
        //Bingo board values
        public Text[] bingoBoardNumbers;
        private int[] bingoBoardValues = new int[15];

        List<int> generatedNumbers = new List<int>();

        private bool[] bingoBoardValuesMatch = new bool[15];
        private Image[] bingoBoardImage = new Image[15];

        public ParticleSystem _100Coins, _250Coins;

        public Animator bingowin;

        private bool[] RowReward;

        private int RandSpace, Frees;

        private bool row1, row2, row3;

        private Animator anim;

        private bool RowOne, RowTwo, Bingo;

        // Start is called before the first frame update
        void Awake()
        {
            RowReward = new bool[5];
            anim = this.GetComponent<Animator>();

            //One or two rows as a reward
            RowOne = false;
            RowTwo = false;

            //Which rows have been completed;
            row1 = false;
            row2 = false;
            row3 = false;
            generatedNumbers = new List<int>(new int[15]);
        }

        public void GenerateBingoBoard()
        {
            //Generate initial values
            for (int i = 0; i < bingoBoardNumbers.Length; i++)
            {
                bingoBoardValues[i] = Random.Range(1, 22);
            }

            //Make sure numbers don't repeat
            for (int i = 0; i < bingoBoardNumbers.Length; i++)
            {
                while (generatedNumbers.Contains(bingoBoardValues[i]))
                {
                    bingoBoardValues[i] = Random.Range(1, 22);
                }
                generatedNumbers[i] = bingoBoardValues[i];
            }

            for (int i = 0; i < bingoBoardNumbers.Length; i++)
            {
                bingoBoardNumbers[i].text = bingoBoardValues[i] + "";
                bingoBoardImage[i] = bingoBoardNumbers[i].GetComponentInParent<Image>();
                bingoBoardValuesMatch[i] = false;

                bingoBoardImage[i].color = new Color32(255, 255, 255, 0);
                bingoBoardNumbers[i].GetComponent<Text>().color = new Color32(0, 0, 0, 255);
            }
        }

        public void CheckBingoNumber(int GeneratedNumber)
        {
            FeedbackText.instance.ShowFeedback(GeneratedNumber + "");

            for (int i = 0; i < bingoBoardNumbers.Length; i++)
            {
                if (GeneratedNumber == bingoBoardValues[i])
                {
                    bingoBoardValuesMatch[i] = true;
                    bingoBoardImage[i].color = new Color32(0, 139, 255, 255);
                    bingoBoardNumbers[i].GetComponent<Text>().color = new Color32(255, 255, 255, 255);
                }
            }

            //If row completed
            if (bingoBoardValuesMatch[0] && bingoBoardValuesMatch[1] && bingoBoardValuesMatch[2] && bingoBoardValuesMatch[3] && bingoBoardValuesMatch[4])
            {
                if (!row1)
                {
                    if (!RowOne)
                    {
                        RowOne = true;

                        for (int i = 0; i < bingoBoardNumbers.Length; i++)
                        {
                            if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4)
                            {
                                bingoBoardImage[i].color = new Color32(255, 0, 0, 255);
                                bingoBoardNumbers[i].GetComponent<Text>().color = new Color32(255, 255, 255, 255);
                            }
                        }

                        //anim.Play("BingoCardBlink", -1, 0f);
                    }
                    else if (RowOne && !RowTwo)
                    {
                        RowTwo = true;

                        for (int i = 0; i < bingoBoardNumbers.Length; i++)
                        {
                            if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4)
                            {
                                bingoBoardImage[i].color = new Color32(255, 0, 0, 255);
                                bingoBoardNumbers[i].GetComponent<Text>().color = new Color32(255, 255, 255, 255);
                            }
                        }

                        //anim.Play("BingoCardBlink", -1, 0f);
                    }
                    else if (RowOne && RowTwo)
                    {

                    }
                    row1 = true;
                }
            }
            if (bingoBoardValuesMatch[5] && bingoBoardValuesMatch[6] && bingoBoardValuesMatch[7] && bingoBoardValuesMatch[8] && bingoBoardValuesMatch[9])
            {
                if (!row2)
                {
                    if (!RowOne)
                    {
                        RowOne = true;

                        for (int i = 0; i < bingoBoardNumbers.Length; i++)
                        {
                            if (i == 5 || i == 6 || i == 7 || i == 8 || i == 9)
                            {
                                bingoBoardImage[i].color = new Color32(255, 0, 0, 255);
                                bingoBoardNumbers[i].GetComponent<Text>().color = new Color32(255, 255, 255, 255);
                            }
                        }

                        //anim.Play("BingoCardBlink", -1, 0f);
                    }
                    else if (RowOne && !RowTwo)
                    {
                        RowTwo = true;

                        for (int i = 0; i < bingoBoardNumbers.Length; i++)
                        {
                            if (i == 5 || i == 6 || i == 7 || i == 8 || i == 9)
                            {
                                bingoBoardImage[i].color = new Color32(255, 0, 0, 255);
                                bingoBoardNumbers[i].GetComponent<Text>().color = new Color32(255, 255, 255, 255);
                            }
                        }

                        //anim.Play("BingoCardBlink", -1, 0f);
                    }
                    else if (RowOne && RowTwo)
                    {

                    }
                    row2 = true;
                }
            }
            if (bingoBoardValuesMatch[10] && bingoBoardValuesMatch[11] && bingoBoardValuesMatch[12] && bingoBoardValuesMatch[13] && bingoBoardValuesMatch[14])
            {
                if (!row3)
                {
                    if (!RowOne)
                    {
                        RowOne = true;

                        for (int i = 0; i < bingoBoardNumbers.Length; i++)
                        {
                            if (i == 10 || i == 11 || i == 12 || i == 13 || i == 14)
                            {
                                bingoBoardImage[i].color = new Color32(255, 0, 0, 255);
                                bingoBoardNumbers[i].GetComponent<Text>().color = new Color32(255, 255, 255, 255);
                            }
                        }

                        //anim.Play("BingoCardBlink", -1, 0f);
                    }
                    else if (RowOne && !RowTwo)
                    {
                        RowTwo = true;

                        for (int i = 0; i < bingoBoardNumbers.Length; i++)
                        {
                            if (i == 10 || i == 11 || i == 12 || i == 13 || i == 14)
                            {
                                bingoBoardImage[i].color = new Color32(255, 0, 0, 255);
                                bingoBoardNumbers[i].GetComponent<Text>().color = new Color32(255, 255, 255, 255);
                            }
                        }

                        //anim.Play("BingoCardBlink", -1, 0f);
                    }
                    else if (RowOne && RowTwo)
                    {

                    }
                    row3 = true;
                }
            }


            //If full bingo
            if (bingoBoardValuesMatch[0] && bingoBoardValuesMatch[1] && bingoBoardValuesMatch[2] && bingoBoardValuesMatch[3] && bingoBoardValuesMatch[4] && bingoBoardValuesMatch[5] && bingoBoardValuesMatch[6] && bingoBoardValuesMatch[7] && bingoBoardValuesMatch[8] && bingoBoardValuesMatch[9] && bingoBoardValuesMatch[10] && bingoBoardValuesMatch[11] && bingoBoardValuesMatch[12] && bingoBoardValuesMatch[13] && bingoBoardValuesMatch[14])
            {
                if (!Bingo)
                {
                    Bingo = true;

                    for (int i = 0; i < bingoBoardNumbers.Length; i++)
                    {
                        bingoBoardImage[i].color = new Color32(255, 0, 0, 255);
                        bingoBoardNumbers[i].GetComponent<Text>().color = new Color32(255, 255, 255, 255);
                    }
                }
            }
        }

        private void Coin100()
        {
            _100Coins.gameObject.SetActive(true);
            //_100Coins.Clear();
            //_100Coins.Play();
        }

        private void Coin250()
        {
            _250Coins.gameObject.SetActive(true);
            //_250Coins.Clear();
            //_250Coins.Play();
        }

        private void CoinBingo()
        {
            _250Coins.gameObject.SetActive(true);
            //_2500Coins.Clear();
            //_2500Coins.Play();
        }

        public void CheckReward()
        {
            if (Bingo)
            {
                //Bingo game over
                ScoreManager.Instance.Cash += 2500 * ScoreManager.Instance.Multiplier;
                ScoreManager.Instance.Save();

                CasinoCommon.jackPotCount++;
                ChallengeController.Instance.SetJackpotCount();
                ChallengeController.Instance.SetMultiple500Count();

                bingowin.Play("BingoShow");
                //UIManager.Instance.ShowJackpotEffects();
                Invoke("CoinBingo", 1.5f);
            }
            else if (!Bingo && RowTwo && RowOne)
            {
                //Row 3
                ScoreManager.Instance.Cash += 250 * ScoreManager.Instance.Multiplier;
                ScoreManager.Instance.Save();
                if(250 * ScoreManager.Instance.Multiplier >= 500)
                    ChallengeController.Instance.SetMultiple500Count();

                //SoundManager.instance.PlayRewardSound();
                bingowin.Play("BingoTwoRows");
                Invoke("Coin250", 1.5f);
            }
            else if (!Bingo && !RowTwo && RowOne)
            {
                //Row 3
                ScoreManager.Instance.Cash += 50 * ScoreManager.Instance.Multiplier;
                ScoreManager.Instance.Save();


                //SoundManager.instance.PlayRewardSound();
                bingowin.Play("BingoRow");
                Invoke("Coin100", 1.5f);
            }
            else
            {

            }
        }

        public void UnlockBoard()
        {
            GenerateBingoBoard();
        }

        public void ResetAllBoards()
        {
            BingoController.instance.ResetBingoBoard();

            RowOne = false;
            RowTwo = false;
            Bingo = false;
        }

        public void ResetBoard()
        {
            Bingo = false;

            //One or two rows as a reward
            RowOne = false;
            RowTwo = false;

            //Which rows have been completed;
            row1 = false;
            row2 = false;
            row3 = false;

            for (int i = 0; i < bingoBoardValuesMatch.Length; i++)
            {
                bingoBoardImage[i] = bingoBoardNumbers[i].GetComponentInParent<Image>();
                bingoBoardValuesMatch[i] = false;
            }
        }
    }

}
