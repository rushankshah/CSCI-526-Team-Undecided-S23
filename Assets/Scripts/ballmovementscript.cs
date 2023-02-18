using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ballmovementscript : MonoBehaviour
{

    public GameOverScreen gameOverScreen;
    public float speed = 4.0f;

    public float startTime;
    public int score = 0;
    private float elapsedTime;

    private int state = -1;//No state: 0 denotes kill by enemy, 1 denotes size death, 3 denotes win
    private Rigidbody2D rigidBody;
    private Vector2 direction;
    // Start is called before the first frame update

    public float freezeDuration = 5.0f;
    public bool isBallFrozen = false;
    public Rigidbody2D enemy;
    private Vector2 originalVelocity;

    private bool onTouch = true;
    public GameObject DiminishingWall;
    public int buttonCount = 0;

    public displaypoints displaypoints;

    public GameObject spikePrefab;
    void Start()
    {
        startTime = Time.time;
        rigidBody = GetComponent<Rigidbody2D>();
        direction = Random.insideUnitCircle.normalized;
        enemy = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical);
        rigidBody.velocity = direction * speed;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Bounds bounds = renderer.bounds;
        Vector2 size = bounds.size;
        rigidBody.velocity = rigidBody.velocity.normalized * (speed / (Mathf.Max(size.x, size.y)));
    }
    public float timeInterval = 1.0f;
    private float timeCounter = 0.0f;

    private bool isGettingSmall = true;
    private void Update()
    {
        if(isGettingSmall)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= timeInterval)
            {
                transform.localScale += new Vector3(-0.05f, -0.05f, 0);
                timeCounter = 0.0f;
            }
        }
        

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Bounds bounds = renderer.bounds;
        Vector2 size = bounds.size;

        rigidBody.velocity = rigidBody.velocity.normalized * (speed / (Mathf.Max(size.x, size.y)));

        if (size.x <= 0.3f || size.y <= 0.3f)
        {
            state = 1;//No state: 0 denotes kill by enemy, 1 denotes size death.
            GameOver();
            this.enabled = false;
        }

        displaypoints.display(score);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(score>=2) //Change this to 10
            {
                score -= 10;
                Instantiate(spikePrefab, gameObject.transform.localPosition, Quaternion.identity);
            }
        }

    }

    private void ResetButtonCollision()
    {
        onTouch = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("food"))
        {
            Destroy(collision.gameObject);
            if(isGettingSmall)
            {
                transform.localScale += new Vector3(0.15f, 0.15f, 0);
            }
            score += 1;
        }

        if (collision.gameObject.CompareTag("enemy"))
        {
            state = 0;// 0 denotes kill by enemy, 1 denotes size death.
            GameOver();
            this.enabled = false;
        }
        if(collision.gameObject.CompareTag("FreezeFood"))
        {
            Destroy(collision.gameObject);
            // enemy.color=Random.ColorHSV();
            FreezeBall();
        }
        if (collision.gameObject.CompareTag("Button"))
        {
            if(score>=3)
            {
                if (onTouch == true)
                {
                    buttonCount++;
                    score -= 5;
                    Invoke("ResetButtonCollision", 2f);
                    onTouch = false;
                    DiminishingWall.transform.localScale -= new Vector3(0.1f, 0, 0);
                    Debug.Log("Collided with button");
                    if(buttonCount==5)
                    {
                        Destroy(collision.gameObject);
                    }
                }
            }
        }
        if(collision.gameObject.CompareTag("InfiniteTag"))
        {
            isGettingSmall = false;
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            Destroy(collision.gameObject);
        }
    }
    public void GameOver()
    {

        elapsedTime = Time.time - startTime;
        gameOverScreen.Setup(score, elapsedTime, state);
    }
    void FreezeBall()
    {
        if (!isBallFrozen)
        {
            isBallFrozen = true;
            originalVelocity = enemy.velocity;
            enemy.velocity = Vector2.zero;
            Invoke("UnfreezeBall", freezeDuration);
        }
    }

    void UnfreezeBall()
    {
        enemy.velocity = originalVelocity;
        isBallFrozen = false;
    }

}