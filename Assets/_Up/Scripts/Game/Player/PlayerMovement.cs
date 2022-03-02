using System;
using System.Collections;
using System.Linq;
using Assets._Scripts.Tools;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public event Action PlayerJump;

    private const float SWITCH_DIRECTION_TIMEOUT = 0.05f;

    [SerializeField] private Direction startDirection;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float teleportSpeed;
    [SerializeField] private float boosterSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float flyAwayPower;

    public Direction Direction
    {
        get
        {
            return direction;
        }

        set
        {
            direction = value;

            transform.eulerAngles -= 180 * Vector3.up;

            if (direction == Direction.Right)
            {
                currentMovement = Vector2.right;
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                currentMovement = Vector2.left;
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
    }

    private Direction direction;

    public bool IsGrounded { get; set; }
    public bool IsMoveToTarget { get; set; }

    public bool IsCalm
    {
        get
        {
            return Rigidbody2D.velocity.magnitude < 0.05f;
        }
    }

    private Transform Transform { get; set; }
    private Rigidbody2D Rigidbody2D { get; set; }
    private Vector3 currentMovement;

    private bool isSwitchDirectionAllowed;

    private void Awake()
    {
        Transform = GetComponent<Transform>();
        Rigidbody2D = GetComponent<Rigidbody2D>();

        Direction = startDirection;

        isSwitchDirectionAllowed = true;
    }

    private void Start()
    {
        if (GameManager.Instance.LevelsCompleted < 10)
        {
            moveSpeed = 2.8f;
        }
        else
        {
            moveSpeed = 3.5f;
        }
    }

    public void Move()
    {
        if (!IsMoveToTarget)
        {
            Transform.position = Vector2.MoveTowards(Transform.position, Transform.position + currentMovement, moveSpeed * Time.deltaTime);

            CheckGround();
        }
        else
        {
            IsGrounded = true;
        }
    }

    private void CheckGround()
    {
        Debug.DrawLine(transform.position + 0.15f * Vector3.up, transform.position + 0.6f * Vector3.down, Color.green);
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position + 0.15f * Vector3.up, Vector2.down, 0.6f);

        bool previousIsGrounded = IsGrounded;

        IsGrounded = hit.Any(h => h.collider.CompareTag(GameConfig.HORIZONTAL_LINE_TAG) ||
                                  h.collider.CompareTag(GameConfig.DISPOSABLE_HORIZONTAL_LINE_TAG));

        if (!previousIsGrounded && IsGrounded)
        {
            Player.Instance.PlayerEffects.CreateSplash();
        }
    }

    public void Jump()
    {
        if (!IsGrounded || IsMoveToTarget)
        {
            return;
        }

        Rigidbody2D.velocity = jumpPower * Vector2.up;

        SoundManager.Instance.PlayJumpSFX();

        PlayerJump.SaveInvoke();
    }

    public void SwitchDirection()
    {
        if (!isSwitchDirectionAllowed || IsMoveToTarget)
        {
            return;
        }

        Direction = direction == Direction.Right ?
            Direction.Left :
            Direction.Right;

        StartCoroutine(StartSwitchDirectionTimeout());
    }

    public void StartFlyAway()
    {
        Vector2 direction = Direction == Direction.Right ? new Vector2(3, flyAwayPower) : new Vector2(-3, flyAwayPower);

        Rigidbody2D.AddForce(direction, ForceMode2D.Impulse);

        Player.Instance.PlayerEffects.SetTeleportTrailActive(true);

        StartCoroutine(ScaleOverTime(2, 0.1f));

        SoundManager.Instance.PlayEndJump();
    }

    public void MoveToTarget(int skipedStructs, bool isTeleport)
    {
        if (IsMoveToTarget)
        {
            return;
        }

        Player.Instance.PlayerEffects.SetTeleportTrailActive(true);

        IsMoveToTarget = true;

        Rigidbody2D.isKinematic = true;
        Rigidbody2D.velocity = Vector2.zero;

        StartCoroutine(StartMoveToTarget(skipedStructs, isTeleport));
    }

    private IEnumerator StartMoveToTarget(int skipedStructs, bool isTeleport)
    {
        yield return new WaitForSeconds(0.9f);
        var oldDampTime = CameraFollow.Instance.DampTime;

        CameraFollow.Instance.DampTime = 0.25f;

        var currentStructID = Player.Instance.CollisionsHandler.RealLastTriggeredStruct.Id;
        var structsCount = Level.Instance.GetStructsCount();

        var targetStructID = 0;

        if (currentStructID + skipedStructs >= structsCount - 1)
        {
            targetStructID = structsCount - 2;
        }
        else
        {
            targetStructID = currentStructID + skipedStructs;
        }

        LevelStruct targetStruct = Level.Instance.GetSctructById(targetStructID);

        var firstOption = targetStructID;
        var secondOption = targetStructID;

        while (!targetStruct.IsClosedStruct)
        {
            firstOption--;
            secondOption++;

            if (firstOption > Player.Instance.CollisionsHandler.RealLastTriggeredStruct.Id + 5)
            {
                targetStruct = Level.Instance.GetSctructById(firstOption);

                if (!targetStruct.IsClosedStruct)
                {
                    targetStruct = Level.Instance.GetSctructById(secondOption);

                    if (targetStruct.Id == Level.Instance.GetLastStruct().Id)
                    {
                        break;
                    }
                }
            }
            else
            {
                targetStruct = Level.Instance.GetSctructById(secondOption);

                if (targetStruct.Id == Level.Instance.GetLastStruct().Id)
                {
                    break;
                }
            }
        }

        Vector3 targetPosition = targetStruct.transform.position;

        float finalSpeed =  isTeleport? teleportSpeed : boosterSpeed;

        while ((transform.position - targetPosition).magnitude > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, finalSpeed * Time.deltaTime);

            yield return null;
        }

        CameraFollow.Instance.DampTime = oldDampTime;

        Player.Instance.PlayerEffects.SetTeleportTrailActive(false);

        Rigidbody2D.isKinematic = false;

        IsMoveToTarget = false;

        FXManager.Instance.GetFinalExplosionObject().SetActive(false);
        SoundManager.Instance.PlayJumpSFX();
    }

    private IEnumerator ScaleOverTime(float scaleFactor, float time)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 destinationScale = scaleFactor * Vector3.one;

        float currentTime = 0.0f;

        while (currentTime <= time)
        {
            transform.localScale = Vector3.MoveTowards(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator StartSwitchDirectionTimeout()
    {
        isSwitchDirectionAllowed = false;

        float time = 0f;

        while (time < SWITCH_DIRECTION_TIMEOUT)
        {
            time += Time.deltaTime;

            yield return null;
        }

        isSwitchDirectionAllowed = true;
    }

    public void Reset()
    {
        Rigidbody2D.velocity = Vector2.zero;
        transform.position = Vector2.zero;
    }
}