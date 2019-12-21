using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//Para asegurarse que el script corra primero, para evitar movimientos poco responsivos
[DefaultExecutionOrder(-100)] 
public class CharacterInput : MonoBehaviour
{
    public bool testTouchControlsInEditor;

    [HideInInspector] public float horizontal;		//Float que guarda el Input horizontal
	[HideInInspector] public bool jumpHeld;			//Bool que guarda si se mantiene el salto
	[HideInInspector] public bool jumpPressed;		//Bool que guarda si se presiona el salto
    [HideInInspector] public bool attackPressed;	//Bool que guarda si se presiona el ataque
    
	//[HideInInspector] public bool crouchHeld;		//Bool que guarda si se mantiene agachar
	//[HideInInspector] public bool crouchPressed;	//Bool que guarda si se presiona agachar

    bool readyToClear;                              //Bool para mantener los Inputs en sincronia

    void Update()
    {
        //Limpia los valores de Input
        ClearInput();


        //Procesa los Inputs de mouse, teclado, mandos
        ProcessInputs();

        //Procesa los Inputs mobile (touch)
        ProcessTouchInputs();

        //Ancla los valores del Input horizontal para estar entre -1 y 1
        horizontal = Mathf.Clamp(horizontal, -1f, 1f);
    }

    void FixedUpdate()
    {
        //Establece una marca que permite a los Inputs ser limpiados en el proximo Update()
        //Esto asegura que se utilicen todos los inputs
        readyToClear = true;
    }

    void ClearInput()
    {
        //Si no esta listo para limpiar los Inputs, sale
        if(!readyToClear)
            return;

        //Resetea los Inputs
        horizontal = 0f;
        jumpPressed = false;
        jumpHeld = false;
        attackPressed  = false;


        readyToClear = false;
    }

    void ProcessInputs()
    {
        //Acumula el Input del axis horizontal
        horizontal += Input.GetAxis("Horizontal");
        if(Input.GetAxis("Horizontal") >= .2f)
        {
            horizontal = 1;
        } else if (Input.GetAxis("Horizontal") <= -.2f)
        {
            horizontal = -1;
        } else 
        {
            horizontal = 0;
        }

        //Acumula los botones Input
        jumpPressed = jumpPressed || Input.GetButtonDown("Jump");
        jumpHeld = jumpHeld || Input.GetButton("Jump");
        attackPressed = attackPressed || Input.GetButtonDown("Attack");
    }

    void ProcessTouchInputs()
    {
        //Si no es una plataforma Mobile, y no esta en prueba de editor, sale
        if(!Application.isMobilePlatform && !testTouchControlsInEditor)
            return;
        
        //Acumula el Input del axis horizontal
        //horizontal += CrossPlatformInputManager.GetAxis("Horizontal");
        if(CrossPlatformInputManager.GetAxis("Horizontal") >= .2f)
        {
            horizontal = 1;
        } else if (CrossPlatformInputManager.GetAxis("Horizontal") <= -.2f)
        {
            horizontal = -1;
        } else 
        {
            horizontal = 0;
        }

        //Acumula los botones Input
        jumpPressed = jumpPressed || CrossPlatformInputManager.GetButtonDown("Jump");
        jumpHeld = jumpHeld || CrossPlatformInputManager.GetButton("Jump");
        attackPressed = attackPressed || CrossPlatformInputManager.GetButtonDown("Attack");
    }
}
