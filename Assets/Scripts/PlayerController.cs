using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField] Transform CenterPos;
    [SerializeField] Transform LeftPos;
    [SerializeField] Transform RightPos;
    [SerializeField] GameObject GameOverPanel;
    public CapsuleCollider playerCollider;
    int CurrentPos = 0;
    public float SideSpeed;
    public float RunSpeed;
    public float JumForce;
    public float lastY;
    [SerializeField] Rigidbody rb;
    public bool IsGameStarted = false;
    public bool IsGameOver = false;
    [SerializeField] Animator PlayerAnimator;
    public static bool isGrounded = true;
    public static bool isJump = false;
    public static bool isRoll = false;
    public static bool isFly = false;
    public static bool hasMagnet = false;
    public static bool hasShied = false;

    // --- Swipe detect ---
    private Vector2 startTouchPos, endTouchPos;
    private bool isSwipe;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        CurrentPos = 0;
        IsGameStarted = false;
        IsGameOver = false;
        isGrounded = true;
        hasMagnet = false;
        hasShied = false;
        isFly = false;
        playerCollider = this.GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if (IsGameStarted)
        {
            Vector3 moveDir = HandleSlopeDetection();

            transform.position += moveDir * RunSpeed * Time.deltaTime;

            // Mobile
            DetectSwipe();
            // PC
            DetectKeyboard();
            // tăng tốc dần
            if (RunSpeed <= 40f)
            {
                RunSpeed += 0.1f * Time.deltaTime;
            }
        }
        
    }

    private Vector3 HandleSlopeDetection()
    {
        // hướng mặc định là chạy thẳng
        Vector3 moveDir = Vector3.forward;

        // Bắn tia trước mặt
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 1.2f))
        {
            if (hit.collider.CompareTag("Slope"))
            {
                // Thực hiện hành động khi gặp mặt dốc
                moveDir = Quaternion.Euler(-70, 0, 0) * moveDir;
            }
        }
        return moveDir;
    }

    private void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPos = touch.position;
                isSwipe = true;
            }
            else if (touch.phase == TouchPhase.Ended && isSwipe)
            {
                endTouchPos = touch.position;
                Vector2 swipeDelta = endTouchPos - startTouchPos;

                if (swipeDelta.magnitude > 50f) // ngưỡng để tính là swipe
                {
                    if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                    {
                        // Vuốt ngang
                        if (swipeDelta.x > 0) MoveRight();
                        else MoveLeft();
                    }
                    else
                    {
                        // Vuốt dọc
                        if (swipeDelta.y > 0) Jump();
                        else Roll();
                    }
                }
                isSwipe = false;
            }
        }
    }

    private void DetectKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Roll();
        }
    }

    private void MoveLeft()
    {
        if (CurrentPos == 0) CurrentPos = 1;
        else if (CurrentPos == 2) CurrentPos = 0;
    }

    private void MoveRight()
    {
        if (CurrentPos == 0) CurrentPos = 2;
        else if (CurrentPos == 1) CurrentPos = 0;
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = Vector3.up * JumForce;
            StartCoroutine(JumpAnim());
            isGrounded = false;
            isJump = true;
            AudioManager.Instance.PlaySoundJump();
        }
    }

    IEnumerator JumpAnim()
    {
        PlayerAnimator.SetInteger("isJump", 1);
        yield return new WaitForSeconds(0.1f);
        PlayerAnimator.SetInteger("isJump", 0);
    }

    private void Roll()
    {
        if (isGrounded && !isRoll)
        {
            isRoll = true;
            playerCollider.height /= 3;
            playerCollider.center = new Vector3(0, -0.3f, 0);
            StartCoroutine(RollAnim());
            PlayerAnimator.SetFloat("isRoll", 1);
            PlayerAnimator.SetInteger("isRunning", 0);
            AudioManager.Instance.PlaySoundJump();
        }
    }
    IEnumerator RollAnim()
    {
        yield return new WaitForSeconds(0.6f);
        playerCollider.height *= 3;
        playerCollider.center = new Vector3(0, 0, 0);
        PlayerAnimator.SetFloat("isRoll", 0);
        PlayerAnimator.SetInteger("isRunning", 1);
        isRoll = false;
    }
    

    private void FixedUpdate()
    {
        if (!IsGameOver)
        {

            
            Vector3 targetPos = transform.position;

            if (CurrentPos == 0) targetPos.x = CenterPos.position.x;
            else if (CurrentPos == 1) targetPos.x = LeftPos.position.x;
            else if (CurrentPos == 2) targetPos.x = RightPos.position.x;

            transform.position = Vector3.MoveTowards(transform.position, targetPos, SideSpeed * Time.deltaTime);
        }
        if (!isGrounded && !isJump)
        {
            rb.AddForce(Vector3.down * 50f, ForceMode.Acceleration);
        }
        
        if (isGrounded || isFly)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }

        if(isFly)
        {
            isGrounded = false;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 20, transform.position.z), 40 * Time.deltaTime); 
            float velocityY = (transform.position.y - lastY) / Time.fixedDeltaTime;
            PlayerAnimator.SetFloat("isFly", velocityY);
        }
        lastY = transform.position.y;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Object")
        {
            if (hasShied)
            {
                transform.position += new Vector3(0, 7f, 0);
                isGrounded = false;
                hasShied=false;
                StopCoroutine("ShiedCountdown");
                UIController.instance.StopBlink(1);
            }
            else
            {
                AudioManager.Instance.PlaySoundFall();
                IsGameStarted = false;
                IsGameOver = true;
                PlayerAnimator.SetInteger("isDied", 1);
                GameOverPanel.SetActive(true);
            }  
        }

        if (collision.collider.tag == "Wall")
        {
            if (hasShied)
            {
                transform.position += new Vector3(0, 7, 25);
                isGrounded = false;
                hasShied = false;
                StopCoroutine("ShiedCountdown");
                UIController.instance.StopBlink(1);
            }
            else
            {
                AudioManager.Instance.PlaySoundFall();
                IsGameStarted = false;
                IsGameOver = true;
                PlayerAnimator.SetInteger("isDied", 1);
                GameOverPanel.SetActive(true);
            }
        }

        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Car"))
        {
            isGrounded = true;
            isJump = false;
            Debug.Log("hello");
        }

        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Car"))
        {
            if (isJump)
            {
                isJump = false;
            } 
        }

        if (collision.collider.CompareTag("Magnet"))
        {
            AudioManager.Instance.PlaySoundCollectItem();
            ActivateMagnet();
            Destroy(collision.collider.gameObject);
        }

        if (collision.collider.CompareTag("Shied"))
        {
            AudioManager.Instance.PlaySoundCollectItem();
            ActivateShied();
            Destroy(collision.collider.gameObject);
        }

        if (collision.collider.CompareTag("Fly"))
        {
            AudioManager.Instance.PlaySoundCollectItem();
            isFly = true;
            PlayerAnimator.SetInteger("isRunning", 0);
            StartCoroutine(FlyCountDown());
            Destroy(collision.collider.gameObject);
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Car"))
        {
            isGrounded = false;
        }
    }

    public void StartGame()
    {
        UIController.instance.GameStart();
        Debug.Log("Game is started");
        IsGameStarted = true;
        PlayerAnimator.SetInteger("isRunning", 1);
        PlayerAnimator.speed = 1.2f;
    }

    public void ActivateMagnet(float duration = 7f)
    {
        if (hasMagnet) StopCoroutine("MagnetCountdown");
        hasMagnet = true;
        StartCoroutine("MagnetCountdown", duration);
    }

    private IEnumerator MagnetCountdown(float duration)
    {
        float timer = duration;
        bool isBlinking = false;
        while (timer > 0f)
        {
            if (IsGameStarted && !IsGameOver)
            {
                timer -= Time.deltaTime;

                if (timer <= 3f && !isBlinking)
                {
                    UIController.instance.StartBlink(3f, 0);
                    isBlinking = true;
                }
            }
            yield return null;
        }
        UIController.instance.StopBlink(0);
        hasMagnet = false;
    }

    public void ActivateShied(float duration = 7f)
    {
        if (hasShied) StopCoroutine("ShiedCountdown");
        hasShied = true;
        StartCoroutine("ShiedCountdown", duration);
    }

    private IEnumerator ShiedCountdown(float duration)
    {
        float timer = duration;
        bool isBlinking = false;
        while (timer > 0f)
        {
            if (IsGameStarted && !IsGameOver)
            {
                timer -= Time.deltaTime;
                if (timer <= 3f && !isBlinking)
                {
                    UIController.instance.StartBlink(3f, 1);
                    isBlinking=true;
                }
            }
            yield return null;
        }
        UIController.instance.StopBlink(1);
        hasShied = false;
    }

    private IEnumerator FlyCountDown(float duration = 10f )
    {
        float timer = duration;
        bool isBlinking = false;
        while (timer > 0f)
        {
            if (IsGameStarted && !IsGameOver)
            {
                timer -= Time.deltaTime;
                if (timer <= 3f && !isBlinking)
                {
                    UIController.instance.StartBlink(3f, 2);
                    isBlinking = true;
                }
            }
            yield return null;
        }
        isFly = false;
        rb.velocity = new Vector3(rb.velocity.x, -10, rb.velocity.z);
        this.transform.GetChild(0).gameObject.SetActive(false);
        PlayerAnimator.SetFloat("isFly", -1);
        PlayerAnimator.SetInteger("isRunning", 1);
    }
}
