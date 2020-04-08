using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Enemy : MonoBehaviour
{

    public float playerSpeed = 6.0F;
    public float enemySpeed = 12f;
    public float gravity = 20.0F;
    public bool increaseSpeedWhenDamaged;
    public bool hover;
    public bool shooting;
    public float projectileSpeed;
    public float hoverIntensity;
    public float secondsForOneLength;
    public float viewDistance;
    public float attackWithinRange;
    public float timeBetweenAttacks;
    public float enemyDamageRadius;
    public float playerDamageRadius;
    public float roamArea;
    public float roamTimer;
    public float timeFreezeLenght;
    public float destroyBodyDelay;
    public bool chargeAttacks;
    public float maxChargeAttackTimer;
    public bool launcher;
    public float thrustForce;
    public int health;
    public int worthPoints;
    public GameObject damager;
    public GameObject blood;
    public GameObject wand;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 randomPosition;
    private Vector3 frometh;
    private Vector3 untoeth;
    private bool possess;
    private bool neverDone;
    private bool functionCalled;
    private bool positionReached;
    private bool justHurt;
    private bool withinTimeFreezeRange;
    private bool possessed;
    private bool playersInvisible;
    [HideInInspector]
    public bool launch;
    [HideInInspector]public bool shotFired;
    private Transform player;
    private GameObject[] allPlayers;
    private GameObject closestPlayer;
    private GameObject furthestPlayer;
    private GameObject targetPlayer;
    private GameObject closestPlayerBody;
    private GameObject furthestPlayerBody;
    private GameObject ignoreThis;
    [HideInInspector]public float attackDelay;
    private float distancePlayer;
    private float timer;
    private float chargeAttackTimer;
    private int oldHealthValue;
    private PlayerScript playerScript;
    CharacterController controller;
    Animator anim;
    NavMeshAgent agent;
    SphereCollider damagerCollider;
    Rigidbody rigidbody;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        if(launcher == true)
        rigidbody = GetComponent<Rigidbody>();
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
        randomPosition = new Vector3(Random.Range(-15f, 15f), transform.position.y, Random.Range(-15f, 15f));
        positionReached = false;
        functionCalled = false;
        anim = GetComponent<Animator>();
        possessed = false;
        GameObject mobSpawner = GameObject.Find("MobSpawner");
        MobSpawnScript mobSpawnScript = mobSpawner.GetComponent<MobSpawnScript>();
        possessed = false;
        timer = roamTimer;
        oldHealthValue = health;
        agent.speed = enemySpeed;
        damagerCollider = damager.transform.GetComponent<SphereCollider>();
    }

    void Update()
    {
        FindClosestPlayer();
        IsCurrently();
        ControlCharacterController();
        Hover();
        HP();
        IncreaseSpeed();
        ChargeAttack();
        Shoot();
        LaunchThyself();

        if (possessed && health > 0)
        {
            Movement();
        }
        else if (!possessed && health > 0)
        {
            FightMode();
        }

        attackDelay += 1 * Time.deltaTime;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "damager" && possessed)
        {
            health -= 1;
            transform.position = Vector3.MoveTowards(transform.position, col.transform.position, -40 * Time.deltaTime);
            anim.SetBool("hurt", true);
            justHurt = true;
            Instantiate(blood, new Vector3(transform.position.x, -1.44f, transform.position.z), Quaternion.identity);
            StartCoroutine(Pause(timeFreezeLenght));
            Invoke("notHurt", 0.3f);
        }
        else if (col.tag == "myDamager" && !possessed)
        {
            health -= 1;
            CancelInvoke("Attack");
            transform.position = Vector3.MoveTowards(transform.position, col.transform.position, -40 * Time.deltaTime);
            anim.SetBool("hurt", true);
            justHurt = true;
            Instantiate(blood, new Vector3(transform.position.x, -1.44f, transform.position.z), Quaternion.identity);
            StartCoroutine(Pause(timeFreezeLenght));
            Invoke("notHurt", 0.3f);
        }

        if (col.tag == "gas" && gameObject.layer != 12)
        {
            health -= 2;
            transform.position = Vector3.MoveTowards(transform.position, col.transform.position, -40 * Time.deltaTime);
            anim.SetBool("hurt", true);
            justHurt = true;
            Invoke("notHurt", 0.3f);
        }
    }

    void HP()
    {
        if (health == 3)
            anim.SetInteger("HP", 3);
        if (health == 2)
            anim.SetInteger("HP", 2);
        if (health == 1)
            anim.SetInteger("HP", 1);

        if (health <= 0)
        {
            if (!functionCalled)
            {
                AddToScore();
                functionCalled = true;
            }
            Destroy(gameObject, destroyBodyDelay);
            anim.SetBool("dead", true);
        }
    }

    void NormalAttack()
    {
        /*GameObject childObject = Instantiate(damager, new Vector3(transform.position.x, transform.position.y + .2f, transform.position.z), Quaternion.identity) as GameObject;
        childObject.transform.parent = gameObject.transform;*/
        damager.SetActive(true);
        damager.tag = "damager";
        attackDelay = 0;
    }

    void Shoot()
    {
        if (shotFired)
        {
            Vector3 projectileDirection = (targetPlayer.transform.position - transform.position).normalized;
            Quaternion _lookRotation = Quaternion.LookRotation(projectileDirection);
            GameObject bullet = Instantiate(damager, new Vector3(transform.position.x, transform.position.y + .2f, transform.position.z), _lookRotation) as GameObject;
            Vector3 bulPos = bullet.transform.position;
            bullet.tag = "damager";
            bulPos.y = transform.position.y;
            attackDelay = 0;
            shotFired = false;
        }
    }

    public void FightMode()
    {
        //attackDelay += 1 * Time.deltaTime;

        if (distancePlayer > 5f || targetPlayer == null)
        {
            if (health > 0) //move to random position
            {
                anim.SetBool("walking", true);
                timer += Time.deltaTime;

                if (timer >= roamTimer)
                {
                    Vector3 newPos = RandomNavSphere(transform.position, roamArea, -1);
                    agent.SetDestination(newPos);
                    timer = 0;
                }
            }
            else { anim.SetBool("walking", false); }
        }

        if (distancePlayer < 5f && health > 0)
        {
            withinTimeFreezeRange = true;
        }
        else { withinTimeFreezeRange = false; }

        if (distancePlayer < viewDistance && health > 0 && targetPlayer != null)
        {
            anim.SetBool("walking", true);
            agent.destination = targetPlayer.transform.position;
        }

        if (distancePlayer < attackWithinRange && attackDelay >= timeBetweenAttacks && health > 0 && targetPlayer != null && chargeAttacks == false)
        {
            if (!shooting)
                NormalAttack();
            if (shooting)
            {
                shotFired = true;
                wand.SetActive(true);
            }
        }

    }

    void Hover()
    {
        if (hover == true && health > 0)
        {
            Physics.IgnoreLayerCollision(10, 8, true);
            Physics.IgnoreLayerCollision(10, 10, true);
            Physics.IgnoreLayerCollision(10, 11, true);

            frometh = new Vector3(transform.position.x, -.5f, transform.position.z);
            untoeth = new Vector3(transform.position.x, hoverIntensity, transform.position.z);
            transform.position = Vector3.Lerp(frometh, untoeth,
            Mathf.SmoothStep(0f, 1f,
            Mathf.PingPong(Time.time / secondsForOneLength, 1f)));
        }

    }

    void IsCurrently()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Player")
            {
                player = child;
                player.GetComponent<PlayerScript>();
                possessed = true;
                damagerCollider.radius = playerDamageRadius;
            }
            else
            {
                //Debug.Log("URGH");
                possessed = false;
                damagerCollider.radius = enemyDamageRadius;
            }
        }

        if (possessed)
        {
            agent.enabled = false;

            if (!justHurt)
            {
                GetComponent<SpriteRenderer>().color = Color.white;
            }

            if (justHurt)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        if (!possessed)
        {

            if (!justHurt)
            {
                Color enemyColor = new Color();
                ColorUtility.TryParseHtmlString("#2492FFFF", out enemyColor);
                GetComponent<SpriteRenderer>().color = enemyColor;
                //gameObject.SetActive(false);
                FightMode();
                agent.enabled = true;
            }
            if (justHurt)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                agent.enabled = false;
                //if (withinTimeFreezeRange)
                //StartCoroutine(Pause(0.1f));
            }
        }
    }

    void notHurt()
    {
        anim.SetBool("hurt", false);
        justHurt = false;
    }

    public void AddToScore()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        Score scoreScript = gameManager.GetComponent<Score>();
        scoreScript.currentScore += worthPoints;
    }

    public void ControlCharacterController()
    {
        CharacterController controller = GetComponent<CharacterController>();

        if (health <= 0)
            controller.enabled = false;


        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    public void Movement()
    {
        if (controller.isGrounded)
        {
            playerScript = closestPlayer.GetComponent<PlayerScript>();
            moveDirection = playerScript.stickInput;
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= playerSpeed;
            transform.position += Vector3.ClampMagnitude(moveDirection, playerSpeed) * Time.deltaTime;

            if (playerScript.stickInput.magnitude >= 0.5f)
                anim.SetBool("walking", true);
            else
            {
                anim.SetBool("walking", false);
            }
        }
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

    public void FindClosestPlayer()
    {
        if (allPlayers.Length == 1)
        {
            allPlayers = allPlayers.OrderBy(allPlayer => Vector3.Distance(transform.position, allPlayer.transform.position)).ToArray();
            closestPlayer = allPlayers[0];
            //furthestPlayer = allPlayers[1];
            //if(closestPlayerBody == null) return;
            closestPlayerBody = closestPlayer.transform.root.gameObject;
            //furthestPlayerBody = furthestPlayer.transform.root.gameObject;
            if (targetPlayer != null)
                distancePlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);

            targetPlayer = closestPlayer;

            /*if (closestPlayerBody.gameObject.layer == 9)
                targetPlayer = furthestPlayer;
            if (furthestPlayerBody.gameObject.layer == 9)
                targetPlayer = closestPlayer;*/

            if (closestPlayerBody.gameObject.layer == 9)
                targetPlayer = null;
        }

        if (allPlayers.Length > 1)
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

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    void IncreaseSpeed()
    {
        if (increaseSpeedWhenDamaged)
        {
            if (health < oldHealthValue)
            {
                agent.speed = enemySpeed + 3f;
                oldHealthValue = health;
            }
        }
    }

    void ChargeAttack()
    {
        if (chargeAttacks == true && health > 0)
        {
            if (targetPlayer == null)
                return;

            if (distancePlayer < 7f && possessed == false)
            {
                chargeAttackTimer += Time.deltaTime;
                transform.position = new Vector3(transform.position.x + Random.RandomRange(-chargeAttackTimer / 30, chargeAttackTimer / 30), transform.position.y, transform.position.z + Random.RandomRange(-chargeAttackTimer / 30, chargeAttackTimer / 30));
                anim.SetBool("hurt", true);
            }

            if (chargeAttackTimer >= maxChargeAttackTimer)
            {
                Instantiate(damager, gameObject.transform.position, Quaternion.identity);
                chargeAttackTimer = 0;
            }
        }

    }

    void LaunchThyself()
    {
        if(launcher == true && launch == true)
        {
            rigidbody.AddForce(playerScript.stickInput * thrustForce);
            Debug.Log(playerScript.stickInput);
        }
    }
}
