/*
source file name: BackgroundStarsBehaviour.cs
name: chaewan woo
student number: 101354291
date last modified: 2023/10/20
program description: background image behaviour
Revision History: It's on the git repo
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundStarsBehaviour : MonoBehaviour
{
    public float horizontalSpeed;
    private Boundary boundary;

    private SpriteRenderer m_spriteRenderer;
	private new Camera camera;

	private void Start()
	{
		m_spriteRenderer = GetComponent<SpriteRenderer>();

		camera = Camera.main;

        if (m_spriteRenderer != null)
        {
            SetBackgroundBoundaryCameraPerspective();
		}
	}

	void SetBackgroundBoundaryCameraPerspective()
	{
		float spriteSize = m_spriteRenderer.bounds.size.x * 0.5f;
		Vector3 pos = camera.transform.position;
		boundary.min = pos.x - camera.orthographicSize * camera.aspect - spriteSize;
		boundary.max = pos.x + camera.orthographicSize * camera.aspect + spriteSize;
	}

	// Update is called once per frame
	void Update()
    {
        Move();
        CheckBounds();
    }

	public void Move()
    {
        transform.position -= new Vector3(horizontalSpeed * Time.deltaTime, 0.0f);
    }

    public void CheckBounds()
    {
        if (transform.position.x < boundary.min)
        {
            ResetStars();
        }
    }

    public void ResetStars()
    {
        transform.position = new Vector2(boundary.max, transform.position.y);
    }
}
