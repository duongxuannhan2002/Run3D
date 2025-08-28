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
    int CurrentPos = 0;
    public float SideSpeed;
    public float RunSpeed;
    public float JumForce;
    [SerializeField] Rigidbody rb;
    public bool IsGameStarted = false;
    public bool IsGameOver = false;
    [SerializeField] Animator PlayerAnimator;
    public static bool isGrounded = true;
    public static bool isJump = false;

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
    }

    void Update()
    {
        if (IsGameStarted)
        {
            // chạy thẳng
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + RunSpeed * Time.deltaTime);
            // Mobile
            DetectSwipe();
            // PC
            DetectKeyboard();
            // tăng tốc dần
            if (RunSpeed <= 30f)
            {
                RunSpeed += 0.1f * Time.deltaTime;
            }
        }
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

    private void FixedUpdate()
    {
        // Di chuyển sang lane
        Vector3 targetPos = transform.position;

        if (CurrentPos == 0) targetPos.x = CenterPos.position.x;
        else if (CurrentPos == 1) targetPos.x = LeftPos.position.x;
        else if (CurrentPos == 2) targetPos.x = RightPos.position.x;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, SideSpeed * Time.deltaTime);
        if (!isGrounded && !isJump)
        {
            rb.AddForce(Vector3.down * 50f, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Object")
        {
            AudioManager.Instance.PlaySoundFall();
            IsGameStarted = false;
            IsGameOver = true;
            PlayerAnimator.SetInteger("isDied", 1);
            GameOverPanel.SetActive(true);
        }

        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Car"))
        {
            isGrounded = true;
            isJump = false;
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
}
