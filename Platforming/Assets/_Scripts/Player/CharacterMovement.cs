using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour
{
    public bool drawDebugRaycasts = true;	//Checks del escenario deben ser visibles?

   	[Header("Movement Properties")]
	public float speed = 8f;				//Velocidad del jugador
	public float coyoteDuration = .05f;		//Tiempo extra para saltar despues de caer
	public float maxFallSpeed = -25f;		//Velocidad maxima a la que puede caer

    [Header("Jump Properties")]
	public float jumpForce = 6.3f;			//Fuerza inicial del salto
	public float jumpHoldForce = 1.9f;		//Fuerza incremental cuando se mantiene el salto
	public float jumpHoldDuration = .1f;	//Cuando tiempo se puede mantener la tecla de salto

	[Header("Attack Properties")]
	public float startTimeBtwAttacks;		//Tiempo minimo entre ataques
	public float attackRange;				//Rango del ataque
	public Transform attackPosition;		//Posicion inicial del ataque


    [Header("Environment Check Properties")]
	public float footOffset = .4f;			//Offset en X del raycast de los pies
	public float footOffsetY = .1f;			//Offset en Y del raycast de los pies
	public float groundDistance = .2f;		//Distancia a la que el personaje se considera que esta en el suelo
	public LayerMask groundLayer;			//Mascara del suelo
	public LayerMask whatIsAttackable;		//Mascara que es atacable

	[Header ("Status Flags")]
	public bool isOnGround;					//El personaje esta en el suelo?
	public bool isJumping;					//El personaje esta saltando?
	public bool recovering;					//El personaje esta recuperandose?


    CharacterInput input;					//Inputs del personaje
	Rigidbody2D rigidBody;					//Componente RigidBody
	Animator animator;						//Componente Animator

    float jumpTime;							//Variable para mantener la duracion de salto
	float coyoteTime;						//Variable para mantener la duracion coyote
	float attackTime;						//Variable para tiempo entre ataques

	float originalXScale;					//Escala original en el eje X
	int direction = 1;						//Direccion a la que mira el personaje

    void Start()
    {
        //Referencia de los componentes necesarios
		input = GetComponent<CharacterInput>();
		rigidBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

        //Graba la escala en x original
		originalXScale = transform.localScale.x;
    }

    void Update()
    {
        //Checkea el espacio para determinar su estado
		PhysicsCheck();

		//Procesar movimientos en tierra y aire
		GroundMovement();		
		MidAirMovement();
    }

    	void PhysicsCheck()
	{
		//Inicia asumiento que el personaje no esta en el suelo
		isOnGround = false;

		//Lanza Raycasts para cada pie
		RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, footOffsetY), Vector2.down, groundDistance);
		RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, footOffsetY), Vector2.down, groundDistance);

		//Si cualquier rayo golpea con el suelo, el personaje esta en el suelo
		if (leftCheck || rightCheck)
			isOnGround = true;
	}

    void GroundMovement()
	{
		//Calcula la velocidad, basado en Inputs
		float xVelocity = speed * input.horizontal;

		//Si la velocidad y la direccion no son equivalentes, gira el personaje
		if (xVelocity * direction < 0f)
			FlipCharacterDirection();

		//Aplica la velocidad 
		rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);

		//Si esta en el suelo, extiende el tiempo de salto Coyote
		if (isOnGround)
			coyoteTime = Time.time + coyoteDuration;
		
		//Si el tiempo de ataque es 0 puedo atacar
		if(attackTime <= 0)
		{
			if(input.attackPressed)
			{	
				Collider2D[] objectsToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, whatIsAttackable); //
				attackTime = startTimeBtwAttacks;
			}
		}
		else
		{
			rigidBody.velocity = new Vector2 (xVelocity/2,rigidBody.velocity.y);
			attackTime -= Time.deltaTime;
		}
	}

	void MidAirMovement()
	{
		// Si se presiona la tecla de salto y el jugador aún no está saltando y el jugador está en el suelo o dentro de la ventana de tiempo de coyote
		if (input.jumpPressed && !isJumping && (isOnGround || coyoteTime > Time.time))
		{
			//...El jugador ya no está en el suelo y está saltando....
			isOnGround = false;
			isJumping = true;

			//...Registra el tiempo que el jugador dejará de poder aumentar su salto...
			jumpTime = Time.time + jumpHoldDuration;

			//...Añade la fuerza de salto al Rigidbody...
			rigidBody.AddForce(new Vector2(0f, jumpForce),ForceMode2D.Impulse);
		}
		//De lo contrario, si está actualmente dentro de la ventana de tiempo de salto...
		else if (isJumping)
		{
			//...Y el boton de salto se mantiene, aplica la fuerza incremental al Rigidbody...
			if (input.jumpHeld)
				rigidBody.AddForce(new Vector2(0f, jumpHoldForce),ForceMode2D.Impulse);

			//...Y si termino el tiempo de salto, settea isJumping a falso
			if (jumpTime <= Time.time)
				isJumping = false;
		}

		//Si el jugador está cayendo muy rápido, reduce la velocidad de Y
		if (rigidBody.velocity.y < maxFallSpeed)
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxFallSpeed);
	}

	public void Die(){
		Destroy(this.gameObject);
	}

    void FlipCharacterDirection()
	{
		//Gira el personaje dando vuelta la direccion
		direction *= -1;

		//Registra la escala actual
		Vector3 scale = transform.localScale;

		//Establece la escala de X para que sea la original * direccion
		scale.x = originalXScale * direction;

		//Aplica la nueva escala
		transform.localScale = scale;
	}


	
	// Estos dos métodos de Raycast cubren Physics2D.Raycast () y proporcionan algunas funcionalidades
	RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length)
	{
		return Raycast(offset, rayDirection, length, groundLayer);
	}

	RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask)
	{		
		// Registra la posición del jugador
		Vector2 pos = transform.position;
		
		// Envía el raycast deseado y registra el resultado
		RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);

		// Si se quiere mostrar rayos en la escena ...
		if (drawDebugRaycasts)
		{
			//... determina el color en función de si golpeó ...
			Color color = hit ? Color.red : Color.green;
			//... y dibuja el rayo en la escena
			Debug.DrawRay(pos + offset, rayDirection * length, color);
		}

		// Devuelve los resultados del Raycast
		return hit;
	}
}
