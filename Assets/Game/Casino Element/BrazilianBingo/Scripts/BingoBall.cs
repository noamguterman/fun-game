using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CasinoElement
{
    public class BingoBall : MonoBehaviour
    {
        public bool Main;

        public Text txt;

        public Sprite[] spr;

        private Image img;

        private void OnEnable()
        {
            img = this.GetComponent<Image>();
            Invoke("SetText", 0.01f);
        }

        private void SetText()
        {
            txt.text = BingoController.instance.GeneratedNumber + "";

            if (BingoController.instance.GeneratedNumber <= 7)
            {
                img.sprite = spr[0];
            }
            else if (BingoController.instance.GeneratedNumber > 7 && BingoController.instance.GeneratedNumber <= 14)
            {
                img.sprite = spr[1];
            }
            else if (BingoController.instance.GeneratedNumber > 14)
            {
                img.sprite = spr[2];
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Main)
            {
                txt.text = BingoController.instance.GeneratedNumber + "";
            }
        }
    }

}
