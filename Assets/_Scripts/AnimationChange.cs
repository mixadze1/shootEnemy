using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChange : MonoBehaviour
{
    public string CurrentState;
    public Animator Animator;

    public void ChangeAnimationState(string newState)
    {
       /* if (CurrentState == newState)
            return;*/
        CurrentState = newState;
        Animator.Play(newState);
    }
}