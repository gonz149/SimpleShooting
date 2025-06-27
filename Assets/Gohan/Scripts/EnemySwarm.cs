using UnityEngine;
using System.Collections.Generic;

public class EnemySwarm : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f; // 集団の移動速度
    [SerializeField] private float moveDistance = 0.5f; // 一歩進む距離
    [SerializeField] private float moveInterval = 1f; // 移動間隔
    [SerializeField] private float minX = -8f; // 移動範囲の左端
    [SerializeField] private float maxX = 8f; // 移動範囲の右端

    private bool movingRight = true; // 右に移動中か
    private float nextMoveTime; // 次の移動時間

    void Start()
    {
        nextMoveTime = Time.time + moveInterval;
    }

    void Update()
    {
        if (Time.time >= nextMoveTime)
        {
            MoveSwarm();
            nextMoveTime = Time.time + moveInterval;
        }
    }

    void MoveSwarm()
    {
        Vector3 currentPosition = transform.position;

        if (movingRight)
        {
            currentPosition.x += moveSpeed * moveInterval; // 右に移動
            if (currentPosition.x >= maxX)
            {
                movingRight = false;
                currentPosition.x = maxX; // 範囲内に収める
                currentPosition.z -= moveDistance; // 一歩前進
            }
        }
        else
        {
            currentPosition.x -= moveSpeed * moveInterval; // 左に移動
            if (currentPosition.x <= minX)
            {
                movingRight = true;
                currentPosition.x = minX; // 範囲内に収める
                currentPosition.z -= moveDistance; // 一歩前進
            }
        }
        transform.position = currentPosition;
    }
}
