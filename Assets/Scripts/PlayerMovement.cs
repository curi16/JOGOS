using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // --- VARIÁVEIS PÚBLICAS ORIGINAIS ---
    public float moveSpeed = 5f;     
    public float jumpForce = 10f;    
    public Transform groundCheck;    
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    // --- VARIÁVEIS DE CONTROLE ESPECÍFICAS PARA CADA JOGADOR ---
    // Tornada pública para aparecer no Inspector e permitir a configuração de Player 1 ou Player 2.
    public bool isPlayerOne = true; 
    
    // Teclas do Jogador 1 (Padrão: Setas e RightShift)
    private KeyCode p1_RightKey = KeyCode.RightArrow;
    private KeyCode p1_LeftKey = KeyCode.LeftArrow;
    private KeyCode p1_JumpKey = KeyCode.UpArrow;
    private KeyCode p1_FightKey = KeyCode.RightShift;
    private KeyCode p1_PunchKey = KeyCode.Period; // '.'

    // Teclas do Jogador 2 (Padrão: WASD e LeftShift)
    private KeyCode p2_RightKey = KeyCode.D;
    private KeyCode p2_LeftKey = KeyCode.A;
    private KeyCode p2_JumpKey = KeyCode.W;
    private KeyCode p2_FightKey = KeyCode.LeftShift;
    private KeyCode p2_PunchKey = KeyCode.Z;

    // --- VARIÁVEIS PRIVADAS ORIGINAIS ---
    private Rigidbody2D rb;
    private bool isGrounded;

    public Transform visual;
    private Animator anim;

    // --- VARIÁVEIS PARA O JOGADOR ATUAL ---
    private KeyCode currentRightKey;
    private KeyCode currentLeftKey;
    private KeyCode currentJumpKey;
    private KeyCode currentFightKey;
    private KeyCode currentPunchKey;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = visual.GetComponent<Animator>();

        // Define quais teclas este script deve usar baseado na configuração 'isPlayerOne'
        if (isPlayerOne)
        {
            currentRightKey = p1_RightKey;
            currentLeftKey = p1_LeftKey;
            currentJumpKey = p1_JumpKey;
            currentFightKey = p1_FightKey;
            currentPunchKey = p1_PunchKey;
        }
        else
        {
            currentRightKey = p2_RightKey;
            currentLeftKey = p2_LeftKey;
            currentJumpKey = p2_JumpKey;
            currentFightKey = p2_FightKey;
            currentPunchKey = p2_PunchKey;
        }
    }

    void Update()
    {
        // Check if touching the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        anim.SetBool("isJump", !isGrounded);

        // --- MOVIMENTO HORIZONTAL (AGORA ESPECÍFICO) ---
        float moveInput = 0f;
        if (Input.GetKey(currentRightKey))
        {
            moveInput = 1f;
        }
        else if (Input.GetKey(currentLeftKey))
        {
            moveInput = -1f;
        }
        
        // Usando rb.velocity (forma mais comum e simples em 2D)
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        //Walking Animation
        anim.SetBool("isRunning", Mathf.Abs(moveInput) > 0f && isGrounded);
        if (moveInput > 0.01f)
        {
            visual.localScale = new Vector3(4, 4, 4);
        }
        else if(moveInput < -0.01f)
        {
            visual.localScale = new Vector3(-4, 4, 4);
        }

        // Jump
        if (Input.GetKeyDown(currentJumpKey) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // --- ATAQUE E POSE DE LUTA (AGORA ESPECÍFICO) ---
        bool isFighting = Input.GetKey(currentFightKey) && isGrounded;
        anim.SetBool("isFightPose", isFighting);

        if (isFighting)
        {
            rb.linearVelocity = Vector3.zero; // Para parar ao entrar em pose de luta
            if (Input.GetKeyDown(currentPunchKey))
            {
                anim.SetTrigger("Punch");
            }
        }
    }
}