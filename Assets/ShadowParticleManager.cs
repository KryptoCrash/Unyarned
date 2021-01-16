using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowParticleManager : MonoBehaviour {
    public GameObject shadowTrailGO;

    private ParticleSystem _shadowTrail;

    // Start is called before the first frame update
    void Start() {
        _shadowTrail = shadowTrailGO.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {
        _shadowTrail.transform.localScale = Vector3.Scale(transform.localScale, new Vector3(0.5f, -0.5f, 1f));
    }
}
