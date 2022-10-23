using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerScript : MonoBehaviour
{

    public float speed = 0;
    public float jumpAmount = 0;
    public Text countText;
    public Text livesText;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;
    public Animator anim;

    private Rigidbody2D rb;
    private float movementX;
    private float movementY;
    private int count;
    private int lives;
    private bool facingRight = true;
    private bool isGameOver;
    private bool IsJumping;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        lives = 3;
        count = 0;

        SetCountText();
        SetLivesText();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        musicSource.clip = musicClipTwo;
        musicSource.Play();
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

      void SetCountText()
    {
        countText.text = "Score: " + count.ToString();
        if (count >= 8)
        {
            winTextObject.SetActive(true);
            musicSource.clip = musicClipOne;
            musicSource.Play();
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            Destroy(gameObject.GetComponent<CapsuleCollider2D>()); 
        }
    }

    void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
        if (lives <= 0)
        {
            loseTextObject.SetActive(true);
            Destroy(this.gameObject);
        }
    }

   private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
        anim.SetFloat("Speed", Mathf.Abs(movementX));

        if (facingRight == false && movementX > 0)
        {
            Flip();
        }
        else if (facingRight == true && movementX < 0)
        {
            Flip();
        }
        
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
        
        if (isOnGround == false)
        {
            anim.SetBool("IsJumping", true);
        }
        else if (isOnGround == true)
        {
            anim.SetBool("IsJumping", false);
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
        if (collider.gameObject.CompareTag("Coin")) 
        {
            collider.gameObject.SetActive(false);
            count = count + 1;

            SetCountText();
        }
         else if (collider.gameObject.CompareTag("Enemy"))
        {
            collider.gameObject.SetActive(false);
            lives = lives - 1;

            SetLivesText();
        }
        if (count == 4 && !isGameOver)
        {
            lives = 3;
            SetLivesText();
            isGameOver = true;
            transform.position = new Vector3(50.0f, 0.0f, 0.0f);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround == true)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
            }
        }
    }
}
