using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerMovement pM;
    PlayerWeapons pW;

    void Start()
    {
        pM = this.GetComponent<PlayerMovement>();
        pW = this.GetComponent<PlayerWeapons>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
