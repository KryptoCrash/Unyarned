using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleManager : MonoBehaviour {
    public GameObject dashParentGO;
    public GameObject dashLineGO;
    public GameObject dashSwirlGO;

    private ParticleSystem _dashParent;
    private ParticleSystem _dashLine;
    private ParticleSystem _dashSwirl;
    
    public GameObject playerTrailGO;

    private ParticleSystem _playerTrail;

    public GameObject attackGO;
    public GameObject attackHitGO;

    private ParticleSystem _attack;
    private ParticleSystem _attackHit;

    public GameObject teleportInGO;
    public GameObject teleportOutGO;

    private ParticleSystem _teleportIn;
    private ParticleSystem _teleportOut;

    public GameObject greenKeyGetGO;
    public GameObject greenKeyUseGO;
    public GameObject redKeyGetGO;
    public GameObject redKeyUseGO;

    private ParticleSystem _greenKeyGet;
    private ParticleSystem _greenKeyUse;
    private ParticleSystem _redKeyGet;
    private ParticleSystem _redKeyUse;

    // Start is called before the first frame update
    void Start() {
        _dashParent = dashParentGO.GetComponent<ParticleSystem>();
        _dashLine = dashLineGO.GetComponent<ParticleSystem>();
        _dashSwirl = dashSwirlGO.GetComponent<ParticleSystem>();

        _playerTrail = playerTrailGO.GetComponent<ParticleSystem>();

        _attack = attackGO.GetComponent<ParticleSystem>();
        _attackHit = attackHitGO.GetComponent<ParticleSystem>();

        _teleportIn = teleportInGO.GetComponent<ParticleSystem>();
        _teleportOut = teleportOutGO.GetComponent<ParticleSystem>();

        _greenKeyGet = greenKeyGetGO.GetComponent<ParticleSystem>();
        _greenKeyUse = greenKeyUseGO.GetComponent<ParticleSystem>();
        _redKeyGet = redKeyGetGO.GetComponent<ParticleSystem>();
        _redKeyUse = redKeyUseGO.GetComponent<ParticleSystem>();

        TurnOffDashPS();
    }

    // Update is called once per frame
    void Update() {
        //Set it to this when the player is on the ground so the playerTrail particles will start appearing
        var em = _playerTrail.emission;
        em.rateOverDistance = 1.5f;
        //Set it here when you don't want the particles to show
        em.rateOverDistance = 1.5f;
    }

    public void TurnOffDashPS() {
        //Gets the emission component of PS and turns the rate to a specified number. This one making all of them not do throw out anything
        var em = _dashParent.emission;
        em.rateOverTime = 0;
        var em1 = _dashLine.emission;
        em1.rateOverDistance = 0;
        var em2 = _dashSwirl.emission;
        em2.rateOverTime = 0;
    }

    public void TurnOnDashPS() {
        //Gets the emission component of PS and turns the rate to a specified number. This one making the gameobject start using the particles 
        var em = _dashParent.emission;
        em.rateOverTime = 500;
        var em1 = _dashLine.emission;
        em1.rateOverDistance = 5;
        var em2 = _dashSwirl.emission;
        em2.rateOverTime = 200;
    }

    public void Flip() {
        // Flip dash
        _dashParent.transform.localScale = Vector3.Scale(_dashParent.transform.localScale, new Vector3(1f, -1f, 1f));
        _dashLine.transform.localScale = Vector3.Scale(_dashLine.transform.localScale, new Vector3(1f, -1f, 1f));
        _dashSwirl.transform.localScale = Vector3.Scale(_dashSwirl.transform.localScale, new Vector3(1f, -1f, 1f));

        // Flip walk trail
        _playerTrail.transform.localScale = Vector3.Scale(_playerTrail.transform.localScale, new Vector3(1f, -1f, 1f));

        // Flip attack
        _attack.transform.localScale = Vector3.Scale(_attack.transform.localScale, new Vector3(1f, -1f, 1f));
        _attackHit.transform.localScale = Vector3.Scale(_attackHit.transform.localScale, new Vector3(1f, -1f, 1f));

        // Flip teleport
        _teleportIn.transform.localScale = Vector3.Scale(_teleportIn.transform.localScale, new Vector3(1f, -1f, 1f));
        _teleportOut.transform.localScale = Vector3.Scale(_teleportOut.transform.localScale, new Vector3(1f, -1f, 1f));
    }

    public void Attack(Vector3 pos) {
        //These commands are to use the attack/attackHit particles. Use these code to activate the particles
        //Attack Particle
        //Used to display the player is attacking
        _attack.transform.position = pos;
        _attack.Clear();
        _attack.Play();
    }

    public void AttackHit(Vector3 pos) {
        //Attack Hit Particle
        //Used to display that the player hit something
        _attackHit.transform.position = pos;
        _attackHit.Clear();
        _attackHit.Play();
    }

    public void TeleportIn(Vector3 pos) {
        _teleportIn.transform.position = pos;
        _teleportIn.Clear();
        _teleportIn.Play();
    }

    public void TeleportOut(Vector3 pos) {
        _teleportOut.transform.position = pos;
        _teleportOut.Clear();
        _teleportOut.Play();
    }

    public void GameOver(Vector3 pos) {

    }

    public void GetGreenKey(Vector3 pos) {
        _greenKeyGet.transform.position = pos;
        _greenKeyGet.Clear();
        _greenKeyGet.Play();
    }

    public void UseGreenKey(Vector3 pos) {
        _greenKeyUse.transform.position = pos;
        _greenKeyUse.Clear();
        _greenKeyUse.Play();
    }

    public void GetRedKey(Vector3 pos) {
        _redKeyGet.transform.position = pos;
        _redKeyGet.Clear();
        _redKeyGet.Play();
    }

    public void UseRedKey(Vector3 pos) {
        _redKeyUse.transform.position = pos;
        _redKeyUse.Clear();
        _redKeyUse.Play();
    }
}
