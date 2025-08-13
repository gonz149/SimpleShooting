using UnityEngine;

/// <summary>
/// 既存のPlayerControllerとの互換性を保つアダプター
/// Adapter Pattern: 既存のインターフェースを新しいシステムに適応
/// </summary>
public class PlayerAdapter : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 3f;
    
    private float currentHealth;
    
    public bool IsDestroyed => currentHealth <= 0;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            DestroyPlayer();
        }
    }

    private void DestroyPlayer()
    {
        // プレイヤー破壊時の処理
        // GameManagerへの通知などをここに追加
        Destroy(gameObject);
    }

    public void SetHealth(float health)
    {
        maxHealth = health;
        currentHealth = health;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
}