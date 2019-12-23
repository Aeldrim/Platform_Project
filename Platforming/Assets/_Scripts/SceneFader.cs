using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{

    Animator anim;              //Referencia al componente Animator
    int fadeParamID;            //El ID del parametro del animator

    // Start is called before the first frame update
    void Start()
    {
        //Asigna el componente Animator
        anim = GetComponent<Animator>();    

        fadeParamID = Animator.StringToHash("Fade");

        GameManager.RegisterSceneFader(this);
    }

    // Update is called once per frame
    public void FadeSceneOut()
    {
        anim.SetTrigger(fadeParamID);
    }
}
