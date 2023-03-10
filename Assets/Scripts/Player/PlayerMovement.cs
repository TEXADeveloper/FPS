using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerWeapons pW;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float speedAimingMultiplier;
    [SerializeField] private float speedJumpMultiplier;
    float speedMultiplier;
    [SerializeField] private float sensibility;
    [SerializeField] private float aimingMultiplier;
    float aimMultiplier;
    Vector2 lookDir = Vector2.zero;
    [SerializeField] private float jumpForce;
    private Vector3 direction;
    float xRotation = 0;
    [SerializeField] private LayerMask layer;
    [SerializeField] private Transform groundCheckPosition;
    [SerializeField] private float length;
    bool grounded;

    public bool Grounded() { return grounded; }

    public bool Moving() { return (direction.x != 0 || direction.y != 0); } 

    void Start()
    {
        pW = this.GetComponent<PlayerWeapons>();
    }

    public void MoveInput(Vector2 dir)
    {
        direction = transform.forward * dir.y + transform.right * dir.x;
        direction.Normalize();
    }

    public void MouseInput(Vector2 dir)
    {
        lookDir = dir;
    }

    void FixedUpdate()
    {
        grounded = Physics.CheckSphere(groundCheckPosition.position, length, layer);

        move();
        moveCamera();
    }

    private void move()
    {
        speedMultiplier = pW.Aiming ? speedAimingMultiplier : 1f;
        speedMultiplier = grounded? speedMultiplier : speedMultiplier * speedJumpMultiplier;
        rb.velocity = new Vector3(direction.x * speed * speedMultiplier, rb.velocity.y, direction.z * speed * speedMultiplier);
    }

    private void moveCamera()
    {
        aimMultiplier = pW.Aiming? aimingMultiplier : 1f;

        transform.Rotate(new Vector3(0, lookDir.x * sensibility * aimMultiplier * Time.fixedDeltaTime, 0));
        
        xRotation -= lookDir.y * sensibility * aimMultiplier * Time.fixedDeltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    public void Jump()
    {
        if (!grounded)
            return;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheckPosition.position, length);
    }
}
