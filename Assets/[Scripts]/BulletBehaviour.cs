/*
source file name: BulletBehaviour.cs
name: chaewan woo
student number: 101354291
date last modified: 2023/10/20
program description: bullet behaviour
Revision History: It's on the git repo
*/
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ScreenBounds
{
    public Boundary horizontal;
    public Boundary vertical;
}

public class BulletBehaviour: MonoBehaviour
{
    [Header("Bullet Properties")]
    public BulletDirection bulletDirection;
    public float speed;
    private ScreenBounds bounds;
    public BulletType bulletType;

    private Vector3 velocity;
    private BulletManager bulletManager;

	private SpriteRenderer m_spriteRenderer;
	private new Camera camera;

    void Start()
    {
        bulletManager = FindObjectOfType<BulletManager>();

		m_spriteRenderer = GetComponent<SpriteRenderer>();

		camera = Camera.main;

        if (m_spriteRenderer != null)
            SetBulletBoundaryCameraPerspective();
	}

	void SetBulletBoundaryCameraPerspective()
	{
        float bulletSize = m_spriteRenderer.bounds.size.x;

		Vector3 pos = camera.transform.position;
		bounds.vertical.min = pos.y - camera.orthographicSize - bulletSize;
		bounds.vertical.max = pos.y + camera.orthographicSize + bulletSize;
		
		bounds.horizontal.min = pos.x - camera.orthographicSize * camera.aspect - bulletSize;
		bounds.horizontal.max = pos.x + camera.orthographicSize * camera.aspect + bulletSize;
	}

	void Update()
    {
        Move();
        CheckBounds();
    }

    void Move()
    {
        transform.position += velocity * Time.deltaTime;
    }

    void CheckBounds()
    {
        if ((transform.position.x > bounds.horizontal.max) ||
            (transform.position.x < bounds.horizontal.min) ||
            (transform.position.y > bounds.vertical.max) ||
            (transform.position.y < bounds.vertical.min))
        {
            bulletManager.ReturnBullet(this.gameObject, bulletType);
        }
    }

    public void SetDirection(BulletDirection direction)
    {
        switch (direction)
        {
            case BulletDirection.UP:
                velocity = Vector3.up * speed;
				transform.rotation = Quaternion.Euler(0, 0, 0);
				break;
            case BulletDirection.RIGHT:
                velocity = Vector3.right * speed;
				transform.rotation = Quaternion.Euler(0, 0, 270);
				break;
            case BulletDirection.DOWN:
                velocity = Vector3.down * speed; 
                transform.rotation = Quaternion.Euler(0, 0, 180);
				break;
            case BulletDirection.LEFT:
                velocity = Vector3.left * speed;
				transform.rotation = Quaternion.Euler(0, 0, 90);
				break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((bulletType == BulletType.PLAYER) ||
            (bulletType == BulletType.ENEMY && other.gameObject.CompareTag("Player")))
        {
            bulletManager.ReturnBullet(this.gameObject, bulletType);
        }
    }

}