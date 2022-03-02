using System;
using System.Collections;
using System.Linq;
using Assets._Scripts.Tools.Base.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts.Tools
{
    public class UiPanel : MonoBehaviour, IPanel, IInitializable
    {
        public RectTransform PanelTransform;
        public ActionController ShowAction, HideAction;
        public Image Hider;

        protected MaskableGraphic[] AllGraphic;
        protected string GraphicTag;

        protected bool CanShow
        {
            get
            {
                return !(_isOpen || _isInProgress && _showCoroutine != null);
            }
        }

        protected bool CanHide
        {
            get
            {
                return !(_isHide || _isInProgress && _hideCoroutine != null);
            }
        }

        private Coroutine 
            _showCoroutine, 
            _hideCoroutine;
        private bool
            _isOpen,
            _isHide,
            _isInProgress;
        public virtual void Show()
        {
            if (!CanShow) return;
            _isHide = false;
            if (_isInProgress && _hideCoroutine != null)
            {
                HideAction.StopCoroutine(_hideCoroutine);
            }
            _isInProgress = true;
            PanelTransform.gameObject.SetActive(true);
            _showCoroutine = ShowAction.Play(PanelTransform, OnShowComplate, GraphicTag);
            if (Hider && gameObject.activeInHierarchy) StartCoroutine(Alpha(0.6f));
        }

        public virtual void Hide()
        {
            if(!CanHide) return;
            _isOpen = false;
            SetRaycastTarget(false, AllGraphic);
            if (_isInProgress && _showCoroutine != null)
            {
                HideAction.StopCoroutine(_showCoroutine);
            }
            if (Hider && gameObject.activeInHierarchy) StartCoroutine(Alpha(0));
            _isInProgress = true;
            _hideCoroutine = HideAction.Play(PanelTransform, OnHideComplate, GraphicTag);
        }

        public virtual void Initialize()
        {
            AllGraphic = GetGraphicsWidthTag(PanelTransform, GraphicTag);
            PanelTransform.gameObject.SetActive(false);
            OnShowComplate += () =>
            {
                _isOpen = true;
                _isInProgress = false;
                _showCoroutine = null;
                SetRaycastTarget(true, AllGraphic);
            };
            OnHideComplate += () =>
            {
                _isHide = true;
                _isInProgress = false;
                _hideCoroutine = null;
                PanelTransform.gameObject.SetActive(false);
            };
            if(!Hider)return;
            var button = Hider.GetComponent<Button>();
            if(!button)return;
            button.onClick.AddListener(Hide);
        }

        public static void SetRaycastTarget(bool isRaycast, MaskableGraphic[] graphics)
        {
            foreach (var gr in graphics)
            {
                gr.raycastTarget = isRaycast;
            }
        }
        public static void SetAlpha(float alpha, MaskableGraphic[] graphics)
        {
            foreach (var gr in graphics)
            {
                gr.color = new Color(gr.color.r, gr.color.g, gr.color.b, alpha);
            }
        }

        public static MaskableGraphic[] GetGraphicsWidthTag(RectTransform paren, string graphicTag = "Untagged")
        {
            if (string.IsNullOrEmpty(graphicTag)) return paren.GetComponentsInChildren<MaskableGraphic>();
            return paren.GetComponentsInChildren<MaskableGraphic>().Where(graphic => graphic.CompareTag(graphicTag)).ToArray();
        }

        private IEnumerator Alpha(float alpha)
        {
            const float timescale = 10;
            if (alpha > 0) Hider.enabled = true;
            var startAlpha = Hider.color.a;
            for (var time = 0f; time < 1; time+=Time.deltaTime* timescale)
            {
                Hider.color = new Color(Hider.color.r, Hider.color.g, Hider.color.b, Mathf.Lerp(startAlpha, alpha,time));
                yield return null;
            }
            Hider.color = new Color(Hider.color.r, Hider.color.g, Hider.color.b, alpha);
            if (alpha <= 0) Hider.enabled = false;
        }

        public event Action 
            OnShowComplate, 
            OnHideComplate;
    }
}
