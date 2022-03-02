using UnityEngine;

namespace Assets._Scripts.Tools
{
    public class Border : MonoBehaviour
    {
        public enum BorderSide
        {
            Left,
            Right,
            Center,
            Top,
            LeftOffScene,
            RightOffScene
        };
        public BorderSide Side;
        public GameObject Delta;

        private void OnEnable()
        {
            Vector3 positon = Camera.main.ScreenToWorldPoint(Vector3.zero);
            var left = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
            var buttom = Camera.main.ScreenToWorldPoint(Vector3.zero).y;
            var right = -left;
            var deltaX = Mathf.Abs(gameObject.transform.position.x - Delta.transform.position.x);
            switch (Side)
            {
                case BorderSide.Left:
                    transform.position = new Vector3(positon.x, transform.position.y, transform.position.z);
                    break;
                case BorderSide.Right:
                    transform.position = new Vector3(positon.x * (-1), transform.position.y, transform.position.z);
                    break;
                case BorderSide.Center:
                    transform.position = new Vector3(positon.x, positon.y, transform.position.z);
                    break;
                case BorderSide.LeftOffScene:
                    transform.position = new Vector3(left - deltaX, transform.position.y, transform.position.z);
                    break;
                case BorderSide.RightOffScene:
                    transform.position = new Vector3(right + deltaX, transform.position.y, transform.position.z);
                    break;
                case BorderSide.Top:
                    transform.position = new Vector3(transform.position.x, -buttom, transform.position.z);
                    break;
            }
        }
    }
}
