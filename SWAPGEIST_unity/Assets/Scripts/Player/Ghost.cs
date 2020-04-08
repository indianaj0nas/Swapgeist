using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public float secondsForOneLength;
    public float hoverIntensity;
    public PlayerScript playerScript;

    private Vector3 frometh;
    private Vector3 untoeth;
    private Vector3 moveDirection = Vector3.zero;
    public GameObject player;


    private bool currentCharacter;

    void Awake()
    {
        //player = GameObject.Find("PLAYER");
        playerScript = player.GetComponent<PlayerScript>();

        //SetSortLayer();
    }

    void Update()
    {
        Movement();
        IsCurrently();
        Hover();
        SetSortLayer();
    }

    void IsCurrently()
    {
        //if (transform.Find("PLAYER"))
        if (transform.childCount >= 1)
        {
            currentCharacter = true;
        }
        else
        {
            currentCharacter = false;
        }

        if (!currentCharacter)
        {
            gameObject.SetActive(false);
        }

    }

    public void Movement()
    {
        CharacterController controller = GetComponent<CharacterController>();

        moveDirection = playerScript.stickInput;
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        //transform.position += Vector3.ClampMagnitude(moveDirection, speed) * Time.deltaTime;
        //transform.position = Vector3.SmoothDamp(transform.position, moveDirection, ref velocity,speed);

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        Physics.IgnoreLayerCollision(9, 8, true);
        Physics.IgnoreLayerCollision(9, 10, true);
        Physics.IgnoreLayerCollision(9, 11, true);
    }

    void OnEnable()
    {
        //if (Input.GetButton("Jump"))
        transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        //moveDirection.y = jumpSpeed;
    }

    void Hover()
    {
        frometh = new Vector3(transform.position.x, -.5f, transform.position.z);
        untoeth = new Vector3(transform.position.x, hoverIntensity, transform.position.z);
        transform.position = Vector3.Lerp(frometh, untoeth,
        Mathf.SmoothStep(0f, 1f,
        Mathf.PingPong(Time.time / secondsForOneLength, 1f)));
    }

    void SetSortLayer()
    {
        MeshRenderer meshRend = GetComponent<MeshRenderer>();

        meshRend.sortingLayerName = "Default";
        meshRend.sortingOrder = 1;
    }
}