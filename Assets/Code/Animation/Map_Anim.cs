using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Anim : MonoBehaviour
{

    private Animator anim;
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    void Update() {

        if(GameManager.Instance.Get_Revealed_Count() > 5) {
            GameManager.Instance.Set_Revealed_Count(0);
            anim.SetBool("Big_Reveal", true);
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait() {
        audio.Play();
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Big_Reveal", false);
    }
}