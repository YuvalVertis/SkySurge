using UnityEngine;

public static class CodesManager
{
    public static readonly int Run = Animator.StringToHash("Run");
    public static readonly int Idle = Animator.StringToHash("Idle");
    public static readonly int Jump = Animator.StringToHash("Jump");
    public static readonly int Blink = Animator.StringToHash("Blink");
    public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    public static readonly int Speed = Animator.StringToHash("Speed");
    public static readonly int jumpVel = Animator.StringToHash("JumpVel");
}