using UnityEngine;

/// <summary>
/// 武器システム
/// Single Responsibility Principle: 武器の発射制御のみを担当
/// </summary>
public class WeaponSystem : MonoBehaviour, IWeaponSystem
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = 0.5f;
    
    float nextFireTime = 0f;
    Transform target;

    public bool CanFire => Time.time >= nextFireTime;

    public void Fire()
    {
        if (!CanFire || bulletPrefab == null || firePoint == null)
            return;

        nextFireTime = Time.time + fireRate;
        
        // 弾の方向を決定
        Quaternion rotation = target != null 
            ? Quaternion.LookRotation((target.position - firePoint.position).normalized)
            : firePoint.rotation;
            
        Instantiate(bulletPrefab, firePoint.position, rotation);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetFireRate(float newFireRate)
    {
        fireRate = newFireRate;
    }

    public void SetBulletPrefab(GameObject newBulletPrefab)
    {
        bulletPrefab = newBulletPrefab;
    }
}