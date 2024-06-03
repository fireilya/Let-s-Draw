using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField]
    private Character character;

    [SerializeField]
    private List<Sprite> charactersSkins;

    private int currentIndex;

    void Start() { SpawnCharacter(); }

    public void SpawnCharacter() 
    { 
        var newCharacter = Instantiate(character.gameObject, transform.position, Quaternion.identity); 
        newCharacter.GetComponent<SpriteRenderer>().sprite = charactersSkins[currentIndex++];
        currentIndex %= charactersSkins.Count;
    }
}
