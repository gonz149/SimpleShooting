using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの統合制御
/// Single Responsibility Principle: 入力処理と各システムの調整のみを担当
/// Dependency Inversion Principle: インターフェースに依存
/// </summary>
public class NewPlayerController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private WeaponSystem weaponSystem;

    // インターフェースでの参照（依存性逆転原則）
    private IMovement movement;
    private IWeaponSystem weapon;

    private Vector2 moveInput;

    private void Awake()
    {
        // 依存性の注入
        movement = playerMovement ?? GetComponent<PlayerMovement>();
        weapon = weaponSystem ?? GetComponent<WeaponSystem>();

        // コンポーネントが見つからない場合の警告
        if (movement == null)
        {
            Debug.LogError("PlayerMovement component not found on " + gameObject.name);
        }
        
        if (weapon == null)
        {
            Debug.LogError("WeaponSystem component not found on " + gameObject.name);
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

    private void FixedUpdate()
    {
        if (movement != null)
        {
            Vector3 moveDirection = new Vector3(moveInput.x, 0f, 0f);
            movement.Move(moveDirection);
        }
    }
}