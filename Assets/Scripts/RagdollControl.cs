using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollControl : MonoBehaviour
{
    [SerializeField]
    private Animator Animator;
    [SerializeField]
    private AIAgent AI;
    [SerializeField]
    private NavMeshAgent Agent;
    Rigidbody[] Rigidbodies;
    CharacterJoint[] Joints;
    CapsuleCollider _collider;
    Rigidbody rb;
    public bool triggerRagdoll;
    Animator anim;
    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        Rigidbodies = GetComponentsInChildren<Rigidbody>();
        Joints = GetComponentsInChildren<CharacterJoint>();
        _collider = GetComponent<CapsuleCollider>();
        DisableRagdoll();

        if (gameObject.CompareTag("Player"))
        {
            rb.isKinematic = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            EnableRagdoll();
        }
    }

    public void DisableRagdoll()
    {
        anim.enabled = true;
        if (Agent)
        {
            Agent.enabled = true;
            AI.enabled = true;
        }
        foreach (CharacterJoint joint in Joints)
        {
            joint.enableCollision = false;
        }
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }
        rb.useGravity = true;
    }

    public void EnableRagdoll()
    {
        anim.enabled = false;
        if (Agent)
        {
            Agent.enabled = false;
            AI.enabled = false;
        }
        //_collider.enabled = false;
        foreach(CharacterJoint joint in Joints)
        {
            joint.enableCollision = true;
        }
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            if (rigidbody != null)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.detectCollisions = true;
                rigidbody.useGravity = true;
                rigidbody.isKinematic = false;
            }
        }
    }
}
