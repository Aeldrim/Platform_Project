using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Properties")]
    public int maxHealth = 10;  //Vida maxima del enemigo  
    public Animator anim;       //Animador del enemigo

    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage (int damage){
        currentHealth -= damage;
        anim.SetTrigger("Hurt");

        if (currentHealth <= 0){
            Die();
        }
    }

    void Die(){
        anim.SetBool("isDead", true);

        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(DestroyGO());
    }

    IEnumerator DestroyGO(){
            yield return new WaitForSeconds (1);

            Debug.Log("termino");
            this.gameObject.SetActive(false);
        }
    }
