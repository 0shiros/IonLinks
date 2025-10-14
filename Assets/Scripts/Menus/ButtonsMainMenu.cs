using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ButtonsMainMenu : MonoBehaviour
{
    private Animator animator;
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        animator = GetComponent<Animator>();
        textMesh = GetComponentInChildren<TextMeshProUGUI>(true);
        textMesh.gameObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        animator.SetBool("OnMouseEnter", true);
        textMesh.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        animator.SetBool("OnMouseEnter", false);
        textMesh.gameObject.SetActive(false);
    }
}
