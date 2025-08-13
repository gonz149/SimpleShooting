using UnityEngine;

/// <summary>
/// ターゲット提供のインターフェース
/// Dependency Inversion Principle: 抽象に依存させる
/// </summary>
public interface ITargetProvider
{
    Transform GetTarget();
    bool HasTarget { get; }
}