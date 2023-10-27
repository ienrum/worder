using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordBoxManager : MonoBehaviour
{
    Animator animator;
    TextMeshProUGUI ui;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ui = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ui.text != "")
        {
            animator.SetBool("isFilled", true);
        }
        else
        {
            animator.SetBool("isFilled", false);
        }
    }
}
