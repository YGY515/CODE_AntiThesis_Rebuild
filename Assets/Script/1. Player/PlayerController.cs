using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public Animator anim;
    public Rigidbody2D rb;

    public WeaponManager weaponManager;
    [SerializeField] private PlayerHealth playerHealth;
    

    [SerializeField] private GameObject Baton_Icon;
    [SerializeField] private GameObject Baton_Empty;

    [SerializeField] private GameObject Dash_Icon;
    [SerializeField] private GameObject Dash_Empty;

    private bool canAttack = true;
    private bool canDash = true;
    private float Cooldown = 0.3f;

    private float dashDuration = 0.5f;
    private float dashSpeed = 3f;
    private bool isDashing = false;


    void Start()
    {
        //weaponManager = new WeaponManager();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        HandleMovementInput();
        WeaponInput();
        DashInput();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerHealth.TakeDamage(1);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            playerHealth.Heal(1);
        }
    }

    void HandleMovementInput()
    {
        // Manage the Player Movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // 블렌드 트리로 만든 애니메이션
        anim.SetFloat("PositionX", horizontal);
        anim.SetFloat("PositionY", vertical);

        Vector2 movement = new Vector2(horizontal, vertical).normalized * moveSpeed;
        rb.velocity = movement;


        if (movement != Vector2.zero)
        {
            anim.SetBool("Running", true);
            if (!isDashing) // 대쉬 중이 아닐 때만 속도 변경
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    anim.SetBool("Flying", true);
                    moveSpeed = 8.5f;
                }
                else
                {
                    anim.SetBool("Flying", false);
                    moveSpeed = 6f;
                }
            }
 

            if (horizontal > 0)     // 오른쪽
            {
                anim.SetFloat("Looking", 0.66f);

            }
            else if (horizontal < 0) // 왼쪽
            {

                anim.SetFloat("Looking", 0.33f);

            }
            else if (vertical > 0)  // 위쪽
            {
                anim.SetFloat("Looking", 1.00f);

            }
            else if (vertical < 0) // 아래쪽
            {
                anim.SetFloat("Looking", 0.00f);

            }

        }
        else // 키 입력이 없는 경우
        {
            anim.SetBool("Flying", false);
            anim.SetBool("Running", false);

        }

    }

    void WeaponInput()
    {
        if (Input.GetKeyDown(KeyCode.Z) && canAttack)
        {
            weaponManager.UseWeapon();
            StartCoroutine(WeaponCooldown());
        }

        if (Input.GetKeyDown(KeyCode.Tab))
            weaponManager.ChangeWeapon();
    }

    private IEnumerator WeaponCooldown()
    {
        canAttack = false;
        if (Baton_Icon != null) Baton_Icon.SetActive(false);
        if (Baton_Empty != null) Baton_Empty.SetActive(true);

        yield return new WaitForSeconds(Cooldown);

        if (Baton_Icon != null) Baton_Icon.SetActive(true);
        if (Baton_Empty != null) Baton_Empty.SetActive(false);
        canAttack = true;
    }

    void DashInput()
    {
        if (Input.GetKeyDown(KeyCode.X) && canDash)
        {
            StartCoroutine(DashCooldown());
        }
    }

    private IEnumerator DashCooldown()
    {
        canDash = false;
        isDashing = true;
        if (Dash_Icon != null) Dash_Icon.SetActive(false);
        if (Dash_Empty != null) Dash_Empty.SetActive(true);

        float prevSpeed = moveSpeed;
        moveSpeed += dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        moveSpeed = prevSpeed;
        isDashing = false;

        yield return new WaitForSeconds(Cooldown);

        if (Dash_Icon != null) Dash_Icon.SetActive(true);
        if (Dash_Empty != null) Dash_Empty.SetActive(false);
        canDash = true;
    }

}
