using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 direction;
    public float speed = 2.0f;
    public float jumpStrength = 2.0f;
    bool canClimb = false;
    bool canJump = true;
    GameObject platformCollider;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //set direction to up if space is pressed, else set direction to gravity
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            direction = Vector2.up * jumpStrength;
            canJump = false;
            StartCoroutine(JumpReset());
        }
        else
        {
            direction += Physics2D.gravity * Time.deltaTime;
        }


        direction.x = Input.GetAxis("Horizontal") * speed;
        if (canClimb)
            direction.y = Input.GetAxis("Vertical") * speed;
        //player should not go below -1
        direction.y = Mathf.Max(direction.y, -1f);

        //change rotation based on direction
        if (direction.x > 0)
            transform.eulerAngles = Vector3.zero;
        else if (direction.x < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
    }

    private void FixedUpdate()
    {
        //using fixed update for natural movement feel
        //MovePosition manually moves a rigidbody
        rb.MovePosition(rb.position + direction * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
            canClimb = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
            canClimb = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") && collision.gameObject.transform.position.y > transform.position.y)
        {
            platformCollider = collision.gameObject;
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), collision.collider, true);
            Debug.Log("platform is above");
        }
        else if (collision.gameObject.CompareTag("Barrel"))
        {
            gameManager.ReduceLife();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            gameManager.ReduceLife();
        }
        else if (collision.gameObject.CompareTag("Princess"))
            gameManager.GameWin();

        StartCoroutine(WaitForIgnoreCollisionReset());
    }

    //reset ignore collision on platform
    IEnumerator WaitForIgnoreCollisionReset()
    {
        yield return new WaitForSeconds(1);
        if (platformCollider != null)
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), platformCollider.gameObject.GetComponent<Collider2D>(), false);
    }

    //stop jump spam
    IEnumerator JumpReset()
    {
        yield return new WaitForSeconds(2);
        canJump = true;
    }

}
