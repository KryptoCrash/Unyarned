using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerScript : MonoBehaviour {
    // Imported Prefabs
    public GameObject shadowPrefab;

    // Shadow
    public GameObject shadow;

    // Sub-Managers
    private PlayerControllerScript controller;
    private PlayerCombatScript combatManager;
    private ShadowControllerScript shadowController;
    public StatsManager stats;

    // Start is called before the first frame update
    void Start() {
        // Spawn game objects from prefabs
        shadow = Instantiate(shadowPrefab);

        // Get script components for all sub-managers
        controller = GetComponent<PlayerControllerScript>();
        controller.shadowAnim = shadow.GetComponent<Animator>();
        combatManager = GetComponent<PlayerCombatScript>();
        stats = GetComponent<StatsManager>();
        shadowController = shadow.GetComponent<ShadowControllerScript>();
    }

    // Update is called once per frame
    void Update() {
        // Get input from player
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxisRaw("Vertical"));
        float rawInput = Input.GetAxisRaw("Horizontal");
        bool shooting = Input.GetKeyDown(KeyCode.LeftShift);
        bool hitting = Input.GetKeyDown(KeyCode.RightShift);
        bool jumping = Input.GetKeyDown(KeyCode.W);
        bool crouching = Input.GetKey(KeyCode.Z);
        bool running = Input.GetKey(KeyCode.X);
        bool dashing = Input.GetKey(KeyCode.C);
        bool warping = Input.GetKeyDown(KeyCode.V);

        // Hand off movement to player controller
        controller.Move(input, rawInput, jumping, crouching, running, dashing);

        // Check if player can warp before warping
        if (warping && controller.canWarp && !controller.onWarpCooldown) {
            // Hand off warp mechanic to controller
            controller.Warp(shadow);

            // Flip sprites
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(1f, -1f, 1f));
            shadow.transform.localScale = Vector3.Scale(shadow.transform.localScale, new Vector3(1f, -1f, 1f));
        }

        // Hand off combat to player combat manager
        if (hitting) combatManager.Attack(controller.playerFacing);
        if (shooting) combatManager.Shoot(controller.playerFacing);
    }
}
