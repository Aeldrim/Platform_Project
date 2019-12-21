using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    CharacterMovement movement;
    Rigidbody2D rigidBody;
    CharacterInput input;
    Animator anim;

    int groundParamID;
    int speedParamID;
    int attackParamID;
    int fallParamID;

    void Start()
    {
        groundParamID = Animator.StringToHash("isOnGround");
        speedParamID = Animator.StringToHash("speed");
        attackParamID = Animator.StringToHash("isAttacking");
        fallParamID = Animator.StringToHash("verticalSpeed");

        //Referencia de los componentes necesarios
        movement	= GetComponent<CharacterMovement>();
		rigidBody	= GetComponent<Rigidbody2D>();
		input		= GetComponent<CharacterInput>();
		anim		= GetComponent<Animator>();

        //Si alguno de los componentes no existe...
		if(movement == null || rigidBody == null || input == null || anim == null)
		{
			//...Muestra un error y borra este componente
			Debug.LogError("Falta uno de los componentes necesarios del Jugador");
			Destroy(this);
		}
    }

    void Update()
    {
        //
        anim.SetBool(groundParamID,movement.isOnGround);
        anim.SetFloat(fallParamID, Mathf.Clamp(rigidBody.velocity.y, -1 , 1));
        anim.SetBool(attackParamID,input.attackPressed);

        anim.SetFloat(speedParamID,Mathf.Abs(input.horizontal));
    }
}
