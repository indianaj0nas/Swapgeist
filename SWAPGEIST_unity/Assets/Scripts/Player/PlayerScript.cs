using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PlayerScript : MonoBehaviour
{
    public int playerId = 0; // The Rewired player id of this character
    public float maxGhostTime;
    public GameObject ghost;
    public GameObject damager;
    public GameObject teethDamager;
    public GameObject slimeDamager;
    public GameObject possessParticle;
    [HideInInspector]
    public Vector3 stickInput;

    //private Player player; // The Rewired Player
    private float ghostTime;
    private int posClicks;
    private float deathMargin = 0.3f;
    private float attackDelayTime = 0.5f;
    private float chargeAttackTimer;
    private float maxChargeAttackTimer = 2.5f;
    private Transform currentBody;
    private Transform monsterParent;
    private Transform parentObject;
    private EnemyBase enemy;

    private Rewired.Player player { get { return ReInput.isReady ? ReInput.players.GetPlayer(playerId) : null; } }


    void Awake()
    {
        //player = ReInput.players.GetPlayer(playerId);

        posClicks = 0;
        transform.parent = ghost.transform;
        transform.position = ghost.transform.position;
        ghostTime = maxGhostTime;
        ghostTime = 15f;
    }

    void Update()
    {
        if (!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if (player == null) return;
        JoystickInput();
        GhostAgain();
        //GhostTimer();
        if (transform.parent.tag == "enemy")
            CallAttack();

        PhysicalDeath();
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "enemy" && transform.parent == ghost.transform && posClicks == 0)
        {
            enemy = col.transform.GetComponent<EnemyBase>();

            if (player.GetButtonDown("Possess") && enemy.health > 0)
            {
                transform.parent = col.transform;
                transform.position = new Vector3(col.transform.position.x, col.transform.position.y, col.transform.position.z);
                //possessing = "enemy";
                Instantiate(possessParticle, transform.position, Quaternion.identity);
                posClicks = 1;
            }
        }
    }

    void GhostAgain()
    {
        if (player.GetButtonDown("Possess"))
        {
            if (posClicks == 1)
                posClicks = posClicks + 1;
            else if (posClicks == 2)
                posClicks = posClicks + 1;

            if (transform.parent != ghost.transform && posClicks == 2)
            {
                Instantiate(possessParticle, transform.position, Quaternion.identity);
                //StartCoroutine(Pause(0.1f));
                transform.parent = null;
                transform.parent = ghost.transform;
                ghost.SetActive(true);
                ghost.transform.position = transform.position;
                transform.position = new Vector3(ghost.transform.position.x, ghost.transform.position.y, ghost.transform.position.z);
                Invoke("ResetClick", 0.1f);
            }
        }
    }

    void ResetClick() { posClicks = 0; }

    void CallAttack()
    {
        if (player.GetButtonDown("Attack"))
        {
            enemy.Attack("myDamager", enemy.playerDamageRadius);

            /* if (enemyScript.shooting == false && attackDelayTime <= 0)
            {
                if (enemyScript.damager.activeInHierarchy == false)
                    enemyScript.damager.SetActive(true);
                enemyScript.damager.tag = "myDamager";
                attackDelayTime = 0.5f;
            }

            if (enemyScript.shooting == true && enemyScript.attackDelay >= enemyScript.timeBetweenAttacks/2)
            {
                if(enemyScript.wand != null)
                enemyScript.wand.SetActive(true);
                //Vector3 projectileDirection = (stickInput - transform.position).normalized;
                Quaternion _lookRotation = Quaternion.LookRotation(stickInput);
                GameObject bullet = Instantiate(enemyScript.damager, new Vector3(transform.position.x, transform.position.y + .2f, transform.position.z), _lookRotation) as GameObject;
                bullet.tag = "myDamager";
                //attackDelayTime = 0.5f;
                enemyScript.attackDelay = 0;
                //enemyScript.shotFired = false;
            } */
        }

        attackDelayTime -= 1f * Time.deltaTime;
    }

    public void PhysicalDeath()
    {
        parentObject = transform.root;

        if (parentObject.gameObject.layer != 9)
            enemy = parentObject.GetComponent<EnemyBase>();

        //if (transform.parent.Find("HP").gameObject.activeInHierarchy == false)
        if (enemy == null)
            return;

        if (enemy.health <= 0)
        {
            deathMargin -= 1f * Time.deltaTime;

            if (deathMargin <= 0)
                Destroy(gameObject);
        }

        if (transform.parent.tag != "enemy")
        {
            deathMargin = 0.5f;
        }
    }

    public void JoystickInput()
    {
        float deadzone = 0f;
        stickInput = new Vector3(player.GetAxis("MoveHorizontal"), 0, player.GetAxis("MoveVertical"));
        if (stickInput.magnitude < deadzone)
            stickInput = Vector3.zero;
        else
            stickInput = stickInput.normalized * ((stickInput.magnitude - deadzone) / (1 - deadzone));
    }

    void OnDestroy()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        Score scoreScript = gameManager.GetComponent<Score>();

        if (scoreScript.currentScore > scoreScript.highScore)
            scoreScript.highScore = scoreScript.currentScore;

        //if(scoreScript.highScore == scoreScript.currentScore)
        //scoreScript.currentScore = 0;
    }

    public IEnumerator Pause(float p)
    {
        Time.timeScale = 0.3f;
        //float pauseEndTime = Time.realtimeSinceStartup + p;
        /* while (Time.realtimeSinceStartup < pauseEndTime)
         {
             yield return 0;
         }*/
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1;
    }
}