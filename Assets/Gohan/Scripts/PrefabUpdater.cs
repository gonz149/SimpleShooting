using UnityEngine;

/// <summary>
/// プレハブの自動更新ユーティリティ
/// 実行時に必要なコンポーネントを自動追加
/// </summary>
public class PrefabUpdater : MonoBehaviour
{
    [Header("Auto-Update Settings")]
    [SerializeField] private bool enableAutoUpdate = true;
    
    private void Awake()
    {
        if (!enableAutoUpdate) return;
        
        UpdatePlayerComponents();
        UpdateEnemyComponents();
    }
    
    private void UpdatePlayerComponents()
    {
        if (CompareTag("Player"))
        {
            // PlayerMovementが存在しない場合は追加
            if (GetComponent<PlayerMovement>() == null)
            {
                gameObject.AddComponent<PlayerMovement>();
                Debug.Log("PlayerMovement component added to " + gameObject.name);
            }
            
            // WeaponSystemが存在しない場合は追加
            if (GetComponent<WeaponSystem>() == null)
            {
                var weaponSystem = gameObject.AddComponent<WeaponSystem>();
                
                // FirePointを検索して設定
                Transform firePoint = transform.Find("FirePoint");
                if (firePoint != null)
                {
                    // WeaponSystemの設定をリフレクションで行う
                    var weaponSystemType = typeof(WeaponSystem);
                    var firePointField = weaponSystemType.GetField("firePoint", 
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (firePointField != null)
                    {
                        firePointField.SetValue(weaponSystem, firePoint);
                    }
                    
                    // BulletPrefabの設定
                    GameObject bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet");
                    if (bulletPrefab != null)
                    {
                        var bulletPrefabField = weaponSystemType.GetField("bulletPrefab", 
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (bulletPrefabField != null)
                        {
                            bulletPrefabField.SetValue(weaponSystem, bulletPrefab);
                        }
                    }
                }
                
                Debug.Log("WeaponSystem component added to " + gameObject.name);
            }
        }
    }
    
    private void UpdateEnemyComponents()
    {
        if (CompareTag("Enemy"))
        {
            // WeaponSystemが存在しない場合は追加
            if (GetComponent<WeaponSystem>() == null)
            {
                var weaponSystem = gameObject.AddComponent<WeaponSystem>();
                
                // EnemyBulletPrefabの設定
                GameObject enemyBulletPrefab = Resources.Load<GameObject>("Prefabs/EnemyBullet");
                if (enemyBulletPrefab != null)
                {
                    var weaponSystemType = typeof(WeaponSystem);
                    var bulletPrefabField = weaponSystemType.GetField("bulletPrefab", 
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (bulletPrefabField != null)
                    {
                        bulletPrefabField.SetValue(weaponSystem, enemyBulletPrefab);
                    }
                }
                
                Debug.Log("WeaponSystem component added to " + gameObject.name);
            }
            
            // PlayerTargetProviderが存在しない場合は追加
            if (GetComponent<PlayerTargetProvider>() == null)
            {
                gameObject.AddComponent<PlayerTargetProvider>();
                Debug.Log("PlayerTargetProvider component added to " + gameObject.name);
            }
        }
    }
}