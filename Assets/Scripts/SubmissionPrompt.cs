using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmissionPrompt : MonoBehaviour
{
    [SerializeField] private float animSpeed;
    private Animator animator;
    private bool isClosing;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void OnEnable()
    {
        animator.SetTrigger("Open");
    }
    public void CloseSubmissionPrompt()
    {
        if(isClosing) return;
        isClosing = true;
        animator.SetTrigger("Close");
        StartCoroutine(DisableObject());
    }
    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(animSpeed);
        isClosing = false;
        gameObject.SetActive(false);
    }
}
