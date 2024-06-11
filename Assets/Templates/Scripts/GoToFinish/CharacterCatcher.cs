using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCatcher : MonoBehaviour
{
    [SerializeField]
    private CharacterSpawner spawner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        spawner.SpawnCharacter();
    }
}
