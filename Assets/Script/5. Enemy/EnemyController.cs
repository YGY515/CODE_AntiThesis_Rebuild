using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public Animator anim;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public AudioSource audioSource;
    public AudioClip hitClip;
    public PlayerHealth playerHealth;
    public DialogueManager dialogueManager;

    private float moveSpeed = 3.0f;
    private Vector2 currentDirection;

    public bool canBeDamaged = false;
    public int hp = 3;
    public Color hitColor = Color.red;
    public float hitEffectDuration = 0.2f;

    public float attackCooldown = 2.0f;
    public float attackDistanceX = 0.05f;
    public float attackDistanceY = 0.05f;

    public float dashDistance = 0.2f;
    public int damageAmount = 1;
    private bool canAttack = true;


    void OnEnable()
    {
        hp = 3;
        gameObject.SetActive(true);
        canBeDamaged = false; 

        playerHealth = FindObjectOfType<PlayerHealth>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        NpcController npc = FindObjectOfType<NpcController>();

        canBeDamaged = EnemyPoolManager.Instance != null && EnemyPoolManager.Instance.EnemyCanBeDamaged;


        if (npc != null)
        {
            var enemyList = new List<EnemyController>(npc.enemies); 
            if (!enemyList.Contains(this))
            {
                enemyList.Add(this);
                npc.enemies = enemyList.ToArray();
            }
        }

        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
        anim.SetFloat("PositionX", 0.0f);
        anim.SetFloat("PositionY", -1.0f);
    }

    void Update()
    {
        if (dialogueManager.isTyping == false)
        {
            AttackPlayer();
            ChasePlayer();
        }

    }
    void FixedUpdate()
    {
        if (canAttack && dialogueManager.isTyping == false)
            rb.MovePosition(rb.position + currentDirection * moveSpeed * Time.fixedDeltaTime);
        
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

            if (!didDamage && Vector2.Distance(newPos, player.position) <= 0.1f)
            {
                playerHealth.PlayerTakeDamage(damageAmount);
                didDamage = true;
            }

            yield return null;
        }

        rb.MovePosition(endPos);
    }

    void ChasePlayer()
    {
        if (player == null) return;

        Vector2 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;

        if (distance < 0.1f) return;

        Vector2 direction = toPlayer.normalized;
        currentDirection = direction;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 애니메이션 방향 설정
        if (angle >= -45f && angle < 45f)
        {
            anim.SetFloat("PositionX", 1.0f);
            anim.SetFloat("PositionY", 0.0f);
        }
        else if (angle >= 45f && angle < 135f)
        {
            anim.SetFloat("PositionX", 0.0f);
            anim.SetFloat("PositionY", 1.0f);
        }
        else if (angle >= -135f && angle < -45f)
        {
            anim.SetFloat("PositionX", 0.0f);
            anim.SetFloat("PositionY", -1.0f);
        }
        else
        {
            anim.SetFloat("PositionX", -1.0f);
            anim.SetFloat("PositionY", 0.0f);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!canBeDamaged) return;

        hp -= damage;

        if (audioSource && hitClip)
            audioSource.PlayOneShot(hitClip);

        if (spriteRenderer)
            StartCoroutine(HitEffect());

        if (hp <= 0)
        {
            StartCoroutine(FadeOutAndDisable());
        }
        else
        {
            StartCoroutine(DamageCooldown());
        }

        canBeDamaged = false; 
    }

    IEnumerator FadeOutAndDisable()
    {
        float duration = 0.5f;
        float elapsed = 0f;

        Color color = spriteRenderer.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        gameObject.SetActive(false);
        spriteRenderer.color = new Color(255, 255, 255, 1f);    // 피격 당한 색 초기화


    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(1.0f); 
        canBeDamaged = true;
    }

    IEnumerator HitEffect()
    {
        Color origin = spriteRenderer.color;
        spriteRenderer.color = hitColor;
        float t = 0;
        while (t < hitEffectDuration)
        {
            spriteRenderer.color = Color.Lerp(hitColor, origin, t / hitEffectDuration);
            t += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = origin;
    }

}
