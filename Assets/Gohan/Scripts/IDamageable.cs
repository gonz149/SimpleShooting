using UnityEngine;

/// <summary>
/// ダメージを受け取ることができるオブジェクトのインターフェース
/// Interface Segregation Principle: ダメージ処理のみを定義
/// </summary>
public interface IDamageable
{
    void TakeDamage(float damage);
    bool IsDestroyed { get; }
}