using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger2 : MonoBehaviour
{
    public float tmp = 10;
    public Animator animator;
    public Rigidbody carRigidbody;
    private void OnTriggerEnter(Collider other)
    {
        animator.Play("FanSpin");
        carRigidbody.velocity = carRigidbody.transform.forward * tmp;
    }
}
