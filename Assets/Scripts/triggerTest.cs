using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerTest : MonoBehaviour
{
    public Animator animator;
    private void OnTriggerEnter(Collider other)
    {
        animator.Play("TriggerOnFire");
    }
}
