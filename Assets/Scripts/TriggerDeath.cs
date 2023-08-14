using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CombatSystem>())
            other.GetComponent<CombatSystem>().GetDamage(10000);
    }
}
