using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody PlayerRb;
    public float MovementSpeed = 1.0f;
    float forwardMove;
    float sidewaysMove;
    // Start is called before the first frame update
    void Start()
    {
        PlayerRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
       
    }
    public void FixedUpdate()
    {
        Movement();

    }
    public void Movement()
    {
        Vector3 moveVector = new Vector3(Input.GetAxis("Horizontal") * 2, 0, Input.GetAxis("Vertical") * 3);
        Vector3 baseVelocity = Vector3.zero;
        Vector3 moveDirection = transform.forward * moveVector.z + transform.right * moveVector.x;
        baseVelocity.y = PlayerRb.velocity.y;
        PlayerRb.velocity = moveDirection * MovementSpeed + baseVelocity;
    }

}