using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMissleMovement : MonoBehaviour {
    // Stats
    public float speed;
    public float attackDamage;

    // Target
    public Transform target;

    // Physics
    private Rigidbody2D rb;

    // Particle
    public GameObject particles;

    // Start is called before the first frame update
    void Start() {
        target = GameObject.Find("Player").GetComponent<Transform>();

        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        Vector3 moveDir = (target.position - transform.position).normalized;
        moveDir.z = 0;
        rb.velocity = moveDir * speed;
        transform.up = moveDir;
        particles.transform.up = moveDir;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "PlayerWeapon") {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<StatsManager>().health -= attackDamage;
            Destroy(gameObject);
        }
    }
}
