/*
source file name: PlayerBehaviour.cs
name: chaewan woo
student number: 101354291
date last modified: 2023/10/20
program description: player behaviour
Revision History: It's on the git repo
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Player Properties")]
    public float speed = 2.0f;
    private Boundary boundary;
    private float horizontalPosition;
    public float horizontalSpeed = 10.0f;
    public bool usingMobileInput = false;

    [Header("Bullet Properties")] 
    public Transform bulletSpawnPoint;
    public float fireRate = 0.2f;

	private SpriteRenderer m_spriteRenderer;
	private new Camera camera;

    private ScoreManager scoreManager;
    private BulletManager bulletManager;

    void Start()
    {
		bulletManager = FindObjectOfType<BulletManager>();

		m_spriteRenderer = GetComponent<SpriteRenderer>();

		camera = Camera.main;

        usingMobileInput = Application.platform == RuntimePlatform.Android ||
                           Application.platform == RuntimePlatform.IPhonePlayer;

        scoreManager = FindObjectOfType<ScoreManager>();

        InvokeRepeating("FireBullets", 0.0f, fireRate);

		if (m_spriteRenderer != null)
		{
			SetPlayerBoundaryCameraPerspective();
			SetPlayerPositionCameraPerspective();
		}
	}

    void SetPlayerBoundaryCameraPerspective()
    {
		float playerHalfSize = m_spriteRenderer.bounds.size.y * 0.5f;
		Vector3 pos = camera.transform.position;
		boundary.min = pos.y - camera.orthographicSize + playerHalfSize;
		boundary.max = pos.y + camera.orthographicSize - playerHalfSize;
	}

    void SetPlayerPositionCameraPerspective()
	{
		float playerSize = m_spriteRenderer.bounds.size.x;
		Vector3 pos = camera.transform.position;
		float startingPosX = pos.x - camera.orthographicSize * camera.aspect + playerSize;   // left-hand side of the screen
		float startingPosY = (boundary.min + boundary.max) * 0.5f;  // y = 0 camera's perspective position.
		transform.position = new Vector3(startingPosX, startingPosY, 0);

		horizontalPosition = startingPosX;
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
            transform.position = Vector2.Lerp(transform.position, destination, Time.deltaTime * horizontalSpeed);
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
