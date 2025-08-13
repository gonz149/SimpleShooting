using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの統合制御
/// Single Responsibility Principle: 入力処理と各システムの調整のみを担当
/// Dependency Inversion Principle: インターフェースに依存
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private WeaponSystem weaponSystem;

    // インターフェースでの参照（依存性逆転原則）
    IMovement movement;
    IWeaponSystem weapon;

    Vector2 moveInput;

    void Awake()
    {
        // 依存性の注入
        movement = playerMovement ?? GetComponent<PlayerMovement>();
        weapon = weaponSystem ?? GetComponent<WeaponSystem>();

        // コンポーネントが見つからない場合は自動追加
        if (movement == null)
        {
            Debug.Log("PlayerMovement component not found on " + gameObject.name + ". Adding automatically.");
            var addedMovement = gameObject.AddComponent<PlayerMovement>();
            movement = addedMovement;
            playerMovement = addedMovement;
        }
        
        if (weapon == null)
        {
            Debug.Log("WeaponSystem component not found on " + gameObject.name + ". Adding automatically.");
            var addedWeapon = gameObject.AddComponent<WeaponSystem>();
            weapon = addedWeapon;
            weaponSystem = addedWeapon;
            
            // FirePointとBulletPrefabの自動設定
            SetupWeaponSystem(addedWeapon);
        }
    }
    
    void SetupWeaponSystem(WeaponSystem weaponComp)
    {
        // FirePointを検索
        Transform firePoint = transform.Find("FirePoint");
        if (firePoint != null)
        {
            // SerializedFieldのfirePointを設定するためにリフレクションを使用
            var field = typeof(WeaponSystem).GetField("firePoint", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(weaponComp, firePoint);
        }
        
        // BulletPrefabの設定（Assets/Gohan/Prefabs/Bullet.prefabを使用）
        GameObject bulletPrefab = UnityEngine.Resources.Load<GameObject>("Gohan/Prefabs/Bullet");
        if (bulletPrefab == null)
        {
            // Resourcesフォルダにない場合はAssetDatabaseで検索（Editor専用）
            #if UNITY_EDITOR
            string[] guids = UnityEditor.AssetDatabase.FindAssets("Bullet t:GameObject", new[] {"Assets/Gohan/Prefabs"});
            if (guids.Length > 0)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                bulletPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
            }
            #endif
        }
        
        if (bulletPrefab != null)
        {
            var field = typeof(WeaponSystem).GetField("bulletPrefab", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field?.SetValue(weaponComp, bulletPrefab);
        }
    }

    // Input Systemからの移動入力イベントハンドラ
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Input Systemからの発射入力イベントハンドラ
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && weapon != null && weapon.CanFire)
        {
            weapon.Fire();
        }
    }

    void FixedUpdate()
    {
        if (movement != null)
        {
            Vector3 moveDirection = new Vector3(moveInput.x, 0f, 0f);
            movement.Move(moveDirection);
        }
    }
}