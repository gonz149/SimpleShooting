using UnityEngine;

/// <summary>
/// 移動機能のインターフェース
/// Interface Segregation Principle: 移動機能のみを定義
/// </summary>
public interface IMovement
{
    void Move(Vector3 direction);
    void SetSpeed(float speed);
    Vector3 CurrentVelocity { get; }
}