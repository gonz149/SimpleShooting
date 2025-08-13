using UnityEngine;

/// <summary>
/// 敵の弾
/// Open/Closed Principle: BaseBulletを継承して拡張
/// Single Responsibility Principle: 敵弾特有の衝突処理のみ
/// </summary>
public class EnemyBullet : BaseBullet
{
    protected override void HandleCollision(Collision collision)
    {
        GameObject hitObject = collision.gameObject;
        string hitTag = hitObject.tag;
        
        // プレイヤーとの衝突をチェック
        if (hitTag == "Player")
        {
            // IDamageableインターフェースでダメージ処理
            IDamageable damageable = hitObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
            else
            {
                // 従来の方式との互換性のため
                Destroy(hitObject);
            }
            
            DestroyBullet();
        }
        else if (hitTag == "Enemy")
        {
            // 敵の弾が敵に当たった場合は何もしない（物理レベルで制御済み）
            return;
        }
        // 壁や他のオブジェクトとの衝突処理はここに追加可能
    }
}