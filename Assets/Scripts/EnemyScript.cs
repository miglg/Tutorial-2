using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
private float dirX;
private float moveSpeed;
private Rigidbody2D rb;
private bool facingRight = true;
private Vector3 localScale;

void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        moveSpeed = 2f;
    }
void FixedUpdate()
{
    rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    
    if (facingRight == false && dirX > 0)
    {
        Flip();
    }
    else if (facingRight == true && dirX < 0)
    {
        Flip();
    }
}
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Marker")) 
        {
            dirX *= -1f;
        }
    }
}