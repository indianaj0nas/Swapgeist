using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class EnemyZombie : EnemyBase
{
    [Header("Roam parameters")]
    public float roamArea;
    public float roamTimer;
    public float viewDistance;
    [Space(20)]

    private Vector3 moveDirection = Vector3.zero;
    private float timer;

    private int currentHealth;

    void Awake()
    {
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
        timer = roamTimer;
        agent.speed = enemySpeed;
    }

    public override void Move()
    {
        PlayerDistance();
        PossessedOrNot();
        anim.SetInteger("HP", health);

        if (allPlayers.Length > 0)
            Roam(closestPlayer.transform.position);
        else
            Roam(transform.position);


        if (distancePlayer < viewDistance && targetPlayer != null)
        {
            state = EnemyBase.enemyStates.state_pursuit;
        }

        /* if (health == currentHealth - 1)
            state = EnemyBase.enemyStates.state_hurt; */

        if (health <= 0)
            state = EnemyBase.enemyStates.state_dead;
    }

    public override void Hurt()
    {
        currentHealth = health;
        hurt = true;

        health -= 1;
        anim.SetBool("hurt", true);
        Instantiate(blood, new Vector3(transform.position.x, -1.44f, transform.position.z), Quaternion.identity);
        StartCoroutine(Pause(timeFreezeLenght));
        Invoke("notHurt", 0.3f);

        if (health >= 1)
            state = EnemyBase.enemyStates.state_move;

        if (health <= 0)
            state = EnemyBase.enemyStates.state_dead;
    }

    public override void Possesed()
    {
        agent.enabled = false;
        playerScript = gameObject.GetComponentInChildren<PlayerScript>();
        GetComponent<SpriteRenderer>().color = Color.white;
        PossessedOrNot();
        UpdatePossessedState();
    }

    public override void Dead()
    {
        agent.enabled = false;
        Destroy(gameObject, 5f);
        anim.SetBool("dead", true);
    }

    public override void Pursuit()
    {
        PlayerDistance();
        PossessedOrNot();
        enemySpeed = +1;
        Debug.Log("pursuit " + targetPlayer);
        anim.SetBool("walking", true);
        agent.destination = targetPlayer.transform.position;

        if (distancePlayer > viewDistance)
            state = EnemyBase.enemyStates.state_move;

        if (distancePlayer < 2f)
            Attack("damager", enemyDamageRadius);
    }

    public override void MovePlayer()
    {
        PlayerMovement(playerScript.stickInput);
    }

    public override void LeaveBody()
    {
        agent.enabled = true;
        Color enemyColor = new Color();
        ColorUtility.TryParseHtmlString("#2492FFFF", out enemyColor);
        GetComponent<SpriteRenderer>().color = enemyColor;
    }

    /* Functions */

    public void PlayerMovement(Vector3 playerInput)
    {
        moveDirection = playerInput;
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= playerSpeed;
        controller.Move(moveDirection * playerSpeed * Time.deltaTime);

        if (playerInput.magnitude >= 0.2f)
        {
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
        }
    }

    public override void Attack(string tag, float damRadius)
    {
        if (attackDelay >= timeBetweenAttacks)
        {
            damager.SetActive(true);
            damager.GetComponent<SphereCollider>().radius = damRadius;
            damager.tag = tag;
            attackDelay = 0;
        }
    }

    void Roam(Vector3 roamCenter)
    {
        timer += Time.deltaTime;

        if (timer >= roamTimer)
        {
            Vector3 newPos = RandomNavSphere(roamCenter, roamArea, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }

        if (agent.velocity.magnitude > 0.2f)
        {
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    public void PossessedOrNot()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Player" && !possessed)
            {
                possessed = true;
                state = enemyStates.state_possesed;
                pState = possessedStates.pState_move;
            }
            if (child.gameObject.tag != "Player" && possessed)
            {
                possessed = false;
                state = enemyStates.state_move;
                pState = possessedStates.pState_leaveBody;
                //pState= possessedStates.pState_empty;
            }
        }
    }
}
