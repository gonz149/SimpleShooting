using UnityEngine;

/// <summary>
/// 弾の基底クラス
/// Open/Closed Principle: 拡張に開放、修正に閉鎖
/// Single Responsibility Principle: 弾の基本動作のみを担当
/// </summary>
public abstract class BaseBullet : MonoBehaviour
{
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float lifeTime = 3f;
    [SerializeField] protected float damage = 1f;

    protected Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        // 弾を前方に飛ばす
        rb.linearVelocity = transform.forward * speed;
        
        // 一定時間後に自身を破壊
        Destroy(gameObject, lifeTime);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
    }

    /// <summary>
    /// 衝突処理を派生クラスで実装
    /// Template Method Pattern
    /// </summary>
    protected abstract void HandleCollision(Collision collision);

    protected virtual void DestroyBullet()
    {
        Destroy(gameObject);
    }
}