using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("EnemyConfig")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 150;

    [Header("Shots")]
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = 2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [Header("EnemyBallConfig")]
    [SerializeField] GameObject enemyBall;
    [SerializeField] float projectileSpeed = 10f;
    [Header("EnemyDestroy" +
        "Config")]
    [SerializeField] GameObject particlesOnDestroy;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0,1)] float deathSoundVolume = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();

    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }


    private void Fire()
    {
        GameObject ball = Instantiate(enemyBall, transform.position, Quaternion.identity) as GameObject;
        ball.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -projectileSpeed);
    }






    private void OnTriggerEnter2D(Collider2D other)
    {
       
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();

        if (!damageDealer) { return; }

        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.getDamage();

        if (health <= 0)
        {
            
            FindObjectOfType<GameSession>().AddToScore(scoreValue);
            Destroy(gameObject);
            GameObject particles = Instantiate(particlesOnDestroy, transform.position, Quaternion.identity) as GameObject;
            Destroy(particles, 1f);
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        }

        
    }
}
