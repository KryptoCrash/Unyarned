using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour {
    // Enemy movement stats
    public float moveSpeed;
    public float nextWaypointDistance;
    public float trackingDistance;

    private int currentWaypoint;
    public bool reachedEndOfPath;
    public bool haveToPhase;

    // Enemy GFX
    public GameObject enemyGFX;

    // Pathfinding
    public Transform target;
    private Path path;
    private Seeker seeker;

    // Physics
    public Vector2 force;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start() {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").GetComponent<Transform>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void FixedUpdate() {

        if (path == null) return;

        // Check if path has completed
        reachedEndOfPath = currentWaypoint >= path.vectorPath.Count;

        if (reachedEndOfPath) return;

        // Get force
        Vector2 moveDir = ((Vector2)path.vectorPath[currentWaypoint] - (Vector2) transform.position).normalized;
        force = moveDir * moveSpeed * Time.deltaTime;

        // Increment to next waypoint if it was reached
        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) currentWaypoint++;

        // Flip enemy when moving in the other direction
        if (force.x < 0f) {
            enemyGFX.transform.localScale = new Vector3(-1, 1, 1);
        } else if (force.x > 0f) {
            enemyGFX.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void UpdatePath() {
        haveToPhase = (transform.position.y < 0 && target.position.y > 0) || (transform.position.y >= 0 && target.position.y <= 0);

        bool closeEnough = Vector2.Distance(transform.position, target.position) < trackingDistance;
        bool onSameSide = (transform.position.y < 0 && target.position.y < 0) || (transform.position.y > 0 && target.position.y > 0);

        if (seeker.IsDone() && closeEnough && onSameSide) {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        } else {
            path = null;
            currentWaypoint = 0;
        }
    }

    void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }
}
