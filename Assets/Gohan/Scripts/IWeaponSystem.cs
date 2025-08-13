using UnityEngine;

/// <summary>
/// 武器システムのインターフェース
/// Interface Segregation Principle: 武器機能のみを定義
/// </summary>
public interface IWeaponSystem
{
    bool CanFire { get; }
    void Fire();
    void SetTarget(Transform target);
}