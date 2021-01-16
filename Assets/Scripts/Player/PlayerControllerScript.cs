using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour {
    // Player movement stats
    public float walkSpeed;
    public float crouchSpeed;
    public float runSpeed;
    public float movementSmoothFactor;
    public float jumpForce;
    public float holdJumpForce;
    public float dashForce;
    public float warpCooldown;
    public float dashCooldown;

    // Player vectors
    public float playerFacing;

    // Physics
    private Vector2 curVelocity = Vector2.zero;

    // Particles system
    private PlayerParticleManager particleManager;

    // Audio Manager
    private AudioManagerScript audioManager;

    // Conditionals
    public bool onGround;
    public bool canWarp;
    public bool onWarpCooldown;
    public bool onDashCooldown;

    // Timers
    public float warpTimer;
    public float dashTimer;

    // Components
    private Rigidbody2D rb;
    private Animator anim;

    // Shadow animator
    public Animator shadowAnim;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        particleManager = GetComponent<PlayerParticleManager>();
        audioManager = GetComponent<AudioManagerScript>();
    }

    void Update() {
        if (onWarpCooldown) {
            // Manage cooldown for warping
            warpTimer += Time.deltaTime;

            if (warpTimer >= warpCooldown) {
                onWarpCooldown = false;
                warpTimer = 0f;
            }
        }
    }

    private void FixedUpdate() {
        anim.SetFloat("speed", Math.Abs(rb.velocity.x));
        shadowAnim.SetFloat("speed", Math.Abs(rb.velocity.x));
    }

    // Calculate how to move the player
    public void Move(Vector2 input, float rawHor, bool jumping, bool crouching, bool running, bool dashing) {
        // Set direction player is facing to raw input
        if (rawHor != 0f) {
            playerFacing = rawHor;
            transform.localScale = new Vector3(playerFacing * Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Apply speed scalars to input.x
        input.x *= walkSpeed;

        // Create movement vector
        Vector2 moveDir = new Vector2(input.x, rb.velocity.y);

        // Smooth movement vector
        moveDir = Vector2.SmoothDamp(rb.velocity, moveDir, ref curVelocity, movementSmoothFactor);

        // Apply movement vector as new velocity
        rb.velocity = moveDir;

        // Jumping
        if (onGround && input.y > 0 && jumping) {
            Vector2 jumpForceDir = new Vector2(0f, input.y * jumpForce * rb.gravityScale);
            rb.AddForce(jumpForceDir);
            anim.SetTrigger("jump");
            shadowAnim.SetTrigger("jump");
            audioManager.Jump();
        } else if (input.y > 0 && rb.velocity.y >= 1.0f) {
            Vector2 jumpForceDir = new Vector2(0f, input.y * holdJumpForce * rb.gravityScale);
            rb.AddForce(jumpForceDir);
        } else if (rb.velocity.y <= 0.0f) {
            rb.velocity -= Vector2.up * 0.05f * Time.deltaTime;
        }

        // Dashing
        if (dashing && !onDashCooldown) {
            audioManager.Dash();
            particleManager.TurnOnDashPS();
            onDashCooldown = true;
        } else if (onDashCooldown) {
            dashTimer += Time.deltaTime;
            if (dashTimer <= 0.5f) {
                rb.velocity = new Vector2(dashForce * playerFacing * Sigmoid(10 - dashTimer * 20), 0f);
            }

            if (dashTimer > dashCooldown) {
                rb.velocity = Vector2.zero;
                particleManager.TurnOffDashPS();
                onDashCooldown = false;
                dashTimer = 0f;
            }
        }
    }

    // Manage player and player shadow while warping
    public void Warp(GameObject shadow) {
        // Play warp sound
        audioManager.Warp();

        // Invert player y velocity to preserve momentum
        rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);

        // Invert gravity for player
        rb.gravityScale *= -1;

        // Warp particles
        particleManager.TeleportOut(transform.position);
        particleManager.TeleportIn(shadow.transform.position);

        // Swap player and shadow positions
        Vector3 tempPosition = transform.position;
        transform.position = shadow.transform.position;
        shadow.transform.position = tempPosition;

        // Flip player particles
        particleManager.Flip();

        // Start cooldown to wait for next warp
        onWarpCooldown = true;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {

            // Ground collision
            if (collision.contacts.Any(contact => contact.normal.y != 0)) {
                audioManager.Ground();
                onGround = true;
            }
            // Wall collision
            if (collision.contacts.Any(contact => contact.normal.x != 0)) {
                // Wall jump code here
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            // Ground collision
            if (collision.contacts.Any(contact => contact.normal.y != 0)) {
                onGround = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        // Ground Exit Collision
        if (collision.gameObject.tag == "Ground") {
            onGround = false;
        }
    }

    private float Sigmoid(float value) {
        float k = (float) Math.Exp(value);
        return k / (1.0f + k);
    }
}
