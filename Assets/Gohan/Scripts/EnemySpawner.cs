using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab; // 敵のプレハブ
    [SerializeField] private int rows = 5; // 敵の行数
    [SerializeField] private int columns = 10; // 敵の列数
    [SerializeField] private float spacingX = 1.5f; // X軸方向の間隔
    [SerializeField] private float spacingZ = 1.5f; // Z軸方向の間隔

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                // 配置位置を計算
                float posX = c * spacingX - (columns - 1) * spacingX / 2f;
                float posZ = r * spacingZ;
                Vector3 spawnPosition = transform.position + new Vector3(posX, 0, posZ);

                // 敵をインスタンス化
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform);
            }
        }
    }
}
