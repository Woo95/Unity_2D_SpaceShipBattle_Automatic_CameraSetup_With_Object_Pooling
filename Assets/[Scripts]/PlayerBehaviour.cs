using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Player Properties")]
    public float speed = 2.0f;
    public Boundary boundary;
    public float horizontalPosition;
    public float verticalSpeed = 10.0f;
    public bool usingMobileInput = false;

    [Header("Bullet Properties")] 
    public Transform bulletSpawnPoint;
    public float fireRate = 0.2f;
    

    private Camera camera;
    private ScoreManager scoreManager;
    private BulletManager bulletManager;

    void Start()
    {
        bulletManager = FindObjectOfType<BulletManager>();

        camera = Camera.main;

        usingMobileInput = Application.platform == RuntimePlatform.Android ||
                           Application.platform == RuntimePlatform.IPhonePlayer;

        scoreManager = FindObjectOfType<ScoreManager>();

        InvokeRepeating("FireBullets", 0.0f, fireRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (usingMobileInput)
        {
            MobileInput();
        }
        else
        {
            ConventionalInput();
        }
        
        Move();

        if (Input.GetKeyDown(KeyCode.K))
        {
            scoreManager.AddPoints(10);
        }

    }

    public void MobileInput()
    {
        foreach (var touch in Input.touches)
        {
            var destination = camera.ScreenToWorldPoint(touch.position);
            transform.position = Vector2.Lerp(transform.position, destination, Time.deltaTime * verticalSpeed);
        }
    }

    public void ConventionalInput()
    {
        float y = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;
        transform.position += new Vector3(0.0f, y, 0.0f);
    }
    
    public void Move()
    {
        float clampedPosition = Mathf.Clamp(transform.position.y, boundary.min, boundary.max);
        transform.position = new Vector2(horizontalPosition, clampedPosition);
    }

    void FireBullets()
    {
        var bullet = bulletManager.GetBullet(bulletSpawnPoint.position, BulletType.PLAYER);
    }
}
