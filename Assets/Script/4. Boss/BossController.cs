using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{

    public Animator anim;
    public GameObject shieldObject;
    public Transform player;
    public Rigidbody2D rb;
    public PlayerHealth playerHealth;
    public BossHealth bossHealth;


    private Vector2 currentDirection;
    private Vector2 randomDirection;
    private float randomMoveInterval = 0.5f;
    private float restInterval = 1f;
    private bool isMoving = true;
    private float moveTimer = 0f;

    private float moveSpeed = 5.0f;
    private float dashSpeed = 7.5f;
    private float dashDuration = 0.5f;
    private float dashCooldown = 1.0f;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;

    public float attackCooldown = 2.0f;
    public float attackDistanceX = 0.05f;
    public float attackDistanceY = 0.05f;
    public float dashDistance = 0.5f;
    public int damageAmount = 1;
    private bool canAttack = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        PickRandomDirection();
    }

    void Update()
    {
        
        if (bossHealth.CurrentHealth <= 0)
        {
            OnBossDied();
        }

        if (shieldObject.activeSelf)
        {
            RandomMoveWithPause();
        }
        else
        {
            AttackPlayer();
            HandleChaseWithDash();
        }
    }

    void FixedUpdate()
    {
        if (!shieldObject.activeSelf)
        {
            if (canAttack)
                rb.MovePosition(rb.position + currentDirection * moveSpeed * Time.fixedDeltaTime);
            
        }
    }

    void AttackPlayer()
    {
        if (!canAttack || player == null) return;

        Vector2 diff = player.position - transform.position;

        if (Mathf.Abs(diff.x) <= attackDistanceX && Mathf.Abs(diff.y) <= attackDistanceY)
        {
            StartCoroutine(AttackCoroutine(diff));
        }
    }

    IEnumerator AttackCoroutine(Vector2 diff)
    {
        canAttack = false;

        Vector2 dashDir = diff.normalized;

        yield return StartCoroutine(DashRoutine(dashDir)); 
        yield return new WaitForSeconds(attackCooldown); 
        canAttack = true;
    }

    IEnumerator DashRoutine(Vector2 dashDir)
    {
        float dashTime = 0.2f;
        float elapsed = 0f;
        Vector2 startPos = rb.position;
        Vector2 endPos = startPos + dashDir * dashDistance;

        bool didDamage = false;

        while (elapsed < dashTime)
        {
            elapsed += Time.deltaTime;
            Vector2 newPos = Vector2.Lerp(startPos, endPos, elapsed / dashTime);
            rb.MovePosition(newPos);

            if (!didDamage && Vector2.Distance(newPos, player.position) <= 0.5f)
            {
                playerHealth.PlayerTakeDamage(damageAmount);
                didDamage = true;            
            }

            yield return null;
        }

        rb.MovePosition(endPos);
    }


    void HandleChaseWithDash()
    {
        if (!isDashing && cooldownTimer <= 0f)
        {
            isDashing = true;
            dashTimer = dashDuration;
        }

        if (isDashing)
        {
            moveSpeed = dashSpeed;
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
                cooldownTimer = dashCooldown;
                moveSpeed = 5.0f;
            }
        }
        else
        {
            moveSpeed = 5.0f;
            if (cooldownTimer > 0f)
                cooldownTimer -= Time.deltaTime;
        }

        ChasePlayer();
    }
    void RandomMoveWithPause()
    {
        moveTimer += Time.deltaTime;
        if (isMoving)
        {
            MoveAndAnimate(randomDirection);
            if (moveTimer >= randomMoveInterval)
            {
                moveTimer = 0f;
                isMoving = false;
            }
        }
        else
        {
            // 쉬는 중(멈춤)
            if (moveTimer >= restInterval)
            {
                moveTimer = 0f;
                isMoving = true;
                PickRandomDirection();
            }
        }
    }

    void PickRandomDirection()
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        randomDirection = directions[Random.Range(0, directions.Length)];
    }

    void ChasePlayer()
    {
        if (player == null) return;
        Vector2 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;

        if (distance < 0.1f)
        {
            // 플레이어와 너무 가까우면 멈추기
            anim.SetFloat("Looking", anim.GetFloat("Looking"));
            currentDirection = Vector2.zero; 
            rb.velocity = Vector2.zero;      
            return;
        }

        Vector2 direction = toPlayer.normalized;
        currentDirection = direction;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float looking = 0.00f;  // 시작 시 아래 방향

        if (angle >= -45f && angle < 45f)
            looking = 0.66f;
        else if (angle >= 45f && angle < 135f)
            looking = 1.00f;
        else if (angle >= -135f && angle < -45f)
            looking = 0.00f;
        else
            looking = 0.33f;

        anim.SetFloat("Looking", looking);
    }



    void MoveAndAnimate(Vector2 dir)
    {
        if (dir == Vector2.right)
            anim.SetFloat("Looking", 0.66f);
        else if (dir == Vector2.left)
            anim.SetFloat("Looking", 0.33f);
        else if (dir == Vector2.up)
            anim.SetFloat("Looking", 1.00f);
        else if (dir == Vector2.down)
            anim.SetFloat("Looking", 0.00f);

        transform.position += (Vector3)(dir.normalized * moveSpeed * Time.deltaTime);
    }

    void OnBossDied()
    {
        StartCoroutine(DieWithArcEffect());
    }

    IEnumerator DieWithArcEffect()
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        anim.enabled = false;

        float duration = 1.0f; 
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(4f, 0f, 0f); 
        float height = 2.5f;

        float startAngle = transform.eulerAngles.z;
        float endAngle = startAngle - 90f;


        SpriteRenderer sr = bossHealth.Boss_spriteRenderer;
        Color startColor = sr.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            Vector3 parabola = Vector3.Lerp(startPos, endPos, t);
            parabola.y += height * 4 * t * (1 - t);
            transform.position = parabola;

            float angle = Mathf.Lerp(startAngle, endAngle, t);
            transform.rotation = Quaternion.Euler(0, 0, angle);

            if (sr != null)
            {
                Color c = startColor;
                c.a = Mathf.Lerp(1f, 0f, t);
                sr.color = c;
            }

            yield return null;
        }

        if (sr != null)
        {
            Color c = startColor;
            c.a = 0f;
            sr.color = c;
        }
        transform.rotation = Quaternion.Euler(0, 0, endAngle);
        transform.position = endPos;

        Destroy(gameObject);
    }

}

