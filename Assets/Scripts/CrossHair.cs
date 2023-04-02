using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    // ũ�ν���� ���¿� ���� ���� ��Ȯ��
    private float gunAccuracy;

    // ũ�ν���� ��Ȱ��ȭ�� ���� �θ� ��ü
    [SerializeField]
    private GameObject go_CrossHairHUD;

    public void WalkingAnimation(bool _flag)
    {
        animator.SetBool("Walking", _flag);
    }
    public void RunningAnimation(bool _flag)
    {
        animator.SetBool("Running", _flag);
    }
    public void CrouchingAnimation(bool _flag)
    {
        animator.SetBool("Crouching", _flag);
    }
}