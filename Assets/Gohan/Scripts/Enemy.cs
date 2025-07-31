using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject enemyBulletPrefab; // 敵の弾のプレハブ
    [SerializeField] private float fireRate = 2f; // 発射間隔
    [SerializeField] private Vector3 fireOffset = new Vector3(0, 0, -0.5f); // 弾の発射位置オフセット

    private float nextFireTime;

    void Start()
    {
        nextFireTime = Time.time + Random.Range(0.5f, fireRate); // 初回発射までの時間をランダムに設定
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + fireRate + Random.Range(-0.5f, 0.5f); // 次の発射時間をランダムに調整
        }
    }

    void FireBullet()
    {
        // プレイヤーの方向を向く（簡易的な追尾）
        // プレイヤーオブジェクトをタグで検索
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            // 弾のプレハブをインスタンス化し、敵の向きに合わせて発射
            GameObject bullet = Instantiate(enemyBulletPrefab, transform.position + fireOffset, Quaternion.LookRotation(directionToPlayer));
            // 弾の速度を設定（Bulletスクリプトで速度が設定されるため、ここでは方向のみ）
            // Rigidbody rb = bullet.GetComponent<Rigidbody>();
            // if (rb != null)
            // {
            //     rb.linearVelocity = directionToPlayer * bullet.GetComponent<Bullet>().speed; // Bulletスクリプトのspeedを使用
            // }
        }
    }
}
