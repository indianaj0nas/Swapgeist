using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class EnemyBase : MonoBehaviour
{

    [Header("Default")]
    public int health;
    public int worthPoints;
    public float enemySpeed;
    public float playerSpeed;
    public GameObject blood;
    [Space(20)]

    [Header("Attack variables")]
    [HideInInspector] public float attackDelay;
    public float timeBetweenAttacks;
    public GameObject damager;
    public float timeFreezeLenght;
    public float playerDamageRadius;
    public float enemyDamageRadius;
    [Space(20)]

    [HideInInspector]
    public CharacterController controller;
    [HideInInspector] public PlayerScript playerScript;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator anim;


    [HideInInspector] public Transform player;
    [HideInInspector]public GameObject[] allPlayers;
    [HideInInspector] public GameObject closestPlayer;
    [HideInInspector] public GameObject furthestPlayer;
    [HideInInspector] public GameObject targetPlayer;
    [HideInInspector] public GameObject closestPlayerBody;
    [HideInInspector] public GameObject furthestPlayerBody;
    [HideInInspector] public float distancePlayer;
    [HideInInspector] public bool possessed;
    [HideInInspector] public bool hurt;

    #region 
    public enum enemyStates
    {
        state_move,
        state_hurt,
        state_possesed,
        state_dead,
        state_pursuit
    }

    public enum possessedStates
    {
        pState_move,
        pState_hurt,
        pState_dead,
        pState_leaveBody,
        pState_empty
    }
    #endregion

    public enemyStates state = enemyStates.state_move;
    public possessedStates pState = possessedStates.pState_move;

    void Update()
    {
        Debug.Log(allPlayers);
        UpdateEnemyState();
        attackDelay += 1 * Time.deltaTime;
        // Health();

     /*    foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Player")
            {
                state = enemyStates.state_possesed;
                UpdatePossessedState();
            }
            else
            {
                pState = possessedStates.pState_leaveBody;
                UpdateEnemyState();
                //state = enemyStates.state_move;
            }
        } */
    }

    public void UpdateEnemyState()
    {
        switch (state)
        {
            case enemyStates.state_move:
                Move();
                break;
            case enemyStates.state_hurt:
                Hurt();
                break;
            case enemyStates.state_possesed:
                Possesed();
                break;
            case enemyStates.state_dead:
                Dead();
                break;
            case enemyStates.state_pursuit:
                Pursuit();
                break;
            default:
                break;
        }
    }

    public void UpdatePossessedState()
    {
        switch (pState)
        {
            case possessedStates.pState_move:
                MovePlayer();
                break;
            case possessedStates.pState_hurt:
                Hurt();
                break;
            case possessedStates.pState_dead:
                Dead();
                break;
            case possessedStates.pState_leaveBody:
                LeaveBody();
                break;
            case possessedStates.pState_empty:
                LeaveBody();
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "damager" && possessed && !hurt)
        {
            transform.position = Vector3.MoveTowards(transform.position, col.transform.position, -40 * Time.deltaTime);
            state = enemyStates.state_hurt;
        }

        if (col.tag == "myDamager" && !possessed && !hurt)
        {
            transform.position = Vector3.MoveTowards(transform.position, col.transform.position, -40 * Time.deltaTime);
            state = enemyStates.state_hurt;
        }
    }

    public virtual void MovePlayer() { /* override function in enemy type class */ }
    
    public virtual void LeaveBody() { /* override function in enemy type class */ }

    public virtual void Empty() { /* override function in enemy type class */ }

    public virtual void Move() { /* override function in enemy type class */ }

    public virtual void Hurt() { /* override function in enemy type class  */ }

    public virtual void Dead() { /* override function in enemy type class */ }

    public virtual void Pursuit() { /* override function in enemy type class */ }

    public virtual void Possesed() { /* override function in enemy type class */ }

    public void PlayerDistance()
    {
        if (allPlayers.Length == 1 && allPlayers.Length != 0)
        {
            allPlayers = allPlayers.OrderBy(allPlayer => Vector3.Distance(transform.position, allPlayer.transform.position)).ToArray();
            closestPlayer = allPlayers[0];
            closestPlayerBody = closestPlayer.transform.root.gameObject;
            if (targetPlayer != null)
                distancePlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);

            targetPlayer = closestPlayer;

           /*  if (closestPlayerBody.gameObject.layer == 9)
                targetPlayer = null; */
        }

        if (allPlayers.Length > 1 && allPlayers.Length != 0)
        {
            allPlayers = allPlayers.OrderBy(allPlayer => Vector3.Distance(transform.position, allPlayer.transform.position)).ToArray();
            closestPlayer = allPlayers[0];
            furthestPlayer = allPlayers[1];
            closestPlayerBody = closestPlayer.transform.root.gameObject;
            furthestPlayerBody = furthestPlayer.transform.root.gameObject;
            if (targetPlayer != null)
                distancePlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);

            if (closestPlayerBody.gameObject.layer == 9)
                targetPlayer = furthestPlayer;
            if (furthestPlayerBody.gameObject.layer == 9)
                targetPlayer = closestPlayer;

            if (furthestPlayerBody.gameObject.layer == 9 && closestPlayerBody.gameObject.layer == 9)
                targetPlayer = null;
        }

    }

    public void Health()
    {
        anim.SetInteger("HP", health);

        if (health < 1)
            state = enemyStates.state_dead;
    }

    public IEnumerator Pause(float p)
    {
        Time.timeScale = 0.1f;
        float pauseEndTime = Time.realtimeSinceStartup + p;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        Time.timeScale = 1;
    }

    void notHurt()
    {
        anim.SetBool("hurt", false);
        hurt = false;
        //justHurt = false;
    }

    public virtual void Attack(string tag, float damRadius) { }
}
