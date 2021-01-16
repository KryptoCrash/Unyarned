using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatScript : MonoBehaviour {
    // Timer
    public float attackTimer;
    public float rangeAttackTimer;
    public float rangeAttackCooldown;

    // Weapons
    public GameObject weapon;
    public GameObject bulletPrefab;

    // Enemy Layer
    public LayerMask enemyLayers;

    // Particle manager
    private PlayerParticleManager particleManager;

    // Audio Manager
    private AudioManagerScript audioManager;

    // Components
    private Animator anim;
    private WeaponStatsManager weaponStats;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
        weaponStats = weapon.GetComponent<WeaponStatsManager>();
        rb = GetComponent<Rigidbody2D>();

        particleManager = GetComponent<PlayerParticleManager>();
        audioManager = GetComponent<AudioManagerScript>();
    }

    private void Update() {
        // Count down cooldown timers
        if (attackTimer > 0f) {
            attackTimer -= Time.deltaTime;
        }

        if (rangeAttackTimer > 0f) {
            rangeAttackTimer -= Time.deltaTime;
        }
    }

    public void Attack(float playerFacing) {
        if (attackTimer <= 0f) {
            // Play attack animation
            anim.SetTrigger("attack");

            // Play attack sound
            audioManager.Attack();

            // Spawn attack particles
            particleManager.Attack(weapon.transform.position);

            // Get everything that overlaps weapon
            Collider2D[] hitObjs = Physics2D.OverlapCircleAll(weapon.transform.position, weaponStats.range, enemyLayers);

            foreach (Collider2D enemy in hitObjs) {
                // Spawn attack particles
                particleManager.AttackHit(enemy.transform.position);

                // Damage
                enemy.gameObject.GetComponent<StatsManager>().health -= weaponStats.attackDamage;

                // Knockback
                enemy.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(playerFacing * weaponStats.knockback, 200 * (transform.position.y < 0 ? -1 : 1)));
            }

            attackTimer = weaponStats.attackRate;
        }
    }

    public void Shoot(float playerFacing) {
        // Timer for cooldown
        if (rangeAttackTimer <= 0f) {
            // Play shoot sound
            audioManager.Shoot();

            // Create a bullet and make it face and move in the direction the player is facing
            GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(playerFacing, 0, 0), Quaternion.Euler(0, 0, playerFacing * 90));
            bullet.GetComponent<BulletMovementScript>().moveDir.x = playerFacing;

            rangeAttackTimer = rangeAttackCooldown;
        }
    }

    /*
     private void OnDrawGizmosSelected() {
        if (weapon == null || weaponStats == null) return;

        Gizmos.DrawWireSphere(weapon.transform.position, weaponStats.range);
    }
    */
}
