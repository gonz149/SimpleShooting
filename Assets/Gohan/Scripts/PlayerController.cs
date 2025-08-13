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
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] WeaponSystem weaponSystem;
    [SerializeField] GameObject playerBulletPrefab;

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
            SetupPlayerWeaponSystem(addedWeapon);
        }
    }
    
    void SetupPlayerWeaponSystem(WeaponSystem weaponComp)
    {
        // FirePointを検索
        Transform firePoint = transform.Find("FirePoint");
        if (firePoint != null)
        {
            WeaponSystemSetup.SetFirePoint(weaponComp, firePoint);
        }
        
        // PlayerBulletPrefabの設定（YAGNI原則: Inspector設定のみに簡素化）
        if (playerBulletPrefab != null)
        {
            WeaponSystemSetup.SetBulletPrefab(weaponComp, playerBulletPrefab);
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
}