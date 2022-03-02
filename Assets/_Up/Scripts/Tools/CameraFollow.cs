using System.Collections;
using UnityEngine;

namespace Assets._Scripts.Tools
{
    public class CameraFollow : MonoBehaviour
    {
        public float DampTime;

        public Vector3 Offset;

        private Transform Target;

        private Vector3 _velocity;

        public static CameraFollow Instance;

        private Vector3 startOffset;

        private bool isFinished;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            startOffset = Offset;

            Target = Player.Instance.transform;
        }

        private void Update()
        {
            if (Target == null || Player.Instance.State == PlayerState.Dead /*|| Player.Instance.State == PlayerState.Dies*/ || isFinished)
            {
                return;
            }

            if (Player.Instance.Movement.IsMoveToTarget)
            {
                Offset.x = 0;

                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(Target.position.x, Target.position.y, transform.position.z)
                                                - Offset, ref _velocity, 0.1f);
                return;
            }

            LevelStruct lastTriggeredStruct = Player.Instance.CollisionsHandler.RealLastTriggeredStruct;

            if (lastTriggeredStruct != null && lastTriggeredStruct.IsLastLevelStruct)
            {
                isFinished = true;
                StartCoroutine(MoveToFinish());

                return;
            }

            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(Player.Instance.transform.position);

            if ((lastTriggeredStruct != null && !lastTriggeredStruct.IsTransitionPlatform) && (viewportPosition.x > 0 && viewportPosition.x < 1) && Player.Instance.State != PlayerState.Dies)
            {
                Offset.x = 0;

                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(lastTriggeredStruct.CameraBind.position.x, Target.position.y, transform.position.z)
                                                                - Offset, ref _velocity, DampTime);
            }
            else
            {
                if (Player.Instance.Movement.Direction == Direction.Left)
                {
                    Offset.x = 2;
                }
                else
                {
                    Offset.x = -2;
                }

                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(Target.position.x, Target.position.y, transform.position.z)
                                                               - Offset, ref _velocity, DampTime);
            }
        }

        public void FocusImmediately()
        {
            Offset = startOffset;

            transform.position = new Vector3(Target.position.x, Target.position.y, transform.position.z) - Offset;
        }

        public void SetBackgroundColor(Color color)
        {
            Camera.main.backgroundColor = color;
        }

        private IEnumerator MoveToFinish()
        {
            var target = Level.Instance.GetLastStruct().transform.position + Vector3.up;

            target.z = transform.position.z;

            while (Vector3.Distance(transform.position, target) > 0.05f)
            {
                transform.position = Vector3.SmoothDamp(transform.position, target, ref _velocity, DampTime);

                yield return null;
            }
        }
    }
}
