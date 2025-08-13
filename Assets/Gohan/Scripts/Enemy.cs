using UnityEngine;

/// <summary>
/// 改善された敵クラス
/// Single Responsibility Principle: 敵の行動のみを担当
/// Dependency Inversion Principle: インターフェースに依存
/// </summary>
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Weapon Settings")]
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private Vector3 fireOffset = new Vector3(0, 0, -0.5f);
    [SerializeField] private GameObject enemyBulletPrefab;
    
    [Header("Dependencies")]
    [SerializeField] private WeaponSystem weaponSystem;
    [SerializeField] private PlayerTargetProvider targetProvider;

    [Header("Health")]
    [SerializeField] private float maxHealth = 1f;
    
    private float currentHealth;
    private float nextFireTime;
    private IWeaponSystem weapon;
    private ITargetProvider target;

    public bool IsDestroyed => currentHealth <= 0;

    private void Awake()
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
            SetupEnemyWeaponSystem(addedWeapon);
        }
        
        if (target == null)
        {
            Debug.Log("PlayerTargetProvider component not found on " + gameObject.name + ". Adding automatically.");
            var addedTarget = gameObject.AddComponent<PlayerTargetProvider>();
            target = addedTarget;
            targetProvider = addedTarget;
        }
    }
    
    private void SetupEnemyWeaponSystem(WeaponSystem weaponComp)
    {
        // EnemyBulletPrefabの設定（Inspectorで設定されたPrefabを使用）
        if (enemyBulletPrefab != null)
        {
            var field = typeof(WeaponSystem).GetField("bulletPrefab", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(weaponComp, enemyBulletPrefab);
        }
        else
        {
            Debug.LogWarning("EnemyBulletPrefab is not assigned in Inspector for " + gameObject.name);
        }
        
        // FireRateの設定
        var fireRateField = typeof(WeaponSystem).GetField("fireRate", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        fireRateField?.SetValue(weaponComp, fireRate);
    }

    private void Start()
    {
        nextFireTime = Time.time + Random.Range(0.5f, fireRate);
        
        // 武器システムにターゲットを設定
        if (weapon != null && target != null && target.HasTarget)
        {
            weapon.SetTarget(target.GetTarget());
        }
    }

    private void Update()
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
        
        if (currentHealth <= 0)
        {
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        // イベント通知などの処理をここに追加可能
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