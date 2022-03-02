using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets._Scripts.Tools
{
    public class ActionController : MonoBehaviour
    {
        public string Note;
        public bool isTimeScaleAddiction = true;

        [Header("Перемещения")] public AnimationCurve[] moveCurvesX;
        public AnimationCurve[] moveCurvesY;

        public bool
            isMoveFlag,
            isRandomDirectionX,
            isRandomBetweenXMoveCurves,
            isRandomDirectionY,
            isRandomBetweenYMoveCurves,
            isMoveFromStartPos = true;

        [Header("Повороты")] public AnimationCurve rotateCurve;
        public bool 
            isRotateFlag,
            LeftRightRotation,
            isRotateFromStartAngle;
        [Header("Размеры")] public AnimationCurve[] scaleCurvesX;
        public AnimationCurve[] scaleCurvesY;

        public bool
            samescale,
            isScaleFlag,
            isRandomBetweenXScaleCurves,
            isRandomBetweenYScaleCurves,
            isScaleFromStartPos;

        [Header("Fade")] public AnimationCurve[] fadeCurves;

        public bool isFadeFlag, isRandomBetweenFadeCurves;


        public Coroutine Play(Action onComplete, params KeyValuePair<AnimationCurve, Action<float>>[] curveActionPairs)
        {
            
            return StartCoroutine(_Play(onComplete, curveActionPairs));
        }

        public Coroutine Play(Transform tr, Action onComplete, string graphicTag = "Untagged")
        {
            return StartCoroutine(_Play(onComplete,tr, graphicTag));
        }

        private static IEnumerator _Play(Action onComplete, params KeyValuePair<AnimationCurve, Action<float>>[] curveActionPairs)
        {
            var maxTime = curveActionPairs.Max(pair => pair.Key[pair.Key.length - 1].time);
            //action
            float time = 0;
            while (time < maxTime)
            {
                time += Time.deltaTime;
                foreach (var pair in curveActionPairs)
                {
                    pair.Value(pair.Key[pair.Key.length - 1].time > time
                        ? pair.Key.Evaluate(time)
                        : pair.Key.Evaluate(pair.Key[pair.Key.length - 1].time));
                }
                yield return null;
                
            } 
            if (onComplete != null) onComplete();
        }

        private IEnumerator _Play(Action onComplete, Transform tr, string graphicTag)
        {
            //start values
            var startLocalPos = tr.localPosition;
            var startLocalScale = tr.localScale;
            var startLocalRotation = tr.localEulerAngles;

            //Curves Flag dependence
            var moveCurveX = isMoveFlag ? (isRandomBetweenXMoveCurves ? moveCurvesX.Some() : moveCurvesX[0]) : null;
            if (isMoveFlag && isRandomDirectionX && Random.value > 0.5f)
            {
                var xKeys = new Keyframe[moveCurveX.length];
                for (int i = 0; i < xKeys.Length; i++)
                {
                    xKeys[i] = new Keyframe(moveCurveX[i].time, -moveCurveX[i].value, moveCurveX[i].inTangent,
                        moveCurveX[i].outTangent);
                }
                moveCurveX = new AnimationCurve(xKeys);
            }
            var moveCurveY = isMoveFlag ? (isRandomBetweenYMoveCurves ? moveCurvesY.Some() : moveCurvesY[0]) : null;
            if (isMoveFlag && isRandomDirectionY && Random.value > 0.5f)
            {
                var yKeys = new Keyframe[moveCurveY.length];
                for (int i = 0; i < yKeys.Length; i++)
                {
                    yKeys[i] = new Keyframe(moveCurveY[i].time, -moveCurveY[i].value, moveCurveY[i].inTangent,
                        moveCurveY[i].outTangent);
                }
                moveCurveY = new AnimationCurve(yKeys);
            }
            var scaleCurveX = isScaleFlag
                ? (isRandomBetweenXScaleCurves ? scaleCurvesX.Some() : scaleCurvesX[0])
                : null;
            var scaleCurveY = isScaleFlag && !samescale
                ? (isRandomBetweenYScaleCurves ? scaleCurvesY.Some() : scaleCurvesY[0])
                : null;
            var fadeCurve = isFadeFlag ? (isRandomBetweenFadeCurves ? fadeCurves.Some() : fadeCurves[0]) : null;
            SpriteRenderer[] spriteRenderers = null;
            MaskableGraphic[] graphics = null;
            var rectTransform = tr as RectTransform;
            if (rectTransform != null)
            {
                graphics = isFadeFlag ? UiPanel.GetGraphicsWidthTag(rectTransform, graphicTag) : null;
            }
            else
            {
                spriteRenderers = isFadeFlag ? tr.GetComponentsInChildren<SpriteRenderer>() : null;
            }


            //Curves time
            var moveXTime = (moveCurveX != null ? moveCurveX[moveCurveX.length - 1].time : 0);
            var moveYTime = (moveCurveY != null ? moveCurveY[moveCurveY.length - 1].time : 0);
            var rotateTime = (isRotateFlag ? rotateCurve[rotateCurve.length - 1].time : 0);
            var scaleXTime = (scaleCurveX != null ? scaleCurveX[scaleCurveX.length - 1].time : 0);
            var scaleYTime = (scaleCurveY != null ? scaleCurveY[scaleCurveY.length - 1].time : 0);
            var fadeTime = (fadeCurve != null ? fadeCurve[fadeCurve.length - 1].time : 0);

            var time = Mathf.Max(moveXTime, moveYTime, rotateTime, scaleXTime, scaleYTime, fadeTime);
            var timeAtCurve = 0f;
            var systemTime = DateTime.Now;

            //action
            while (timeAtCurve <= time)
            {
                timeAtCurve += isTimeScaleAddiction?Time.deltaTime: (float)(DateTime.Now - systemTime).TotalSeconds;
                if (isMoveFlag)
                {
                    tr.localPosition = new Vector3(
                        isMoveFromStartPos
                            ? startLocalPos.x + moveCurveX.Evaluate(Mathf.Clamp(timeAtCurve, 0, moveXTime))
                            : 0 + moveCurveX.Evaluate(Mathf.Clamp(timeAtCurve, 0, moveXTime)),
                        isMoveFromStartPos
                            ? startLocalPos.y + moveCurveY.Evaluate(Mathf.Clamp(timeAtCurve, 0, moveYTime))
                            : 0 + moveCurveY.Evaluate(Mathf.Clamp(timeAtCurve, 0, moveYTime)), startLocalPos.z);
                }
                if (isRotateFlag)
                {
                    if (LeftRightRotation)
                    {
                        tr.localEulerAngles = new Vector3(startLocalRotation.x, startLocalRotation.y,
                            isRotateFromStartAngle?startLocalRotation.z:0 + rotateCurve.Evaluate(Mathf.Clamp(timeAtCurve, 0, rotateTime)));
                    }
                    else
                    {
                        tr.Rotate(Vector3.forward, isRotateFromStartAngle ? startLocalRotation.z : 0 + rotateCurve.Evaluate(Mathf.Clamp(timeAtCurve, 0, rotateTime)));
                    }
                }
                if (isScaleFlag)
                {
                    if (samescale)
                    {
                        tr.localScale =
                            new Vector3(
                                isScaleFromStartPos
                                    ? startLocalScale.x
                                    : 0 + scaleCurveX.Evaluate(Mathf.Clamp(timeAtCurve, 0, scaleXTime)),
                                isScaleFromStartPos
                                    ? startLocalScale.y
                                    : 0 + scaleCurveX.Evaluate(Mathf.Clamp(timeAtCurve, 0, scaleXTime)),
                                tr.localScale.z);
                    }
                    else
                    {
                        tr.localScale =
                            new Vector3(
                                isScaleFromStartPos
                                    ? startLocalScale.x
                                    : 0 + scaleCurveX.Evaluate(Mathf.Clamp(timeAtCurve, 0, scaleXTime)),
                                isScaleFromStartPos
                                    ? startLocalScale.y
                                    : 0 + scaleCurveY.Evaluate(Mathf.Clamp(timeAtCurve, 0, scaleYTime)),
                                tr.localScale.z);
                    }
                }
                if (isFadeFlag)
                {
                    if (spriteRenderers != null)
                    {
                        foreach (var g in spriteRenderers)
                        {
                            g.color = new Color(g.color.r, g.color.g, g.color.b,
                                fadeCurve.Evaluate(Mathf.Clamp(timeAtCurve, 0, fadeTime)));
                        }
                    }
                    if (graphics != null)
                    {
                        foreach (var g in graphics)
                        {
                            g.color = new Color(g.color.r, g.color.g, g.color.b,
                                fadeCurve.Evaluate(Mathf.Clamp(timeAtCurve, 0, fadeTime)));
                        }
                    }
                }
                if (!isTimeScaleAddiction)
                {
                    systemTime = DateTime.Now;
                }
                yield return null;
                
            }
            if (onComplete != null) onComplete();
        }


    }
}
