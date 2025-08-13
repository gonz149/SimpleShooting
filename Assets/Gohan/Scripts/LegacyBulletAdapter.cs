using UnityEngine;

/// <summary>
/// 既存のBulletクラスを新しいシステムに適応させるアダプター
/// Adapter Pattern: 古いBulletクラスのインターフェースを維持しつつ新機能を提供
/// </summary>
public class LegacyBulletAdapter : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // 弾を前方に飛ばす
        rb.linearVelocity = transform.forward * speed;

        // 一定時間後に自身を破壊
        Destroy(gameObject, lifeTime);
    }

    // 既存のOnCollisionEnterと同じ処理
    void OnCollisionEnter(Collision collision)
    {
        // 衝突したオブジェクトのタグを取得
        string collidedTag = collision.gameObject.tag;

        // この弾がプレイヤーの弾の場合
        if (CompareTag("PlayerBullet"))
        {
            if (collidedTag == "Enemy")
            {
                // 新しいIDamageableインターフェースを優先的に使用
                IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(1f);
                }
                else
                {
                    // 従来の方式
                    Destroy(collision.gameObject);
                }
                Destroy(gameObject);
            }
        }
        // この弾が敵の弾の場合
        else if (CompareTag("EnemyBullet"))
        {
            if (collidedTag == "Player")
            {
                // 新しいIDamageableインターフェースを優先的に使用
                IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(1f);
                }
                else
                {
                    // 従来の方式
                    Destroy(collision.gameObject);
                }
                Destroy(gameObject);
            }
            else if (collidedTag == "Enemy")
            {
                // 敵の弾が敵に当たった場合は何もしない（衝突を無視）
                return;
            }
        }
    }
}