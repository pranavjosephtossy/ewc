using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCtrl : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float verticalSpeed;
    public float turnSpeed;
    public float jumpForce;
    private float groundlimit = -5;
    private Animator playerAnimation;

    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint;

    private Rigidbody playerRB;
    private bool isDead = false;

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<Animator>();
    }

    void Update()
    {

        if (isDead)
        {
            return;
        }

        // get horizontal and vertical inputs
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // forward, backward movement
        if (verticalInput > 0)
        {
            // forward
            playerAnimation.SetBool("frwd", true);
            playerAnimation.SetBool("back", false);
        }
        else if (verticalInput < 0)
        {
            // backward
            playerAnimation.SetBool("back", true);
            playerAnimation.SetBool("frwd", false);
        }
        else
        {
            playerAnimation.SetBool("frwd", false);
            playerAnimation.SetBool("back", false);
        }

            // left and right movement
            if (horizontalInput > 0)
            {
                // right
                playerAnimation.SetBool("right", true);
                playerAnimation.SetBool("left", false);  
            }
            else if (horizontalInput < 0)
            {
                // left
                playerAnimation.SetBool("left", true);
                playerAnimation.SetBool("right", false);  
            }
            else
            {
                playerAnimation.SetBool("right", false);
                playerAnimation.SetBool("left", false);
            }
            //this moves the players
            transform.Translate(Vector3.forward * Time.deltaTime * verticalInput * verticalSpeed);
            transform.Translate(Vector3.right * Time.deltaTime * horizontalInput * turnSpeed);
            // jump only if ur not over ground limit, some lineancy when it comes to the exact y position
            if (Input.GetKeyDown(KeyCode.Space) && transform.position.y !< 0.5)
            {
                playerAnimation.SetBool("jump", true);
                playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else
            {
                playerAnimation.SetBool("jump", false);
            }
            //reset animation
            if (playerAnimation.GetBool("attack") && !Input.GetButtonDown("Fire1"))
            {
                StartCoroutine(ResetAttackAnimation());
            }

            //if player falls of edge(doesnt work in alpha)
            if (transform.position.y <= groundlimit && !isDead)
            {
                playerAnimation.SetBool("death", true);
                Die();
                Debug.Log("Game Over");
            }
        }

        IEnumerator ResetAttackAnimation()
        {
            //wait, reset animation
            yield return new WaitForSeconds(0.5f);
            playerAnimation.SetBool("attack", false);  
            //playerAnimation.SetBool("summon", false);  // reset summon animation
        }
        //detroy ghost on collision
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy") && !isDead)
            {
                Die();
            }
        }

        void SummonFireball()
        {
            if (fireballPrefab != null && fireballSpawnPoint != null)
            {
                GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, fireballSpawnPoint.rotation);
                Rigidbody fireballRB = fireball.GetComponent<Rigidbody>();

                if (fireballRB != null)
                {
                    fireballRB.useGravity = false;
                    playerAnimation.SetBool("attack", true);
                    fireballRB.AddForce(fireballSpawnPoint.forward * 70f, ForceMode.Impulse);
                }

                else
                {
                    playerAnimation.SetBool("attack", true);
                }
            }
        }

        // set animations to false
        void Die()
        {
            isDead = true;
            playerAnimation.SetBool("death", true);
            playerRB.velocity = Vector3.zero;
            playerAnimation.SetBool("frwd", false);
            playerAnimation.SetBool("back", false);
            playerAnimation.SetBool("right", false);
            playerAnimation.SetBool("left", false);
            Debug.Log("Player has died!");
        }
    }



