using Assets._Scripts.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionsHandler : MonoBehaviour
{
    public BoxCollider2D BoxCollider2D { get; private set; }
    public LevelStruct LastTriggeredStruct { get; set; }
    public LevelStruct RealLastTriggeredStruct { get; private set; }

    public event Action LastStructReached;

    private void Awake()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var playerMovement = Player.Instance.Movement;

        if (!playerMovement.IsMoveToTarget)
        {
            if (collision.collider.CompareTag(GameConfig.VERTICAL_LINE_TAG) ||
                collision.collider.CompareTag(GameConfig.DISPOSABLE_VERTICAL_LINE_TAG))
            {
                playerMovement.SwitchDirection();
            }

            if (collision.collider.CompareTag(GameConfig.MAGIC_BALL_TAG))
            {
                var magicBall = collision.collider.GetComponent<MagicBall>();

                magicBall.MoveToNextPoint();
                playerMovement.SwitchDirection();
            }

            if (collision.collider.CompareTag(GameConfig.PROTECTOR))
            {
                var protector = collision.collider.GetComponent<Protector>();

                SoundManager.Instance.PlayProtectorSave();
                protector.Dispose();
                playerMovement.SwitchDirection();
            }

            if (collision.collider.CompareTag(GameConfig.DISPOSABLE_HORIZONTAL_LINE_TAG))
            {
                var disposableLine = collision.collider.GetComponent<DisposableLine>();

                if (disposableLine.transform.position.y + 0.05f < Player.Instance.transform.position.y)
                {
                    disposableLine.Dispose();
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GameConfig.DISPOSABLE_VERTICAL_LINE_TAG))
        {
            var disposableLine = collision.collider.GetComponent<DisposableLine>();

            disposableLine.Dispose();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConfig.LEVEL_STRUCT_TAG))
        {
            var levelStruct = collision.GetComponent<LevelStruct>();

            if (Player.Instance.transform.position.y > levelStruct.transform.position.y && levelStruct.Id != 0 && LastTriggeredStruct.Id < levelStruct.Id)
            {
                Player.Instance.Movement.SwitchDirection();

                return;
            }

            if (LastTriggeredStruct != null && LastTriggeredStruct.Id > levelStruct.Id)
            {
                LastTriggeredStruct.SetVerticalLinesEnabled(false);
            }

            levelStruct.SetVerticalLinesEnabled(true);
        }
        else if (collision.CompareTag(GameConfig.TELEPORT_TAG))
        {
            var teleport = collision.GetComponent<Teleport>();

            teleport.TeleportPlayer();

            FXManager.Instance.PlayWinVFX(transform.position);
            SoundManager.Instance.PlayTeleportSFX();
        }
        else if (collision.CompareTag(GameConfig.COIN_TAG))
        {
            if(GameManager.Mode == GameMode.Levels)
                GameData.Coins += 2;
            else
            {
                GameData.Coins++;
            }

            var coin = collision.GetComponent<Coin>();

            coin.Dispose();

            SoundManager.Instance.PlayGemSFX();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(GameConfig.LEVEL_STRUCT_TAG))
        {
            var levelStruct = collision.GetComponent<LevelStruct>();

            if (!levelStruct.IsTriggered)
            {
                List<Transform> horizontalLinesBelow = Player.Instance.GetHorizontalLinesBelow();

                if (horizontalLinesBelow.Count > 0)
                {
                    if (levelStruct.IsContains(horizontalLinesBelow))
                    {
                        var pointsString = PointsManager.Instance.AddPoints();

                        levelStruct.ShowPoints(pointsString);
                        GameManager.Instance.DeactiveEndlessLevelPart(levelStruct.transform.parent.GetComponent<EndlessLevelPart>());
                        levelStruct.DeactiveTrigger();

                        LastTriggeredStruct = levelStruct;

                        UIManager.Instance.GetPanel<UIGamePanel>().ProgressBar.UpdateProgress();

                        if (levelStruct.IsLastLevelStruct)
                        {
                            LastStructReached.SaveInvoke();
                        }
                    }
                }
            }

            RealLastTriggeredStruct = levelStruct;
        }
    }
}