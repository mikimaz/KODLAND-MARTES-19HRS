using UnityEngine;
using Photon.Pun;
public class Enemy : MonoBehaviourPunCallbacks
{
    [SerializeField] protected int health;
    [SerializeField] protected float attackDistance;    
    [SerializeField] protected int damage;
    [Serial
    protected GameObject[] players;

    public virtual void Move() 
    {
    }
    public virtual void Attack() 
    {
    }
    [PunRPC]
    public void ChangeHealth(int count)
    {
        health -= count;
        if(health <= 0)
        {
            dead = true;
            GetComponent<Collider>().enabled = false;
            anim.enabled = true;
            anim.SetBool("Die", true);
            
        }
    }
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject; 
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        CheckPlayers();
    }

    private void Update()
    {
        //Declarar una variable que almacenará una distancia mínima
        //Mathf.Infinity - infinito positivo
        float closestDistance = Mathf.Infinity;
        //Pasando por la lista de jugadores
        foreach (GameObject closestPlayer in players)
        {
            //Calculando la distancia entre el enemigo y el jugador
            float checkDistance = Vector3.Distance(closestPlayer.transform.position, transform.position);
            //Si la distancia hasta este jugador es menor que la distancia hasta el jugador anterior, entonces...
            if (checkDistance < closestDistance)
            {
                //Si el jugador anterior está vivo
                if(closestPlayer.GetComponent<PlayerController>().dead == false)
                {
                    //Guardar el jugador actual como el jugador más cercano
                    player = closestPlayer;
                    //Cambiando el valor de closestDistance a la distancia a este jugador
                    closestDistance = checkDistance;
                }
            }
        }
        //Comprobando si la variable jugador tiene un jugador en ella
        //Esta comprobación nos ayudará a prevenir errores
        if (player != null)
        {
            //El resto del script no cambió con respecto a las lecciones anteriores
            distance = Vector3.Distance(transform.position, player.transform.position);
            if (!dead)
            {
                Attack();
            }
        }
    }
    private void FixedUpdate()
    {
        if (!dead && player != null)
        {
            Move();
        }
    }

    void CheckPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        Invoke("CheckPlayers", 3f);
    }

    public void GetDamage(int count)
    {
        photonView.RPC("ChangeHealth", RpcTarget.All, count);
    }
}
