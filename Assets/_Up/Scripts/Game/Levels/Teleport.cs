using UnityEngine;

public class Teleport : MonoBehaviour
{
    [HideInInspector] public int SkipedStructs;

    public void TeleportPlayer()
    {
        Player.Instance.Movement.MoveToTarget(SkipedStructs, true);

        Destroy(gameObject);
    }
}