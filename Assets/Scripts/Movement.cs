using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Range(0.0f, 0.3f)]
    public float MovementSmoothTime = 0.12f;

    public float speed;
    public float dashSpeed;
    public float dashTime;

    public ParticleSystem dashFX;

    public RagdollControl ragdollControl;

    bool isDashing;
    bool canDash = true;
    Vector3 input;
    Vector3 currentInput;
    Vector3 smoothInputVelocity;
    Animator anim;
    Camera _mainCamera;
    Rigidbody rb;
    CombatSystem cb;
    private float _rotationVelocity;
    Button dashButton;
    // Start is called before the first frame update
    void Start()
    {   
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        cb = GetComponent<CombatSystem>();
        _mainCamera = Camera.main;
        dashButton = GameObject.Find("DashButton").GetComponent<Button>();
        dashButton.onClick.AddListener(TriggerDash);
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector3(UltimateJoystick.GetHorizontalAxis("Movement"), 0f, UltimateJoystick.GetVerticalAxis("Movement"));
        currentInput = Vector3.SmoothDamp(currentInput, input, ref smoothInputVelocity, MovementSmoothTime);
        anim.SetFloat("Speed", Vector2.SqrMagnitude(input));

        var _targetRotation = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
        if (input.magnitude != 0)
        {
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0f, _mainCamera.transform.eulerAngles.y, 0f) * currentInput;

        if (!isDashing && cb.canMove)
        {
            transform.position = transform.position + targetDirection * speed * Time.deltaTime;
        }



        if (Input.GetKeyDown(KeyCode.K))
        {
            TriggerDash();
        }
    }

    public void TriggerDash()
    {
        if (canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        rb.isKinematic = false;
        Rigidbody[] childrb = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in childrb)
        {
            rb.isKinematic = false;
        }
        Vector3 targetDirection = Quaternion.Euler(0f, _mainCamera.transform.eulerAngles.y, 0f) * currentInput;
        rb.AddForce(targetDirection.normalized * dashSpeed, ForceMode.VelocityChange);
        yield return new WaitForSeconds(dashTime);
        rb.velocity = Vector3.zero;
        foreach (var _rb in childrb)
        {
            _rb.isKinematic = true;
        }
        rb.isKinematic = false;
        isDashing = false;
        Invoke(nameof(cooldownCanDash), 1f);
        //var startTime = Time.time;
        //if (Time.time < startTime + dashTime)
        //{
        //    Vector3 targetDirection = Quaternion.Euler(0f, _mainCamera.transform.eulerAngles.y, 0f) * currentInput;
        //    rb.AddForce(targetDirection.normalized * dashSpeed, ForceMode.VelocityChange);
        //    yield return null;
        //}
    }

    public void cooldownCanDash()
    {
        canDash = true;
    }
}
