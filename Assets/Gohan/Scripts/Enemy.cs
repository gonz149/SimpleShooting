using UnityEngine;

/// <summary>
/// 改善された敵クラス
/// Single Responsibility Principle: 敵の行動のみを担当
/// Dependency Inversion Principle: インターフェースに依存
/// </summary>
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Weapon Settings")]
    [SerializeField] float fireRate = 2f;
    [SerializeField] GameObject enemyBulletPrefab;
    [SerializeField] Transform firePoint;
    
    [Header("Dependencies")]
    [SerializeField] WeaponSystem weaponSystem;
    [SerializeField] PlayerTargetProvider targetProvider;

    [Header("Health")]
    [SerializeField] float maxHealth = 1f;
    
    float currentHealth;
    float nextFireTime;
    IWeaponSystem weapon;
    ITargetProvider target;

    public bool IsDestroyed => currentHealth <= 0;

    void Awake()
    {
        currentHealth = maxHealth;
        
        // 依存性の注入
        weapon = weaponSystem ?? GetComponent<WeaponSystem>();
        target = targetProvider ?? GetComponent<PlayerTargetProvider>();

        // コンポーネントが見つからない場合は自動追加
        if (weapon == null)
        {
            Debug.Log("WeaponSystem component not found on " + gameObject.name + ". Adding automatically.");
            var addedWeapon = gameObject.AddComponent<WeaponSystem>();
            weapon = addedWeapon;
            weaponSystem = addedWeapon;
            
            // EnemyBulletPrefabの自動設定
            SetupWeaponSystem(addedWeapon);
        }
        
        if (target == null)
        {
            Debug.Log("PlayerTargetProvider component not found on " + gameObject.name + ". Adding automatically.");
            var addedTarget = gameObject.AddComponent<PlayerTargetProvider>();
            target = addedTarget;
            targetProvider = addedTarget;
        }
    }
    
    void SetupWeaponSystem(WeaponSystem weaponComp)
    {
        // BulletPrefabの設定（公開メソッドを使用）
        if (enemyBulletPrefab != null)
        {
            weaponComp.SetBulletPrefab(enemyBulletPrefab);
            Debug.Log("EnemyBulletPrefab set for " + gameObject.name + ": " + enemyBulletPrefab.name);
        }
        else
        {
            Debug.LogWarning("EnemyBulletPrefab is not assigned in Inspector for " + gameObject.name);
        }
        
        // FirePointの設定（なければ自身のTransform）
        Transform targetFirePoint = firePoint != null ? firePoint : transform;
        WeaponSystemSetup.SetFirePoint(weaponComp, targetFirePoint);
        
        if (firePoint == null)
        {
            Debug.LogWarning("FirePoint is not assigned in Inspector for " + gameObject.name + ". Using enemy's transform as fallback.");
        }
        
        // FireRateの設定（公開メソッドを使用）
        weaponComp.SetFireRate(fireRate);
    }

    void Start()
    {
        nextFireTime = Time.time + Random.Range(0.5f, fireRate);
        
        // 武器システムにターゲットを設定
        if (weapon != null && target != null && target.HasTarget)
        {
            weapon.SetTarget(target.GetTarget());
        }
    }

    void Update()
    {
        if (Time.time >= nextFireTime && weapon != null && weapon.CanFire)
        {
            if (target != null && target.HasTarget)
            {
                weapon.SetTarget(target.GetTarget());
                weapon.Fire();
                nextFireTime = Time.time + fireRate + Random.Range(-0.5f, 0.5f);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");
        
        if (currentHealth <= 0)
        {
            DestroyEnemy();
        }
    }

    void DestroyEnemy()
    {
        // イベント通知などの処理をここに追加可能
        Debug.Log($"{gameObject.name} destroyed!");
        Destroy(gameObject);
    }

    /// <summary>
    /// 発射レートを動的に変更
    /// </summary>
    public void SetFireRate(float newFireRate)
    {
        fireRate = newFireRate;
    }
}