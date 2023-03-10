using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    PlayerMovement pM;
    PlayerWeapons pW;

    void Start()
    {
        pM = this.GetComponent<PlayerMovement>();
        pW = this.GetComponent<PlayerWeapons>();
    }

    void Update()
    {
        pM.MoveInput(moveInput);
        pM.MouseInput(mouseInput);
    }

    Vector2 moveInput = Vector2.zero;
    public void SetMovementHorizontal(InputAction.CallbackContext ctx) => moveInput = new Vector2(ctx.ReadValue<float>(), moveInput.y);

    public void SetMovementVertical(InputAction.CallbackContext ctx) => moveInput = new Vector2(moveInput.x, ctx.ReadValue<float>());

    Vector2 mouseInput = Vector2.zero;
    public void SetMouseHorizontal(InputAction.CallbackContext ctx) => mouseInput = new Vector2(ctx.ReadValue<float>(), mouseInput.y);

    public void SetMouseVertical(InputAction.CallbackContext ctx) => mouseInput = new Vector2(mouseInput.x, ctx.ReadValue<float>());

    public void Jump() => pM.Jump();

    public void Aim(InputAction.CallbackContext ctx) => pW.Aim(ctx.performed);

    public void Shoot(InputAction.CallbackContext ctx) => pW.ShootInput(ctx.performed);
    
    public void ReloadWeapon(InputAction.CallbackContext ctx) { if (ctx.performed) pW.Reload(); }

    public void NextWeapon(InputAction.CallbackContext ctx) { if(ctx.performed) pW.ChangeWeapon(1); }
    public void PrevWeapon(InputAction.CallbackContext ctx) { if(ctx.performed) pW.ChangeWeapon(-1); }

    public void SelectWeapon(int num) => pW.SelectWeapon(num);
}
