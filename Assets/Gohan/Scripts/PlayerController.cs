using UnityEngine;
using UnityEngine.InputSystem; // Input Systemを使用するために必要

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // 移動速度

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

    private void FixedUpdate()
    {
        // X軸方向のみ移動
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, 0f);
        rb.linearVelocity = moveDirection * moveSpeed;
    }
}
