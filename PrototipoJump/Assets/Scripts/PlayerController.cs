using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player 'base'")]
    private Rigidbody playerRb;
    [SerializeField] float jumpForce = 10;
    [SerializeField] float gravityModifier;
    private bool isOnGround;
    public static bool gameOver;
     private InputAction jumpAction;
     [SerializeField] InputActionAsset inputActions;

    [Header("Animacao")]
    private Animator playerAnim;
    
    [Header("Particula")]
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    [Header("Vida")]
    private int currentLifes; 
    [SerializeField] int maxLifes;
    [SerializeField] HudManager hudManager;

    void Awake()
    {
        jumpAction = inputActions.FindAction("Jump");
    }
    private void OnEnable() 
    {
        jumpAction.Enable();
    }
    private void OnDisable()
    {
        jumpAction.Disable(); 

    }

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -9.81f * gravityModifier, 0);
        playerAnim = GetComponent<Animator>();
        currentLifes = maxLifes;
        gameOver = false;

        if (hudManager != null)
        {
            hudManager.updateLifes(currentLifes);
        }
    }

    void Update()
    {
        if (jumpAction.WasPressedThisFrame() && isOnGround && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !gameOver)
        {
            isOnGround = true; 
            dirtParticle.Play();
        } 
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            currentLifes--; 
            if (hudManager != null)
            {
                hudManager.updateLifes(currentLifes);
            }

            if(currentLifes <= 0) 
            { 
                processGameOver();
            }
        }
    }

    private void processGameOver() 
    { 
        Debug.Log("Game Over"); 
        gameOver = true; 
        playerAnim.SetInteger("DeathType_int", 1); 
        playerAnim.SetBool("Death_b", true); 
        dirtParticle.Stop(); 
        explosionParticle.Play();
    }

    public static bool IsGameOver()
    { 
        return gameOver;
    }
}
