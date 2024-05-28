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

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Finish")
            levelUI.ShowLevelResult();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            var previousVelocity = rb.velocity;
            if (rb.bodyType == RigidbodyType2D.Static)
                rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(Vector2.right * pushForce);
            rb.velocity = rb.velocity.magnitude < maxVelocity ? rb.velocity : previousVelocity;
        }
    }
}
