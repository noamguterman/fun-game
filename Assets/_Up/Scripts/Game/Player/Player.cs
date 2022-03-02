using Assets._Scripts.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action PlayerDead;

    public PlayerState State
    {
        get; set;
    }

    public PlayerMovement Movement { get; private set; }
    public PlayerCollisionsHandler CollisionsHandler { get; private set; }
    public PlayerEffects PlayerEffects { get; private set; }

    private SpriteRenderer spriteRenderer;

    public static Player Instance;

    private void Awake()
    {
        Instance = this;

        Movement = GetComponent<PlayerMovement>();
        CollisionsHandler = GetComponent<PlayerCollisionsHandler>();
        PlayerEffects = GetComponent<PlayerEffects>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        State = PlayerState.Alive;
    }

    private void Update()
    {
        if (State == PlayerState.Alive)
        {
            CheckPlayerSlide();
            CheckDeath();
        }
    }

    private void CheckDeath()
    {
        if (!Movement.IsGrounded)
        {
            var isDead = GetHorizontalLinesBelow().Count == 0;

            if (isDead)
            {
                State = PlayerState.Dies;

                Invoke("PlayerDeath", 1f);
            }
        }
    }

    private void CheckPlayerSlide()
    {
        PlayerEffects.SetTrailActive(Movement.IsGrounded && !Movement.IsMoveToTarget && State == PlayerState.Alive);
    }

    private void PlayerDeath()
    {
        List<Transform> linesBelow = GetHorizontalLinesBelow();

        if (linesBelow.Count == 0)
        {
            State = PlayerState.Dead;

            PlayerDead.SaveInvoke();
        }
        else
        {
            State = PlayerState.Alive;
        }
    }

    public void SetSkin(Sprite skin)
    {
        spriteRenderer.sprite = skin;
    }

    public List<Transform> GetHorizontalLinesBelow()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector2.down, GameConfig.PLAYER_DEATH_HEIGHT);

        RaycastHit2D[] horizontalLinesBelow = hit.Where(h => h.collider.CompareTag(GameConfig.HORIZONTAL_LINE_TAG) || h.collider.CompareTag(GameConfig.DISPOSABLE_HORIZONTAL_LINE_TAG)).ToArray();

        return horizontalLinesBelow.Select(line => line.transform).ToList();
    }
}