using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HexPlayerMovement : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float stealthWalkSpeed = 1.5f;

    public float doubleClickTime = 0.3f;
    public float animSmooth = 10f;

    public Animator animator;
    public HexGrid grid;
    public IdleRandomizer idleRandomizer;

    Vector2Int currentHex;
    Queue<Vector3> worldPath = new Queue<Vector3>();
    Vector3 currentTarget;

    bool isMoving;
    bool isRunning;
    bool isStealth;

    float lastClickTime;
    float animSpeed;

    void Start()
    {
        currentHex = grid.WorldToHex(transform.position);

        Vector3 snap = grid.HexToWorld(currentHex.x, currentHex.y);
        snap.y = transform.position.y;
        transform.position = snap;

        animator.SetFloat("speed", 0f);
        animator.SetBool("isStealth", false);
    }

    void Update()
    {
        HandleStealthInput();

        UpdateMovement();
        UpdateAnimation();
    }

    // ---------------- INPUT ----------------

    void HandleStealthInput()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            isStealth = !isStealth;
            animator.SetBool("isStealth", isStealth);

            // в скрытности бег запрещён
            if (isStealth)
                isRunning = false;
        }
    }

    void DetectRunInput()
    {
        if (isStealth)
        {
            isRunning = false;
            lastClickTime = Time.time;
            return;
        }

        isRunning = Time.time - lastClickTime <= doubleClickTime;
        lastClickTime = Time.time;
    }

    // ---------------- MOVEMENT ----------------

    void UpdateMovement()
    {
        if (!isMoving)
            return;

        float moveSpeed;

        if (isStealth)
            moveSpeed = stealthWalkSpeed;
        else
            moveSpeed = isRunning ? runSpeed : walkSpeed;

        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, currentTarget) < 0.05f)
        {
            transform.position = currentTarget;

            if (worldPath.Count > 0)
            {
                currentTarget = worldPath.Dequeue();
                RotateTo(currentTarget);
            }
            else
            {
                StopMoving();
            }
        }
    }

    // ---------------- ANIMATION ----------------

    void UpdateAnimation()
    {
        float targetAnimSpeed = 0f;

        if (isMoving)
        {
            if (isStealth)
                targetAnimSpeed = 0.5f;
            else
                targetAnimSpeed = isRunning ? 1f : 0.5f;
        }

        animSpeed = Mathf.Lerp(animSpeed, targetAnimSpeed, Time.deltaTime * animSmooth);
        animator.SetFloat("speed", animSpeed);
    }

    // ---------------- PATH ----------------

    public void MoveToHex(int q, int r)
    {
        DetectRunInput();

        Vector2Int startHex = grid.WorldToHex(transform.position);
        Vector2Int targetHex = new Vector2Int(q, r);

        List<Vector2Int> hexPath = grid.FindPath(startHex, targetHex);
        if (hexPath == null || hexPath.Count == 0)
            return;

        worldPath.Clear();

        foreach (var hex in hexPath)
        {
            Vector3 pos = grid.HexToWorld(hex.x, hex.y);
            pos.y = transform.position.y;
            worldPath.Enqueue(pos);
        }

        currentTarget = worldPath.Dequeue();
        RotateTo(currentTarget);

        isMoving = true;

        if (idleRandomizer != null)
            idleRandomizer.SetMoving(true);
    }

    void StopMoving()
    {
        isMoving = false;
        isRunning = false;

        currentHex = grid.WorldToHex(transform.position);

        if (idleRandomizer != null)
            idleRandomizer.SetMoving(false);
    }

    void RotateTo(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        if (dir != Vector3.zero)
            transform.forward = dir;
    }
}
