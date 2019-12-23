using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{

    [Header("Stats Properties")]
	public int health;						//Vida actual del jugador
	public int maxHealth;					//Vida maxima del jugador

	public Image[] hearths;					//Vidas en ui
	public Sprite fullHearth;				//Imagen a mostrar cuando el corazon esta lleno
	public Sprite emptyHearth;				//Imagen a mostrar cuando el corazon esta vacio

    bool isAlive;                           //Almacena el estado del jugador
    int trapLayer;                          //La capa en la que estan las trampas //Es un int, para que sea mas efectiva en mobiles
    int enemyLayer;                         //La capa en la que esta el enemigo

    // Start is called before the first frame update
    void Start()
    {
        
		health = maxHealth;     //Settea la Vida
        isAlive = true;         
        trapLayer = LayerMask.NameToLayer ("Traps");
        enemyLayer = LayerMask.NameToLayer ("Enemy");

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i <hearths.Length; i++){
            if (i < health){
                hearths[i].sprite = fullHearth;
            } else
            {
                hearths[i].sprite = emptyHearth;
            }

            if(i < maxHealth){
                hearths[i].enabled = true;
            } else{
                hearths[i].enabled = false;
            }
        }	
    }

    void OnTriggerEnter2D (Collider2D col){
        if(col.gameObject.layer != trapLayer || !isAlive)
            return;

        health = 0;
        isAlive = false;
        
        gameObject.SetActive(false);
        GameManager.PlayerDied();
    }

    void OnCollisionEnter2D (Collision2D o){
        if(o.gameObject.layer != enemyLayer || !isAlive)
            return;

        Debug.Log("Colisiono");
        
        health --;
        if(health <= 0){
            isAlive = false;
            gameObject.SetActive(false);
            GameManager.PlayerDied();
        }
    }
}
