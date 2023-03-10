using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Weapon")]
public class Weapon : ScriptableObject
{
    public bool automatic;
    public int maxBulletAmount;
    public int bullets;
    public float reloadTime;
    public float shootingCooldown;
    public float maxDistance;
    public float errorMargin;
    public float jumpErrorIncrease;
    public float moveErrorIncrease;
    public float timeOnMaxError;
    public float timeDecreaseMultiplier;
    public Vector3 screenPosition;
    public Vector3 aimingPosition;
    public Vector3 shootPoint;
    public GameObject weaponPrefab;
    public AudioClip shootClip;
    public AudioClip reloadClip;
    public bool reloading = false;

    public bool GetReloading() { return reloading; }

    public IEnumerator Reload(PlayerSound pS)
    {
        if (bullets == maxBulletAmount || reloading)
            yield break;
        pS.PlaySound(reloadClip);
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        bullets = maxBulletAmount;
        reloading = false;
    }
}
