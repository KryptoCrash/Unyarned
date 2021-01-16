using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossEnemyAI : MonoBehaviour {
    // Stats
    public float maxJumpHeight;
    public float walkSpeed;
    public float jumpForce;
    public float movementSmoothFactor;

    public bool onGround;

    // Weapon
    public GameObject weapon;
    private WeaponStatsManager weaponStats;
    public GameObject misslePrefab;

    // Timer
    public float attackTimer;
    public float missleTimer;

    // Pathfinder
    private EnemyPathfinding pathfinder;

    // Physics
    private Rigidbody2D rb;
    private Vector2 curVelocity;

    // Animator
    public GameObject gfx;
    private Animator anim;

    // Start is called before the first frame update
    void Start() {
        pathfinder = GetComponent<EnemyPathfinding>();

        weaponStats = weapon.GetComponent<WeaponStatsManager>();

        rb = GetComponent<Rigidbody2D>();

        anim = gfx.GetComponent<Animator>();
    }

    void FixedUpdate() {
        // Missle Timer Update
        missleTimer += Time.deltaTime;

        if (missleTimer >= 10f) {
            ShootMissles();

            missleTimer = 0f;
        }

        if (pathfinder.haveToPhase) {
            pathfinder.haveToPhase = false;

            // Invert player y velocity to preserve momentum
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);

            // Invert gravity for player
            rb.gravityScale *= -1;

            // Invert scale and pos
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(1f, -1f, 1f));
            transform.position = Vector3.Scale(transform.position, new Vector3(1f, -1f, 1f));
        }

        Vector2 force = pathfinder.force.normalized;

        // Apply speed scalars to force.x
        force.x *= walkSpeed;

        // Create movement vector
        Vector2 moveDir = new Vector2(force.x, rb.velocity.y);

        // Smooth movement vector
        moveDir = Vector2.SmoothDamp(rb.velocity, moveDir, ref curVelocity, movementSmoothFactor);

        // Apply movement vector as new velocity
        rb.velocity = moveDir;

        // Jumping
        if (onGround && pathfinder.force.y > 5f && pathfinder.force.y < maxJumpHeight) {
            Vector2 jumpForceDir = new Vector2(0f, jumpForce * rb.gravityScale);
            rb.AddForce(jumpForceDir);
        }

        // Attacking (when enemy is close to player and not on cooldown)
        if (pathfinder.reachedEndOfPath && attackTimer <= 0f) {
            // Play attack animation

            // Check if player is in attack range
            Collider2D[] hitObjs = Physics2D.OverlapCircleAll(weapon.transform.position, weaponStats.range);

            foreach (Collider2D hitObj in hitObjs) {
                if (hitObj.gameObject.tag == "Player") {
                    // Damage player
                    hitObj.gameObject.GetComponent<StatsManager>().health -= weaponStats.attackDamage;

                    // Knockback
                    hitObj.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(moveDir.x * weaponStats.knockback, 200));
                }
            }

            attackTimer = weaponStats.attackRate;
        } else if (attackTimer > 0f) attackTimer -= Time.deltaTime;
    }

    private void ShootMissles() {
        for (int i = 0; i < 3f; i++) {
            Instantiate(misslePrefab, new Vector3(21.78f, Random.Range(-10, 10), 0), Quaternion.identity);
        }

        for (int i = 0; i < 3f; i++) {
            Instantiate(misslePrefab, new Vector3(41.834f, Random.Range(-10, 10), 0), Quaternion.identity);
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
        if (collision.gameObject.tag == "Ground") {
            // Ground exit collision
            onGround = false;
        }
    }
}
