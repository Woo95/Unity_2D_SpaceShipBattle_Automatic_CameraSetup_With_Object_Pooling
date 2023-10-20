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

    private new Camera camera;
    private float rotation = 90.0f;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
		SetBackgroundPositionCameraPerspective();
    }

    void SetBackgroundPositionCameraPerspective()
    {
		SpriteRenderer starRenderer = starPrefab.GetComponent<SpriteRenderer>();
		if (starRenderer == null)
            return;
        
        float spriteSizeX = starRenderer.bounds.size.x;
		float spriteSizeY = starRenderer.bounds.size.y;

		float cameraHalfWidth = camera.orthographicSize * camera.aspect;
		float cameraHalfHeight = camera.orthographicSize;

		Vector3 cameraPos = camera.transform.position;

		float startingPosX = cameraPos.x - cameraHalfWidth - spriteSizeX;
		float initialStratingPosY = cameraPos.y - cameraHalfHeight;

		while (startingPosX < cameraHalfWidth)
		{
			float startingPosY = initialStratingPosY;
			while (startingPosY < cameraHalfHeight)
			{
				InstantiateBackgroundStar(startingPosX, startingPosY);
				startingPosY += spriteSizeY;
			}
			startingPosX += spriteSizeX;
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