using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{   
    //Créer une instance de Rigidbody + Animator + Collider + Sprite
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D coll;
    private SpriteRenderer sprite; //change direction du Sprite

    // Touches de déplacement
    bool moveRight = false;
    bool moveLeft = false;

    //Etats de saut
    private bool isJumping;
    private bool isHoldingJump;  // Indique si le bouton de saut est maintenu
    private float jumpTimeCounter;  // Compteur de temps de saut
    
    //Constante de vitesse x et de saut y
    [SerializeField] private float speedMove = 10f;     // La vitesse latérale
    [SerializeField] private float jumpForce = 10f;     // La force initiale du saut
    [SerializeField] private float holdJumpForce = 5f;  // La force supplémentaire pendant un saut long
    [SerializeField] private float maxJumpTime = 0.2f;  // Durée maximale pendant laquelle on peut maintenir pour un saut long
    [SerializeField] private LayerMask jumpableGround; //Vérifie si peut sauter

    //Créer des états d'animations
    private enum MovementState { idle, running, jumping, falling };
    private MovementState state;

    //Audio Jump Effect
    [SerializeField] private AudioSource jumpSoundEffect;
    
    // Start is called before the first frame update
    private void Start()
    {
        //Créer une référence vers Rigibody, Animator, Collider et Sprite
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        Movements();
        Animations();
        HandleJump();
    }

    private void HandleJump()
    {
        if (isHoldingJump && jumpTimeCounter > 0)
        {
            // Ajout de force pour un saut prolongé
            rb.velocity = new Vector2(rb.velocity.x, holdJumpForce);
            jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            // Arrête la prolongation du saut si le temps est écoulé
            isHoldingJump = false;
        }
    }
    public void PointerDownRightButton()
    {
        moveRight = true;
        moveLeft = false; // Désactive l'autre direction
    }

    public void PointerDownLeftButton()
    {
        moveLeft = true;
        moveRight = false; // Désactive l'autre direction
    }

    public void PointerUpButton()
    {
        // Relâchement de tous les boutons, arrêt du mouvement
        moveRight = false;
        moveLeft = false;
    }

    public void PointerEnterRightButton()
    {
        // Si vous glissez sur le bouton droit, désactivez le gauche
        moveRight = true;
        moveLeft = false;
    }

    public void PointerEnterLeftButton()
    {
        // Si vous glissez sur le bouton gauche, désactivez le droit
        moveLeft = true;
        moveRight = false;
    }

    public void PointerUpJumpButton()
    {
        isHoldingJump = false; // Arrête l'ajout de force pour un saut long
    }

    public void PointerDownJumpButton()
    {
        if (IsGrounded() && rb.bodyType == RigidbodyType2D.Dynamic)
        {
            // Déclenche le saut initial avec la force de base
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);    //Déplacement: Vitesse = Vx ; Vy
            jumpSoundEffect.Play();
            isJumping = true;
            isHoldingJump = true; // Indique que le joueur maintient le bouton pour un saut long
            jumpTimeCounter = maxJumpTime; // Initialise le compteur pour la durée du saut long
        }
    }

    private void Movements()
    {
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            //Advance, Back or Idle
            if (moveRight)
            {
                float right = 1f;
                rb.velocity = new Vector2(right * speedMove, rb.velocity.y);//dirX est soit + ou -
            }
            else if (moveLeft)
            {
                float left = -1f;
                rb.velocity = new Vector2(left * speedMove, rb.velocity.y);//dirX est soit + ou -
            }
            else
            {
                float idle = 0f;
                rb.velocity = new Vector2(idle * speedMove, rb.velocity.y);//dirX est soit + ou -
            }
        }

    }
    private void Animations()
    {
        //Player Animation running
        if (moveRight)//bolean
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (moveLeft)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        //Player Animation jumping
        float minJumpVelocity = 0.1f;
        float minFallVelocity = -0.1f;

        if (rb.velocity.y > minJumpVelocity)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < minFallVelocity)
        {
            state = MovementState.falling;
        }

        //Convert enum "state" to Int
        anim.SetInteger("state", (int)state);
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}


