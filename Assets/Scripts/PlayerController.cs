using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        CurrentPos = 0; // 0 Center, 1 Left, 2 Right
        IsGameStarted = false;
        IsGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsGameStarted && !IsGameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Game is started");
                IsGameStarted = true;
                PlayerAnimator.SetInteger("isRunning", 1);
                PlayerAnimator.speed = 1.2f;
            }
        }
        if (IsGameStarted)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + RunSpeed * Time.deltaTime);
            if (CurrentPos == 0)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    CurrentPos = 1;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    CurrentPos = 2;
                }
            }
            else if (CurrentPos == 1)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    CurrentPos = 0;
                }
            }
            else if (CurrentPos == 2)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    CurrentPos = 0;
                }
            }

            if (CurrentPos == 0)
            {
                if (Vector3.Distance(transform.position, new Vector3(CenterPos.position.x, transform.position.y, transform.position.z)) >= 0.1f)
                {
                    Vector3 dir = new Vector3(CenterPos.position.x, transform.position.y, transform.position.z) - transform.position;
                    transform.Translate(dir.normalized * SideSpeed * Time.deltaTime, Space.World);
                }

            }
            else if (CurrentPos == 1)
            {
                if (Vector3.Distance(transform.position, new Vector3(LeftPos.position.x, transform.position.y, transform.position.z)) >= 0.1f)
                {
                    Vector3 dir = new Vector3(LeftPos.position.x, transform.position.y, transform.position.z) - transform.position;
                    transform.Translate(dir.normalized * SideSpeed * Time.deltaTime, Space.World);
                }

            }
            else if (CurrentPos == 2)
            {
                if (Vector3.Distance(transform.position, new Vector3(RightPos.position.x, transform.position.y, transform.position.z)) >= 0.1f)
                {
                    Vector3 dir = new Vector3(RightPos.position.x, transform.position.y, transform.position.z) - transform.position;
                    transform.Translate(dir.normalized * SideSpeed * Time.deltaTime, Space.World);
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                //rb.AddForce(Vector3.up * JumForce);
                rb.velocity = Vector3.up * JumForce;
                StartCoroutine(Jump());
            }
        }
    }
    IEnumerator Jump()
    {
        PlayerAnimator.SetInteger("isJump", 1);
        yield return new WaitForSeconds(0.1f);
        PlayerAnimator.SetInteger("isJump", 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Object")
        {
            IsGameStarted = false;
            IsGameOver = true;
            //PlayerAnimator.applyRootMotion = true;
            PlayerAnimator.SetInteger("isDied", 1);
            GameOverPanel.SetActive(true);
        }
    }
}
