using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovementScript : MonoBehaviour {
    // Speed/Mag of movement vector
    public float bulletSpeed;

    public float bulletTimer;

    // Define direction for movement
    public Vector2 moveDir;

    // Weapon Stats
    private WeaponStatsManager weaponStats;

    // Components
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        weaponStats = GetComponent<WeaponStatsManager>();

        rb.AddForce(moveDir * bulletSpeed);
    }

    // Update is called once per frame
    void Update() {
        bulletTimer += Time.deltaTime;

        // If bullet stops or timer runs out, destroy bullet
        if (rb.velocity == Vector2.zero || bulletTimer > 1.5f) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            // Damage
            collision.gameObject.GetComponent<StatsManager>().health -= weaponStats.attackDamage;

            // Knockback
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(moveDir.x * weaponStats.knockback, 200 * (transform.position.y < 0 ? -1 : 1)));
        }
    }
}
