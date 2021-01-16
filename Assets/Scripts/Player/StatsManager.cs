using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour {
    public float health;

    private void Update() {
        if (health <= 0f) {
            gameObject.SetActive(false);
        }
    }
}
