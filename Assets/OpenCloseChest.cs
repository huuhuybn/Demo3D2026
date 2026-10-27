using UnityEngine;

public class OpenCloseChest : MonoBehaviour
{
    
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            animator.SetInteger("status",1);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetInteger("status",0);
        }
    }
}
