using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Configuration parameters
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float health = 200f;


    [Header("Projectile")]
    [SerializeField] GameObject cannonBall;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] float deathSoundVolume = 0.5f;
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.1f;

    Coroutine firingCoroutine;
    Level level;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
        level = FindObjectOfType<Level>();
    }

   

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(fireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }


    IEnumerator fireContinuously()
    {
        while (true) {
            GameObject ball = Instantiate(cannonBall, transform.position, Quaternion.identity) as GameObject;
            ball.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + 0.5f;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - 0.5f;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + 0.6f;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - 0.6f;
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        
        var newXPos = Mathf.Clamp(transform.position.x + deltaX,xMin,xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY,yMin,yMax);

        transform.position = new Vector2(newXPos,newYPos);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other + " " + gameObject);
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.getDamage();
            
        if (health <= 0)
        {
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
            level.LoadGameOver();
        }

        damageDealer.hit();
    }

    public float GetHealth()
    {
        return health;
    }
}
