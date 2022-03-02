using UnityEngine;

namespace Assets._Scripts.Tools
{
    public class Ease
    {
        public static float InBack(float t)
        {
            float s = 1.70158f;
            return t * t * ((s + 1) * t - s);
        }
        public static float InBounce(float t)
        {
            return 1 - OutBounce(1 - t);
        }
        public static float InCirc(float t)
        {
            if (t >= 1) return t;
            return -1 * (Mathf.Sqrt(1 - t * t) - 1);
        }
        public static float InCubic(float t)
        {
            return t * t * t;
        }
        public static float InElastic(float t)
        {
            float s = 1.70158f;
            float p = 0f;
            float a = 1.0f;
            if (t == 0) return 0;
            if (t == 1) return 1;
            if (p == 0) p = 1 * .3f;
            if (a < 1)
            {
                a = 1f;
                s = p / 4;
            }
            else s = p / (2 * Mathf.PI) * Mathf.Asin(1 / a);
            return -(a * Mathf.Pow(2, 10 * (t = t - 1)) * Mathf.Sin((t * 1 - s) * (2 * Mathf.PI) / p));
        }
        public static float InExpo(float t)
        {
            return (t == 0) ? 1 : 1 * Mathf.Pow(2, 10 * (t / 1 - 1));
        }
        public static float InOutBack(float t)
        {
            float s = 1.70158f;
            if ((t = t / 0.5f) < 1) return 1 / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s));
            return 1f / 2 * ((t = t - 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2);
        }
        public static float InOutBounce(float t)
        {
            if (t < 1f / 2) return InBounce(t * 2) * .5f;
            return OutBounce(t * 2 - 1) * .5f + 1 * .5f;
        }
        public static float InOutCirc(float t)
        {
            if ((t = t / 0.5f) < 1) return -1f / 2 * (Mathf.Sqrt(1 - t * t) - 1);
            return 1f / 2 * (Mathf.Sqrt(1 - (t = t - 2) * t) + 1);
        }
        public static float InOutCubic(float t)
        {
            if ((t /= 1f / 2) < 1) return 1f / 2 * t * t * t;
            return 1f / 2 * ((t = t - 2) * t * t + 2);
        }
        public static float InOutElastic(float t)
        {
            float s = 1.70158f;
            float p = 0;
            float a = 1;
            if (t == 0) return 0;
            if ((t /= 1f / 2) == 2) return 1;
            if (p == 0) p = 1 * (.3f * 1.5f);
            if (a < 1)
            {
                a = 1;
                s = p / 4;
            }
            else s = p / (2 * Mathf.PI) * Mathf.Asin(1 / a);
            if (t < 1) return -.5f * (a * Mathf.Pow(2, 10 * (t = t - 1)) * Mathf.Sin((t * 1 - s) * (2 * Mathf.PI) / p));
            return a * Mathf.Pow(2, -10 * (t = t - 1)) * Mathf.Sin((t * 1 - s) * (2 * Mathf.PI) / p) * .5f + 1;
        }
        public static float InOutExpo(float t)
        {
            if (t == 0) return 0;
            if (t == 1) return 1;
            if ((t = t / (1 / 2)) < 1) return 1 / 2 * Mathf.Pow(2, 10 * (t - 1));
            return 1 / 2 * (-Mathf.Pow(2, -10 * --t) + 2);
        }
        public static float InOutQuad(float t)
        {
            if ((t /= 1 / 2) < 1) return 1 / 2 * t * t;
            return -1 / 2 * ((--t) * (t - 2) - 1);
        }
        public static float InOutQuart(float t)
        {
            if ((t /= 1 / 2) < 1) return 1 / 2 * t * t * t * t;
            return -1 / 2 * ((t = t - 2) * t * t * t - 2);
        }
        public static float InOutQuint(float t)
        {
            if ((t = t / (1 / 2)) < 1) return (1 / 2) * t * t * t * t * t;
            return 1 / 2 * ((t = t - 2) * t * t * t * t + 2);
        }
        public static float InOutSine(float t)
        {
            return -1 / 2 * (Mathf.Cos(Mathf.PI * t / 1) - 1);
        }
        public static float InQuad(float t)
        {
            return t * t;
        }
        public static float InQuart(float t)
        {
            return t * t * t * t;
        }
        public static float InQuint(float t)
        {
            return t * t * t * t * t;
        }
        public static float InSine(float t)
        {
            return -1 * Mathf.Cos(t / 1 * (Mathf.PI / 2)) + 1;
        }
        public static float OutBack(float t)
        {
            float s = 1.70158f;
            return 1 * ((t = t / 1 - 1) * t * ((s + 1) * t + s) + 1);
        }
        public static float OutBounce(float t)
        {
            if (t < (1 / 2.75))
            {
                return 1 * (7.5625f * t * t);
            }
            else if (t < (2 / 2.75))
            {
                return 1 * (7.5625f * (t = t - (1.5f / 2.75f)) * t + .75f);
            }
            else if (t < (2.5 / 2.75))
            {
                return 1 * (7.5625f * (t = t - (2.25f / 2.75f)) * t + .9375f);
            }
            else
            {
                return 1 * (7.5625f * (t = t - (2.625f / 2.75f)) * t + .984375f);
            }
        }
        public static float OutCirc(float t)
        {
            return 1 * Mathf.Sqrt(1 - (t = t - 1) * t);
        }
        public static float OutCubic(float t)
        {
            return 1 * ((t = t / 1 - 1) * t * t + 1);
        }
        public static float OutElastic(float t)
        {
            float s = 1.70158f;
            float p = 0;
            float a = 1;
            if (t == 0) return 0;
            if (t == 1) return 1;
            if (p == 0) p = 1 * .3f;
            if (a < 1)
            {
                a = 1;
                s = p / 4;
            }
            else s = p / (2 * Mathf.PI) * Mathf.Asin(1 / a);
            return a * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 1 - s) * (2 * Mathf.PI) / p) + 1;
        }
        public static float OutExpo(float t)
        {
            return (t == 1) ? 1 : 1 * (-Mathf.Pow(2, -10 * t / 1) + 1);
        }
        public static float OutQuad(float t)
        {
            return -1 * t * (t - 2);
        }
        public static float OutQuart(float t)
        {
            return -1 * ((t = t / 1 - 1) * t * t * t - 1);
        }
        public static float OutQuint(float t)
        {
            return ((t = t - 1) * t * t * t * t + 1);
        }
        public static float OutSine(float t)
        {
            return 1 * Mathf.Sin(t / 1 * (Mathf.PI / 2));
        }
        public static float Linear(float t)
        {
            return t;
        }
    }
}
