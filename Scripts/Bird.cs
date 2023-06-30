using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask hunterLayer;
    [SerializeField] private int remainingLives = 3; // Pøidán atribut [SerializeField] pro editaci poètu životù v Unity Editoru

    private Vector2 startPosition;  // Startovací pozice ptáka

    private void Start()
    {
        startPosition = transform.position;  // Uložení startovací pozice ptáka
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hunterLayer == (hunterLayer | (1 << collision.gameObject.layer)))
        {
            LoseLife();
        }
    }

    private void LoseLife()
    {
        remainingLives--;
        if (remainingLives <= 0)
        {
            Die();
        }
        else
        {
            Respawn();
        }
    }

    private void Die()
    {
        // Zde provedeš akci po smrti objektu "Bird"
        Destroy(gameObject);
    }

    private void Respawn()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = startPosition;

        // Další akce, které chceš provést pøi respawnu
    }
}
