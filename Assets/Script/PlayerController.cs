using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Player sprite
    private GameObject standingPlayer;
    private GameObject ballPlayer;
    
    //Player movement
    [Header("Player movement")]   
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;    
    [SerializeField] private LayerMask selectedLayerMask;   
    private Rigidbody2D myRigibody;
    [SerializeField] private Transform checkGroundPoint;
    private bool isGrounded;
    private bool isFlippedInX;
    private Transform myTransform;
    private float ballModeCounter;
    [SerializeField] private float waitForBallMode;
    [SerializeField] private float isGroundedRange;

    //Player animator    
    private Animator myAnimatorStandingPlayer;
    private Animator myAnimatorBallPlayer;
    private int IdSpeed;
    private int IdIsGrounded;
    private int IdShootArrow;
    private int IdCanDoubleJump;

    [Header("Player shoot")]
    [SerializeField] private ArrowController arrowController;
    private Transform transformArrowPoint;
    private Transform transformBombPoint;
    [SerializeField] private BombController bombController;

    //Player dust
    [Header("Player efects dust")]    
    [SerializeField] private GameObject dustJump;
    private Transform transformDustPoint;
    private bool isIdle;
    private bool canDoubleJump;

    //Player dash
    [Header("Player efects dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    private float dashCounter;
    [SerializeField] private float waitForDash;
    private float afterDashCounter;

    [Header("Player after image")]
    [SerializeField] private SpriteRenderer playerSR;
    [SerializeField] private SpriteRenderer afterImageSR;
    [SerializeField] private float afterImageLifeTime;
    [SerializeField] private Color afterImageColor;
    [SerializeField] private float afterImageTimeBetween;
    private float afterImageCounter;

    //Player Extras
    private PlayerExtrasTracker playerExtrasTracker;

    private void Awake()
    {
        myRigibody = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
        playerExtrasTracker = GetComponent<PlayerExtrasTracker>();
    }

    void Start()
    {
        standingPlayer = GameObject.Find("StandingPlayer");
        ballPlayer = GameObject.Find("BallPlayer");
        ballPlayer.SetActive(false);
        transformArrowPoint = GameObject.Find("ArrowPoint").GetComponent<Transform>();
        transformDustPoint = GameObject.Find("DustPoint").GetComponent<Transform>();
        checkGroundPoint = GameObject.Find("CheckGroundPoint").GetComponent<Transform>();
        transformBombPoint = GameObject.Find("BombPoint").GetComponent<Transform>();
        myAnimatorStandingPlayer = standingPlayer.GetComponent<Animator>();
        myAnimatorBallPlayer = ballPlayer.GetComponent<Animator>();
        IdSpeed = Animator.StringToHash("speedX");
        IdIsGrounded = Animator.StringToHash("isGrounded");
        IdShootArrow = Animator.StringToHash("shootArrow");
        IdCanDoubleJump = Animator.StringToHash("canDoubleJump");
    }

    void Update()
    {

        //Move();
        Dash();
        Jump();
        CheckAndSetDirection();
        Shoot();
        PlayDust();
        BallMode();

    }    

    private void Dash()
    {
        if(afterDashCounter > 0)
        {
            afterDashCounter -= Time.deltaTime;
        }
        else
        {
            if ((Input.GetButtonDown("Fire2") && standingPlayer.activeSelf) && playerExtrasTracker.CanDash)
            {
                dashCounter = dashTime;
                ShowAfterImage();
            }
            
        }        

        if(dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            myRigibody.velocity = new Vector2(dashSpeed * myTransform.localScale.x,myRigibody.velocity.y);
            afterImageCounter -= Time.deltaTime;
            if(afterImageCounter <= 0)
            {
                ShowAfterImage();
            }

            afterDashCounter = waitForDash;
        }
        else
        {
            Move();
        }

        
    }    

    private void Move()
    {
        float inputX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        myRigibody.velocity = new Vector2(inputX, myRigibody.velocity.y);
        if (standingPlayer.activeSelf)
        {
            myAnimatorStandingPlayer.SetFloat(IdSpeed, Mathf.Abs(myRigibody.velocity.x));
        }
        if(ballPlayer.activeSelf)
        {
            myAnimatorBallPlayer.SetFloat(IdSpeed, Mathf.Abs(myRigibody.velocity.x));
        }
        
    }

    private void Jump()
    {
        //isGrounded = Physics2D.OverlapCircle(checkGroundPoint.position, isGroundedRange, selectedLayerMask);
        isGrounded = Physics2D.Raycast(checkGroundPoint.position,Vector2.down,isGroundedRange,selectedLayerMask);
        if (Input.GetButtonDown("Jump") && (isGrounded || (canDoubleJump && playerExtrasTracker.CanDobleJump)))
        {

            if (isGrounded)
            {
                canDoubleJump = true;
                Instantiate(dustJump, transformDustPoint.position, Quaternion.identity);
            }
            else
            {
                canDoubleJump = false;
                myAnimatorStandingPlayer.SetTrigger(IdCanDoubleJump);
            }

            
            myRigibody.velocity = new Vector2(myRigibody.velocity.x, jumpForce);
            
        }
        myAnimatorStandingPlayer.SetBool(IdIsGrounded, isGrounded);
    }

    private void CheckAndSetDirection()
    {
        if (myRigibody.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isFlippedInX = true;
        }
        else if (myRigibody.velocity.x > 0)
        {
            transform.localScale = Vector3.one;
            isFlippedInX = false;
        }
    }

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1") && standingPlayer.activeSelf)
        {
            ArrowController tempArrowController = Instantiate(arrowController, transformArrowPoint.position, transformArrowPoint.rotation);
            if(isFlippedInX)
            {
                tempArrowController.ArrowDirection = new Vector2(-1, 0f);
                tempArrowController.GetComponent<SpriteRenderer>().flipX = true;
            }else
            {
                tempArrowController.ArrowDirection = new Vector2(1, 0f);
            }
            
            myAnimatorStandingPlayer.SetTrigger(IdShootArrow);
        }
        if ((Input.GetButtonDown("Fire1") && ballPlayer.activeSelf) && playerExtrasTracker.CanDropBombs)
        {
            Instantiate(bombController, transformBombPoint.position, Quaternion.identity);
        }
    }

    private void PlayDust()
    {
        if(myRigibody.velocity.x != 0 && isIdle)
        {
            isIdle = false;
            if(isGrounded)
                Instantiate(dustJump, transformDustPoint.position, Quaternion.identity);
        }
        if(myRigibody.velocity.x == 0)
        {
            isIdle = true;
        }
    }

    private void ShowAfterImage()
    {
        SpriteRenderer tempAfterImage = Instantiate(afterImageSR, myTransform.position, myTransform.rotation);
        tempAfterImage.sprite = playerSR.sprite;
        tempAfterImage.transform.localScale = myTransform.localScale;
        tempAfterImage.color = afterImageColor;
        Destroy(tempAfterImage.gameObject, afterImageLifeTime);
        afterImageCounter = afterImageTimeBetween;
    }

    private void BallMode()
    {
        float inputY = Input.GetAxisRaw("Vertical");
        if((inputY <= -.9f && !ballPlayer.activeSelf || inputY >= .9f && ballPlayer.activeSelf) && playerExtrasTracker.CanEnterBallMode)
        {
            ballModeCounter -= Time.deltaTime;
            if(ballModeCounter < 0)
            {
                ballPlayer.SetActive(!ballPlayer.activeSelf);
                standingPlayer.SetActive(!standingPlayer.activeSelf);
            }
        }
        else
        {
            ballModeCounter = waitForBallMode;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkGroundPoint.position, isGroundedRange);
    }

}