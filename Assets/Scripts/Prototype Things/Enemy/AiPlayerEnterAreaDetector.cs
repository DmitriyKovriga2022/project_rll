using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPlayerEnterAreaDetector : MonoBehaviour
{
    public Transform player;
    private Rigidbody2D rb;

    private Vector2 movement;
    
    public float speed = 2;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("MainPlayer").GetComponent<Transform>();
    }

    private void Update()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        movement = direction;
    }

    private void FixedUpdate()
    {
        moveCharacter(movement);
    }

    void moveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }


}
