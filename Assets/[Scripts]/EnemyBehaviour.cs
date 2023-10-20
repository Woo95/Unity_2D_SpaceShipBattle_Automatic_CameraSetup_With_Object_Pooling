/*
source file name: EnemyBehaviour.cs
name: chaewan woo
student number: 101354291
date last modified: 2023/10/20
program description: background image behaviour
Revision History: It's on the git repo
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBehaviour : MonoBehaviour
{
    private Boundary horizontalRespawnBoundary;
    private Boundary verticalBoundary;
    private Boundary horizontalBoundary;
    public float horizontalSpeed;
    public float verticalSpeed;
    public Color randomColor;

    [Header("Bullet Properties")]
    public Transform bulletSpawnPoint;
    public float fireRate = 0.2f;
    
    private BulletManager bulletManager;
    private SpriteRenderer spriteRenderer;

    private new Camera camera;

    private Transform trans;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bulletManager = FindObjectOfType<BulletManager>();

        camera = Camera.main;

        trans = transform;

        SetEnemyBoundaryCameraPerspective();

		ResetEnemy();
		InvokeRepeating("FireBullets", 0.3f, fireRate);
	}

	void SetEnemyBoundaryCameraPerspective()
	{
        float enemySizeY = spriteRenderer.bounds.size.y;

		Vector3 pos = camera.transform.position;

		verticalBoundary.min = pos.y - camera.orthographicSize + enemySizeY * 0.71f;    // multiplied 0.71f for more percise verticalBoundary
		verticalBoundary.max = pos.y + camera.orthographicSize - enemySizeY * 0.71f;

		horizontalBoundary.min = pos.x - camera.orthographicSize * camera.aspect - enemySizeY;
		horizontalBoundary.max = pos.x + camera.orthographicSize * camera.aspect + enemySizeY;

        horizontalRespawnBoundary.min = horizontalBoundary.max;
		horizontalRespawnBoundary.max = horizontalBoundary.max + enemySizeY * 2.0f;
	}

	// Update is called once per frame
	void Update()
    {
        Move();
        CheckBounds();
    }

	float verticalDirection = 1;
	public void Move()
    {
		trans.position -= new Vector3(horizontalSpeed, verticalSpeed * verticalDirection, trans.position.z) * Time.deltaTime;

        if (trans.position.y < verticalBoundary.min || trans.position.y > verticalBoundary.max)
            verticalDirection *= -1;
	}

    public void CheckBounds()
    {
        if (transform.position.x < horizontalBoundary.min) // out of the map -> reset enemy
        {
            ResetEnemy();
        }
    }

    public void ResetEnemy()
    {
        var RandomXPosition = Random.Range(horizontalRespawnBoundary.min, horizontalRespawnBoundary.max);
        var RandomYPosition = Random.Range(verticalBoundary.min, verticalBoundary.max);
        horizontalSpeed = Random.Range(3.0f, 9.0f);
        verticalSpeed = Random.Range(3.0f, 6.0f);
		trans.position = new Vector3(RandomXPosition, RandomYPosition, 0.0f);

        List<Color> colorList = new List<Color>() {Color.red, Color.yellow, Color.magenta, Color.cyan, Color.white, Color.white};

        randomColor = colorList[Random.Range(0, 6)];
        spriteRenderer.material.SetColor("_Color", randomColor);
    }

    void FireBullets()
    {
        var bullet = bulletManager.GetBullet(bulletSpawnPoint.position, BulletType.ENEMY);
    }
}
