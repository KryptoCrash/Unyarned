using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGFXScript : MonoBehaviour {
    // AI
    public AIPath aiPath;

    // Managers
    private SpriteScript spriteManager;

    // Start is called before the first frame update
    void Start() {
        spriteManager = GetComponent<SpriteScript>();
    }

    // Update is called once per frame
    void Update() {
        // Flip enemy when moving in the other direction
        if (aiPath.desiredVelocity.x < 0f) {
            transform.localScale = new Vector3(-1, 1, 1);
        } else if (aiPath.desiredVelocity.x > 0f) {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
