using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの統合制御
/// Single Responsibility Principle: 入力処理と各システムの調整のみを担当
/// Dependency Inversion Principle: インターフェースに依存
/// </summary>
public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Component References")]
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] WeaponSystem weaponSystem;
    [SerializeField] GameObject playerBulletPrefab;

    [Header("Health")]
    [SerializeField] float maxHealth = 3f;

    // インターフェースでの参照（依存性逆転原則）
    IMovement movement;
    IWeaponSystem weapon;

    Vector2 moveInput;
    float currentHealth;

    public bool IsDestroyed => currentHealth <= 0;

    void Awake()
    {
        currentHealth = maxHealth;
        
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
        }
        
        // 既存・新規に関わらず武器システムをセットアップ
        if (weaponSystem != null)
        {
            SetupPlayerWeaponSystem(weaponSystem);
        }
    }
    
    void SetupPlayerWeaponSystem(WeaponSystem weaponComp)
    {
        // FirePointを検索
        Transform firePoint = transform.Find("FirePoint");
        if (firePoint != null)
        {
            WeaponSystemSetup.SetFirePoint(weaponComp, firePoint);
            Debug.Log("FirePoint set for " + gameObject.name);
        }
        else
        {
            Debug.LogWarning("FirePoint not found for " + gameObject.name + ". Using transform as fallback.");
            WeaponSystemSetup.SetFirePoint(weaponComp, transform);
        }
        
        // PlayerBulletPrefabの設定（公開メソッドを使用）
        if (playerBulletPrefab != null)
        {
            weaponComp.SetBulletPrefab(playerBulletPrefab);
            Debug.Log("PlayerBulletPrefab set for " + gameObject.name + ": " + playerBulletPrefab.name);
        }
        else
        {
            Debug.LogWarning("PlayerBulletPrefab is not assigned in Inspector for " + gameObject.name);
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

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");
        
        if (currentHealth <= 0)
        {
            DestroyPlayer();
        }
    }

    void DestroyPlayer()
    {
        // ゲームオーバー処理やイベント通知などの処理をここに追加可能
        Debug.Log("Player destroyed!");
        Destroy(gameObject);
    }
}