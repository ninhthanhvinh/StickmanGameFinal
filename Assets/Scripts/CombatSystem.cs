using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CombatSystem : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public Slider hpBar;
    public BoxCollider boxCollider;
    private Rigidbody rb;
    private Animator anim;
    private ParticleSystem[] weaponVFX;
    private RagdollControl control;
    private GameManager gameManager;
    public bool canMove = true;
    public bool isAttacking = false;
    bool canAttack;
    AudioManager audioManager;
    Button attackButton;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        rb = GetComponent<Rigidbody>();
        audioManager = AudioManager.Instance;
        weaponVFX = GetComponentsInChildren<ParticleSystem>();
        anim = GetComponent<Animator>();
        control = GetComponent<RagdollControl>();
        health = maxHealth;
        canAttack = true;
        if (gameObject.CompareTag("Player"))
        {
            attackButton = GameObject.Find("AttackButton").GetComponent<Button>();
            attackButton.onClick.AddListener(Attack);
        }

    }

    // Update is called once per frame
    public void Attack()
    {
        if (canAttack)
        {

            boxCollider.isTrigger = true;
            anim.SetTrigger("Attack");
            canMove = false;
            audioManager.PlaySFX("SwordSFX");
            foreach (var vfx in weaponVFX)
            {
                vfx.Play();
            }
            canAttack = false;
            Invoke(nameof(RenewAttack), .5f);
        }

    }

    public void SetAttackState()
    {
        isAttacking = true;
    }

    private void Update()
    {
        hpBar.value = health / maxHealth;
    }
    public void GetDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            hpBar.gameObject.SetActive(false);
            rb.useGravity = true;
            if(TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
                agent.velocity = Vector3.zero;
            health = 0;
            control.EnableRagdoll();
            if (TryGetComponent<Movement>(out Movement movement))
            {
                Destroy(movement);
            }
            Invoke(nameof(Destroy), 2f);
        }
        
    }

    public void Destroy()
    {
        if (gameObject.CompareTag("Player"))
            {
                gameManager.Lose();
            }
        Destroy(this.gameObject);
    }

    public void RenewAttack()
    {
        boxCollider.isTrigger = false;
        canAttack = true;
        canMove = true;
    }
}
