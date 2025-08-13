using UnityEngine;
using System.Reflection;

/// <summary>
/// WeaponSystem設定の共通ユーティリティ
/// DRY原則: 重複したWeaponSystem設定処理を共通化
/// </summary>
public static class WeaponSystemSetup
{
    /// <summary>
    /// WeaponSystemに弾のPrefabを設定
    /// </summary>
    public static void SetBulletPrefab(WeaponSystem weaponSystem, GameObject bulletPrefab)
    {
        if (weaponSystem == null || bulletPrefab == null) return;
        
        var bulletField = typeof(WeaponSystem).GetField("bulletPrefab", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        bulletField?.SetValue(weaponSystem, bulletPrefab);
    }
    
    /// <summary>
    /// WeaponSystemに発射ポイントを設定
    /// </summary>
    public static void SetFirePoint(WeaponSystem weaponSystem, Transform firePoint)
    {
        if (weaponSystem == null || firePoint == null) return;
        
        var firePointField = typeof(WeaponSystem).GetField("firePoint", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        firePointField?.SetValue(weaponSystem, firePoint);
    }
    
    /// <summary>
    /// WeaponSystemに発射レートを設定
    /// </summary>
    public static void SetFireRate(WeaponSystem weaponSystem, float fireRate)
    {
        if (weaponSystem == null) return;
        
        var fireRateField = typeof(WeaponSystem).GetField("fireRate", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        fireRateField?.SetValue(weaponSystem, fireRate);
    }
}