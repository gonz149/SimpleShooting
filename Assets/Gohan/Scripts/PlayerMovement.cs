using UnityEngine;

/// <summary>
/// プレイヤーの移動制御
/// Single Responsibility Principle: 移動制御のみを担当
/// </summary>
public class PlayerMovement : MonoBehaviour, IMovement
{
    [SerializeField] private float moveSpeed = 5f;
    
    private Rigidbody rb;
    private Vector3 currentVelocity;

    public Vector3 CurrentVelocity => currentVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
    {
        // X軸方向のみ移動（インベーダーゲームの制約）
        Vector3 moveDirection = new Vector3(direction.x, 0f, 0f);
        currentVelocity = moveDirection * moveSpeed;
        rb.linearVelocity = currentVelocity;
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }
}