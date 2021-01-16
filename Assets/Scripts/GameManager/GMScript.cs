using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GMScript : MonoBehaviour {
    // Game stats
    public bool gameOver;

    // UI
    public TextMeshProUGUI healthBar;

    // Audio
    private AudioSource curAudio;
    public AudioSource mainTheme;
    public AudioSource bossTheme;

    // Player
    public GameObject player;
    private PlayerManagerScript playerManager;

    // Boss
    public GameObject bossDoor1;
    public GameObject bossDoor2;
    public GameObject bossPrefab;
    public GameObject boss;
    public StatsManager bossStats;
    public AudioSource bossDeath;

    // Start is called before the first frame update
    void Start() {
        playerManager = player.GetComponent<PlayerManagerScript>();
        curAudio = mainTheme;
        curAudio.Play();
    }

    // Update is called once per frame
    void Update() {
        if (!gameOver) {
            UpdateUI();

            if (playerManager.stats.health <= 0f) {
                playerManager.shadow.SetActive(false);
                GameOver();
            }
            if (boss != null && bossStats.health <= 0f) {
                EndBoss();
            }
        } else SceneManager.LoadScene("Title");
    }

    void UpdateUI() {
        healthBar.text = "Health: " + (int) Math.Ceiling(Math.Max(playerManager.stats.health, 0));
    }

    public void StartBoss() {
        if (boss == null) {

            // Why do I hear boss music?
            curAudio.Stop();
            curAudio = bossTheme;
            curAudio.Play();

            // Block player into boss room
            bossDoor1.SetActive(true);
            bossDoor2.SetActive(true);

            boss = Instantiate(bossPrefab);
            bossStats = boss.GetComponent<StatsManager>();
        }
    }

    public void EndBoss() {
        // Why don't I hear boss music?
        curAudio.Stop();
        curAudio = mainTheme;
        curAudio.Play();

        // Let player leave boss room
        bossDoor1.SetActive(false);

        // Play boss death sound
        bossDeath.Play();

        boss = null;
    }

    public void GameWon() {
        gameOver = true;
        healthBar.text = "You won!";
    }

    public void GameOver() {
        gameOver = true;
        healthBar.text = "You lost!";
    }
}
