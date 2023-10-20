/*
source file name: BackgroundStarSpawner.cs
name: chaewan woo
student number: 101354291
date last modified: 2023/10/20
program description: background image spawner to fix all the camera size
Revision History: It's on the git repo
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundStarSpawner : MonoBehaviour
{
    public GameObject starPrefab;

	private SpriteRenderer m_starRenderer;
	private new Camera camera;
    private float rotation = 90.0f;

    // Start is called before the first frame update
    void Start()
    {
		m_starRenderer = starPrefab.GetComponent<SpriteRenderer>();

		camera = Camera.main;

		if (m_starRenderer != null)
		{
			SetBackgroundPositionCameraPerspective();
		}
    }

    void SetBackgroundPositionCameraPerspective()
    {
        float spriteSizeX = m_starRenderer.bounds.size.x;
		float spriteSizeY = m_starRenderer.bounds.size.y;

		Vector3 cameraPos = camera.transform.position;
		float cameraHalfWidth = camera.orthographicSize * camera.aspect;
		float cameraHalfHeight = camera.orthographicSize;

		float startX = cameraPos.x - cameraHalfWidth - spriteSizeX;
		float startY = cameraPos.y - cameraHalfHeight;

		float endX = cameraPos.x + cameraHalfWidth;
		float endY = cameraPos.y + cameraHalfHeight;

		for (float posX = startX; posX < endX; posX += spriteSizeX)
		{
			for (float posY = startY; posY < endY; posY += spriteSizeY)
			{
				InstantiateBackgroundStar(posX, posY);
			}
		}
	}

    void InstantiateBackgroundStar(float posX, float posY)
	{
		GameObject backgroundStar = Instantiate(starPrefab, new Vector3(posX, posY, 0), Quaternion.identity, transform);


		int randomSign = Random.Range(0, 2) * 2 - 1; // random value between 1 or -1
		rotation *= randomSign; // toggle the rotation for the current star randomly

		backgroundStar.transform.Rotate(Vector3.forward, rotation);

		backgroundStar.SetActive(true);
    }
}