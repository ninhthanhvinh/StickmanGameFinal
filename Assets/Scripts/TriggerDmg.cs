using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDmg : MonoBehaviour
{   
    public float weaponDmg;
    private CombatSystem system;

    private void Awake()
    {
        system = GetComponentInParent<CombatSystem>();
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.gameObject.name);
    //    collision.gameObject.TryGetComponent<CombatSystem>(out var combatSystem);
    //    if (combatSystem)
    //    {
    //        combatSystem.GetDamage(weaponDmg);
    //    }
    //}
    public float force;
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent<CombatSystem>(out var combatSystem);
        if (combatSystem && combatSystem != GetComponentInParent<CombatSystem>())
        {
            if(system.isAttacking) combatSystem.GetDamage(weaponDmg);
            system.isAttacking = false;
            //var direction = Vector3.zero - other.transform.position + GetComponentInParent<CombatSystem>().transform.position;
            //direction.y = 0f;
            //other.transform.position -= direction.normalized * force;
        }
    }
}
