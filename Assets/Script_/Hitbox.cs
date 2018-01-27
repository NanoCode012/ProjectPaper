using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {
    
    public AudioClip clip;

    private void OnTriggerEnter2D(Collider2D col)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position);
        Destroy(col.gameObject);
        print("die");
            
    }
}
