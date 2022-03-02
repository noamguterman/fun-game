using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Assets._Scripts.Tools
{
    public static class Feature
    {
        private static bool _invoces;
        private static GameObject gameObject;
        private static MonoBehaviour _thiBehaviour;

        static Feature()
        {
            gameObject = new GameObject("Feature");
            Object.DontDestroyOnLoad(gameObject);
            _thiBehaviour=gameObject.AddComponent<Help>();
        }

        public static void ResetParameters(this Animator animator)
        {
            AnimatorControllerParameter[] parameters = animator.parameters;
            for (int i = 0; i < parameters.Length; i++)
            {
                AnimatorControllerParameter parameter = parameters[i];
                switch (parameter.type)
                {
                    case AnimatorControllerParameterType.Int:
                        animator.SetInteger(parameter.name, parameter.defaultInt);
                        break;
                    case AnimatorControllerParameterType.Float:
                        animator.SetFloat(parameter.name, parameter.defaultFloat);
                        break;
                    case AnimatorControllerParameterType.Bool:
                        animator.SetBool(parameter.name, parameter.defaultBool);
                        break;
                }
            }
        }
        public static void RecursiveColorSet(this Transform objTransform, Color color)
        {
            var sr = objTransform.GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = color;
            for (var i = 0; i < objTransform.childCount; i++)
            {
                RecursiveColorSet(objTransform.GetChild(i), color);
            }
        }

        public static void ToAll<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if(action==null)return;
            foreach (var el in collection)
            {
                action(el);
            }
        }

        public static Coroutine Invoke(float time, Action action)
        {
            return _thiBehaviour.StartCoroutine(_Invoke(time, action));
        }
        public static Coroutine Invoke(object @obje, Action action)
        {
            return _thiBehaviour.StartCoroutine(_Invoke(obje, action));
        }
        //public static void Invoke

        public static void StopAllInvokes()
        {
            _thiBehaviour.StopCoroutine("_Invoke");
        }

        public static void MoveTowardsTo(this Transform transform, Vector3 position, float speed, Action action = null)
        {
            _thiBehaviour.StartCoroutine(_MoveTowardsTo(transform, position, speed, action));
        }

        public static object SaveInvoke(this Delegate @delegate, params object[] @params)
        {
            if (@delegate == null) return null;
            return @delegate.DynamicInvoke(@params);
        }
        public static void LocalyMoveTowardsTo(this Transform transform, Vector3 position, float speed,
            Action action = null)
        {
            _thiBehaviour.StartCoroutine(_LocalyMoveTowardsTo(transform, position, speed, action));
        }

        public static void Normolize(this Transform transform, Vector3 angle, float speed)
        {
            _thiBehaviour.StartCoroutine(_Normolize(transform, angle, speed));
        }
        public static void LerpTo(this Transform transform, Vector3 position, float speed, float delta, Action action = null)
        {
            _thiBehaviour.StartCoroutine(_LerpTo(transform, position, speed, delta, action));
        }

        public static void LookAt2D(this Transform transform, Vector3 position)
        {

            Quaternion rotation = Quaternion.LookRotation
                (position - transform.position, transform.TransformDirection(Vector3.up));
            transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
            if (transform.position.x < position.x)
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);
        }

        public static void RecursivelyAction(this Transform transform, Action<Transform> action)
        {
            action(transform);
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).RecursivelyAction(action);
            }
        }

        public static void WaitFor(this Transform transform, Predicate<Transform> predicate, Action action)
        {
            _thiBehaviour.StartCoroutine(_WaitFor(transform, predicate, action));
        }

        #region Eumerators

        private static IEnumerator _Normolize(Transform transform, Vector3 angle, float speed)
        {
            while (transform&&Quaternion.Angle(transform.localRotation, Quaternion.Euler(angle)) > 0.0001)
            {
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation,
                    Quaternion.Euler(angle), speed * Time.deltaTime);
                yield return null;
            }
            
        }
        private static IEnumerator _Invoke(float time, Action action)
        {
            var dTime = Time.time + time;
            while (dTime > Time.time)
            {
                yield return null;
            }
            try
            {
                action();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        private static IEnumerator _Invoke(object @object, Action action)
        {
            yield return @object;
            try
            {
                action();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        private static IEnumerator _MoveTowardsTo(Transform transform, Vector3 position, float speed, Action action)
        {
            while (transform&&transform.position != position)
            {
                transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
                yield return null;
            }
            if (transform && action != null) action();
        }
        private static IEnumerator _LerpTo(Transform transform, Vector3 position, float speed, float delta, Action action)
        {
            while (transform && (transform.position - position).sqrMagnitude> delta)
            {
                transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
                yield return null;
            }
            if (action != null&& transform) action();
        }
        private static IEnumerator _LocalyMoveTowardsTo(Transform transform, Vector3 position, float speed, Action action = null)
        {
            while (transform.localPosition != position)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, position, speed * Time.deltaTime);
                yield return null;
            }
            if (action != null) action();
        }

        private static IEnumerator _WaitFor(Transform transform, Predicate<Transform> @for, Action action )
        {
            if(@for==null|| !transform) yield break;
            while (transform&&!@for(transform))
            {
                yield return null;
            }
            if (action != null&& transform) action();
        }
#endregion
        public static void Shuffle<T>(this IList<T> list)
        {
            Random random = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static IList<T> Distinct<T>(this IList<T> firstList, IList<T> secondList)
        {
            return firstList.Where(el => !secondList.Contains(el)).ToList();
        }

        public static T Some<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                //Debug.Log("Empty");
                return default(T);
            }
            //System.Random random = new System.Random();
            //return list[random.Next(0, list.Count)];
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        public static Vector3 Mask(this Vector3 target, Vector3 mask){
            return new Vector3(target.x*mask.x,target.y*mask.y,target.z*mask.z);
        }
        public static Vector3 Mask(this Vector3 target, float x,float y, float z)
        {
            return new Vector3(target.x * x, target.y * y, target.z * z);
        }
        public static Color Mask(this Color target, float r, float g, float b, float a)
        {
            return new Color(target.r * r, target.g * g, target.b * b, target.a * a);
        }
        public static Color SetAlpha(this Color target, float a)
        {
            return new Color(target.r , target.g, target.b, a);
        }

        public static T Some<T>(params T[] col)
        {
            return col.Some();
        }

        public static float Angle2D(Vector3 start, Vector3 end)
        {
            float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * 180 / Mathf.PI - 90;
            if (0.0f > angle)
                angle += 360.0f;
            return angle;
        }

        public class Help : MonoBehaviour { }
    }
}
