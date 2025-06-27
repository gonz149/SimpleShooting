using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // 弾の移動速度
    [SerializeField] private float lifeTime = 3f; // 弾の生存時間

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // 弾を前方に飛ばす
        rb.linearVelocity = transform.forward * speed;

        // 一定時間後に自身を破壊
        Destroy(gameObject, lifeTime);
    }
}
