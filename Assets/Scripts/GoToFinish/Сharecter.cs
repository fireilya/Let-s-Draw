using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private float pushForce;

    [SerializeField]
    private float maxVelocity;

    [SerializeField]
    private DrawController drawController;

    private Rigidbody2D rb;
    private bool isPucnhed;
    void Start()
    {
        drawController = FindFirstObjectByType<DrawController>(); 
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && drawController.State.isMoveSignalEnabled && !isPucnhed)
        {
            var previousVelocity = rb.velocity;
            if (rb.bodyType == RigidbodyType2D.Static)
                rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(Vector2.right * pushForce);
            rb.velocity = rb.velocity.magnitude < maxVelocity ? rb.velocity : previousVelocity;
            isPucnhed = true;
        }
    }
}
