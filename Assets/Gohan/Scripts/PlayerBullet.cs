using UnityEngine;

/// <summary>
/// プレイヤーの弾
/// Open/Closed Principle: BaseBulletを継承して拡張
/// Single Responsibility Principle: プレイヤー弾特有の衝突処理のみ
/// </summary>
public class PlayerBullet : BaseBullet
{
    protected override void HandleCollision(Collision collision)
    {
        GameObject hitObject = collision.gameObject;
        
        // 敵との衝突をチェック
        if (hitObject.CompareTag("Enemy"))
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
        // 他のオブジェクト（壁など）との衝突では破壊しない場合の処理も可能
    }
}