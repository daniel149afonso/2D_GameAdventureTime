using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{   
    //Cr�er une instance de Rigidbody + Animator + Collider + Sprite
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D coll;
    private SpriteRenderer sprite; //change direction du Sprite

    // Touches de d�placement
    bool moveRight = false;
    bool moveLeft = false;

    //Etats de saut
    private bool isJumping;
    private bool isHoldingJump;  // Indique si le bouton de saut est maintenu
    private float jumpTimeCounter;  // Compteur de temps de saut
    
    //Constante de vitesse x et de saut y
    [SerializeField] private float speedMove = 10f;     // La vitesse lat�rale
    [SerializeField] private float jumpForce = 10f;     // La force initiale du saut
    [SerializeField] private float holdJumpForce = 5f;  // La force suppl�mentaire pendant un saut long
    [SerializeField] private float maxJumpTime = 0.2f;  // Dur�e maximale pendant laquelle on peut maintenir pour un saut long
    [SerializeField] private LayerMask jumpableGround; //V�rifie si peut sauter

    //Cr�er des �tats d'animations
    private enum MovementState { idle, running, jumping, falling };
    private MovementState state;

    //Audio Jump Effect
    [SerializeField] private AudioSource jumpSoundEffect;
    
    // Start is called before the first frame update
    private void Start()
    {
        //Cr�er une r�f�rence vers Rigibody, Animator, Collider et Sprite
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
       // ArrowBoard();
        Movements();
        Animations();
        HandleJump();
    }

    private void HandleJump()
    {
        if (isHoldingJump && jumpTimeCounter > 0)
        {
            // Ajout de force pour un saut prolong�
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, holdJumpForce);
            jumpTimeCounter -= Time.deltaTime;
        }
        else
        {
            // Arr�te la prolongation du saut si le temps est �coul�
            isHoldingJump = false;
        }
    }

    private void ArrowBoard()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveLeft = true;
            moveRight = false;
            Debug.Log("Left pressed");
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveLeft = false;
            moveRight = true;
            Debug.Log("Right pressed");
        }
    }
    public void PointerDownRightButton()
    {
        moveRight = true;
        moveLeft = false; // D�sactive l'autre direction
    }

    public void PointerDownLeftButton()
    {
        moveLeft = true;
        moveRight = false; // D�sactive l'autre direction
    }

    public void PointerUpButton()
    {
        // Rel�chement de tous les boutons, arr�t du mouvement
        moveRight = false;
        moveLeft = false;
    }

    public void PointerEnterRightButton()
    {
        // Si vous glissez sur le bouton droit, d�sactivez le gauche
        moveRight = true;
        moveLeft = false;
    }

    public void PointerEnterLeftButton()
    {
        // Si vous glissez sur le bouton gauche, d�sactivez le droit
        moveLeft = true;
        moveRight = false;
    }

    public void PointerUpJumpButton()
    {
        isHoldingJump = false; // Arr�te l'ajout de force pour un saut long
    }

    public void PointerDownJumpButton()
    {
        if (IsGrounded() && rb.bodyType == RigidbodyType2D.Dynamic)
        {
            // D�clenche le saut initial avec la force de base
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);    //D�placement: Vitesse = Vx ; Vy
            jumpSoundEffect.Play();
            isJumping = true;
            isHoldingJump = true; // Indique que le joueur maintient le bouton pour un saut long
            jumpTimeCounter = maxJumpTime; // Initialise le compteur pour la dur�e du saut long
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
                rb.linearVelocity = new Vector2(right * speedMove, rb.linearVelocity.y);//dirX est soit + ou -
            }
            else if (moveLeft)
            {
                float left = -1f;
                rb.linearVelocity = new Vector2(left * speedMove, rb.linearVelocity.y);//dirX est soit + ou -
            }
            else
            {
                float idle = 0f;
                rb.linearVelocity = new Vector2(idle * speedMove, rb.linearVelocity.y);//dirX est soit + ou -
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

        if (rb.linearVelocity.y > minJumpVelocity)
        {
            state = MovementState.jumping;
        }
        else if (rb.linearVelocity.y < minFallVelocity)
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


