using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharecter : MonoBehaviour
{
    [SerializeField]
    private float pushForce;

    [SerializeField]
    private float maxVelocity;

    [SerializeField]
    private LevelUI levelUI;

    [SerializeField]
    private DrawController drawController;

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && drawController.State.isMoveSignalEnabled)
        {
            var previousVelocity = rb.velocity;
            if (rb.bodyType == RigidbodyType2D.Static)
                rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(Vector2.right * pushForce);
            rb.velocity = rb.velocity.magnitude < maxVelocity ? rb.velocity : previousVelocity;
        }
    }
}
