using UnityEngine;

/// <summary>
/// プレイヤーターゲット提供者
/// Single Responsibility Principle: プレイヤー検索のみを担当
/// </summary>
public class PlayerTargetProvider : MonoBehaviour, ITargetProvider
{
    private Transform playerTransform;
    private bool isPlayerCached = false;

    public bool HasTarget => GetTarget() != null;

    public Transform GetTarget()
    {
        // キャッシュされたプレイヤーが有効かチェック
        if (isPlayerCached && playerTransform != null)
        {
            return playerTransform;
        }

        // プレイヤーを検索してキャッシュ
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            isPlayerCached = true;
            return playerTransform;
        }

        isPlayerCached = false;
        return null;
    }

    /// <summary>
    /// プレイヤーが破壊された場合などにキャッシュをクリア
    /// </summary>
    public void ClearCache()
    {
        playerTransform = null;
        isPlayerCached = false;
    }
}