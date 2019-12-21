using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    static GameManager current;

    public float deathDuration = 1.5f; //Cuanto demora en resetearse luego de la muerte del jugador

    // Start is called before the first frame update
    void Start()
    {
        //Set this as the current game manager
		current = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlayerDied (){
        current.Invoke("RestartScene", current.deathDuration);
    }

    void RestartScene(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
}
