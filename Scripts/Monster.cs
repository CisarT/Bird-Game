using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] Sprite _deadSprite;
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] float _movementSpeed = 1f; // Rychlost pohybu
    [SerializeField] LayerMask _boxLayer; // Vrstva obsahuj�c� objekty "Box"
    [SerializeField] float _flipDelay = 0.5f; // Prodleva p�ed oto�en�m po n�razu do objektu "Box"

    bool _hasDied;
    bool _isMovingLeft; // Indikuje, zda se objekt pohybuje vlevo

    void Start()
    {
        _isMovingLeft = true; // Objekt za�ne pohybem doleva
    }

    void Update()
    {
        if (!_hasDied)
        {
            Move();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (ShouldDieFromCollision(collision))
        {
            Die();
        }
        else if (_boxLayer == (_boxLayer | (1 << collision.gameObject.layer)))
        {
            StartCoroutine(FlipAfterDelay());
        }
    }

    bool ShouldDieFromCollision(Collision2D collision)
    {
        if (_hasDied)
            return false;

        Bird bird = collision.gameObject.GetComponent<Bird>();
        if (bird != null)
            return true;

        if (collision.contacts[0].normal.y < -0.5)
            return true;

        return false;
    }

    void Die()
    {
        _hasDied = true;
        GetComponent<SpriteRenderer>().sprite = _deadSprite;
        _particleSystem.Play();

        StartCoroutine(DisappearAfterDelay(1f)); // Objekt zmiz� po 1 sekund�
    }

    IEnumerator DisappearAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    void Move()
    {
        Vector3 movement = Vector3.left * _movementSpeed * Time.deltaTime;

        if (!_isMovingLeft)
        {
            // Pohyb doprava
            movement = -movement;
        }

        transform.Translate(movement);
    }

    IEnumerator FlipAfterDelay()
    {
        yield return new WaitForSeconds(_flipDelay);
        FlipMovementDirection();
    }

    void FlipMovementDirection()
    {
        _isMovingLeft = !_isMovingLeft;

        // Oto�en� objektu ve sc�n�
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
