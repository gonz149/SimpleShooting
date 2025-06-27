using UnityEngine;
using UnityEngine.InputSystem; // Input Systemを使用するために必要

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // 移動速度
    [SerializeField] private GameObject bulletPrefab; // 弾のプレハブ
    [SerializeField] private Transform firePoint; // 弾の発射位置
    [SerializeField] private float fireRate = 0.5f; // 発射間隔
    private float nextFireTime = 0f; // 次の発射可能時間

    private Rigidbody rb;
    private Vector2 moveInput; // Input Systemからの移動入力

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Input Systemからの移動入力イベントハンドラ
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Input Systemからの発射入力イベントハンドラ (OnFireからOnAttackに変更)
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    private void FixedUpdate()
    {
        // X軸方向のみ移動
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, 0f);
        rb.linearVelocity = moveDirection * moveSpeed;
    }
}
