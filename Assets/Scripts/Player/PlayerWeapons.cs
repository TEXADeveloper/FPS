using UnityEngine;
using System.Collections;

public class PlayerWeapons : MonoBehaviour
{
    private PlayerMovement pM;
    private PlayerSound pS;
    [HideInInspector] public bool Aiming = false;
    [Header("Weapons")]
        [SerializeField] Weapon[] weapons;
        private GameObject thisWeapon;
        private Transform shootPoint;
        int selected;
        [SerializeField] private LineRenderer lr;
        [SerializeField] private LayerMask layer;

    float automaticShootTimer = 0;
    float errorTimer = 0;
    float error = 0;
    bool shooting = false;

    void SetWeapon(int newWeapon)
    {
        if (weapons.Length <= 0)
            return;
        selected = newWeapon;
        GameObject OldWeapon = thisWeapon;
        thisWeapon = GameObject.Instantiate(weapons[selected].weaponPrefab, Camera.main.transform);
        thisWeapon.transform.localPosition = weapons[selected].screenPosition;
        if (shootPoint == null)
            shootPoint = new GameObject("ShootPoint").transform;
        shootPoint.transform.parent = thisWeapon.transform;
        shootPoint.localPosition = weapons[selected].shootPoint;
        Destroy(OldWeapon);
    }

    public void SelectWeapon(int num)
    {
        if (weapons.Length >= num)
            SetWeapon(num);
    }

    public void ChangeWeapon(int value)
    {
        if (weapons.Length <= 0)
            return;
        if ((selected == 0 && value == -1) || (selected == weapons.Length - 1 && value == 1))
            SelectWeapon((value == 1)? 0 : weapons.Length - 1);
        else
            SelectWeapon(selected + value);
    }

    void Start()
    {
        pS = this.GetComponent<PlayerSound>();
        pM = this.GetComponent<PlayerMovement>();
        SetWeapon(0);
    }

    public void Aim(bool performed)
    {
        Aiming = performed;
        thisWeapon.transform.localPosition = Aiming? weapons[selected].aimingPosition : weapons[selected].screenPosition;
    }

    void Update()
    {
        automaticShootTimer -= Time.deltaTime;
        if (canShootAutomatic())
            shoot();

        if (shooting)
            errorTimer = Mathf.Clamp(errorTimer + Time.deltaTime, 0f, weapons[selected].timeOnMaxError);
        else
            errorTimer = Mathf.Clamp(errorTimer - Time.deltaTime * weapons[selected].timeDecreaseMultiplier, 0f, weapons[selected].timeOnMaxError);
    }

    public void ShootInput(bool pressed)
    {
        shooting = pressed;
        if (canShootAutomatic() || canShootNonAutomatic())
            shoot();
    }

    public void Reload()
    {
        StartCoroutine(weapons[selected].Reload(pS));
    }

    private bool canShootAutomatic() { return canShoot() && weapons[selected].automatic; }
    private bool canShootNonAutomatic() { return canShoot() && !weapons[selected].automatic; }
    private bool canShoot() { return shooting && automaticShootTimer <= 0 && weapons[selected].bullets > 0 && !weapons[selected].GetReloading(); }

    private void shoot()
    {
        weapons[selected].bullets--;
        automaticShootTimer = weapons[selected].shootingCooldown;

        shootingLogic();
        StartCoroutine(showLine());
        pS.PlaySound(weapons[selected].shootClip);
    }

    void shootingLogic()
    {
        float errorOverTime = (weapons[selected].errorMargin / weapons[selected].timeOnMaxError * errorTimer) + ((pM.Grounded())? 0 : weapons[selected].jumpErrorIncrease) + ((!pM.Moving())? 0 : weapons[selected].moveErrorIncrease);
        Vector3 normal = (new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0)).normalized;
        RaycastHit[] points = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward + (normal * errorOverTime), weapons[selected].maxDistance, layer);
        Vector3[] positions = new Vector3[2];
        positions[0] = shootPoint.position;
        positions[1] = Camera.main.transform.forward * weapons[selected].maxDistance;
        if (points.Length > 0)
        {
            positions[1] = points[0].point;
            RaycastHit[] hit = Physics.RaycastAll(positions[0], positions[1], 2 * weapons[selected].maxDistance, layer);
        }
        lr.SetPositions(positions);
    }

    private IEnumerator showLine()
    {
        lr.enabled = true;
        yield return new WaitForSeconds(.1f);
        lr.enabled = false;
    }
}
