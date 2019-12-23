using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    static GameManager current;
    SceneFader sceneFader;

    public float deathDuration = 1.5f; //Cuanto demora en resetearse luego de la muerte del jugador

    // Start is called before the first frame update
    void Awake()
    {
        //Set this as the current game manager
		current = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlayerDied (){
        //Si hay un scene fader, hacer que haga un fade out
        if(current.sceneFader != null)
			  current.sceneFader.FadeSceneOut();
        //Invoca al metodo RestartScene
        current.Invoke("RestartScene", current.deathDuration);
    }

    void RestartScene(){
      //Carga la escena
		  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    public static void RegisterSceneFader(SceneFader fader){
        //Guarda la referencia al scenefader
        current.sceneFader = fader;
    }

}
