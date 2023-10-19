using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundStarsBehaviour : MonoBehaviour
{
    public float verticalSpeed;
    public Boundary boundary;
    

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckBounds();
    }

    public void Move()
    {
        transform.position -= new Vector3(0.0f, verticalSpeed * Time.deltaTime);
    }

    public void CheckBounds()
    {
        if (transform.position.y < boundary.min)
        {
            ResetStars();
        }
    }

    public void ResetStars()
    {
        transform.position = new Vector2(0.0f, boundary.max);
    }
}
