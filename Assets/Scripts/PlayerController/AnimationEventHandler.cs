using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    
    [Header("Effects and Sound")]
    public ParticleSystem attack1Effect;
    public ParticleSystem attack2Effect;
    public ParticleSystem attack3Effect;
    public ParticleSystem attack4Effect;

    public Animator animator;

    private void Start()
    {
        animator = GetComponent <Animator>();
    }

    public void CanMove()
    {
        animator.SetBool("CanWalk", true);
    }

    public void CannotMove()
    {
        animator.SetBool("CanWalk", false);
    }

    public void Attack1Effect()
    {
        attack1Effect.Play();
    }

    public void Attack2Effect()
    {
        attack2Effect.Play();
    }

    public void Attack3Effect()
    {
        attack3Effect.Play();
    }

    public void Attack4Effect()
    {
        attack4Effect.Play();
    }
}
