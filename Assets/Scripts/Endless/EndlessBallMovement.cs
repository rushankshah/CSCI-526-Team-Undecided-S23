using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Endless
{
    public class EndlessBallMovement : MonoBehaviour
    {

        public GameOverScreen gameOverScreen;
        public float speed = 4.0f;

        private float startTime;
        private int score = 0;
        private float elapsedTime;

        private int state = -1;//No state: 0 denotes kill by enemy, 1 denotes size death.
        private Rigidbody2D rigidBody;
        private Vector2 direction;
        // Start is called before the first frame update

        public float freezeDuration = 5.0f;
        public bool isBallFrozen = false;
        public Rigidbody2D enemy;
        private Vector2 originalVelocity;

        public GameObject instructions;

        public displaypoints displaypoints;
        private bool canvasDispalay = false;
        void Start()
        {
            startTime = Time.time;
            rigidBody = GetComponent<Rigidbody2D>();
            direction = Random.insideUnitCircle.normalized;
            enemy = GetComponent<Rigidbody2D>();
            instructions.SetActive(true);
            canvasDispalay = true;
            Time.timeScale = 0;
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
            rigidBody.velocity = rigidBody.velocity.normalized * (speed / (Mathf.Max(size.x, size.y,0.6f)));
        }
        public float timeInterval = 1.0f;
        private float timeCounter = 0.0f;

        private void Update()
        {
            timeCounter += Time.deltaTime;

            if (timeCounter >= timeInterval)
            {
                transform.localScale += new Vector3(-0.05f, -0.05f, 0);
                timeCounter = 0.0f;
            }

            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            Bounds bounds = renderer.bounds;
            Vector2 size = bounds.size;

            rigidBody.velocity = rigidBody.velocity.normalized * (speed / (Mathf.Max(size.x, size.y,0.6f)));

            if (size.x <= 0.3f || size.y <= 0.3f)
            {
                state = 1;//No state: 0 denotes kill by enemy, 1 denotes size death.
                GameOver();
                this.enabled = false;
            }
            displaypoints.display(score);
            // Debug.Log(canvasDispalay);
            if (Input.GetKeyDown(KeyCode.Tab) && canvasDispalay==false)
            {
                instructions.SetActive(true);
                canvasDispalay=true;
                Time.timeScale = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Tab) && canvasDispalay==true)
            {
                instructions.SetActive(false);
                canvasDispalay=false;
                Time.timeScale = 1;
            }


        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("food"))
            {
                Destroy(collision.gameObject);
                transform.localScale += new Vector3(0.15f, 0.15f, 0);
                score += 1;
            }

            if (collision.gameObject.CompareTag("enemy"))
            {
                state = 0;// 0 denotes kill by enemy, 1 denotes size death.
                GameOver();
                this.enabled = false;
            }
            if (collision.gameObject.CompareTag("FreezeFood"))
            {
                Debug.Log("collided");
                Destroy(collision.gameObject);
                // enemy.color=Random.ColorHSV();
                FreezeBall();
            }
        }
        public void GameOver()
        {

            elapsedTime = Time.time - startTime;
            gameOverScreen.Setup(score, elapsedTime, state);
        }
        void FreezeBall()
        {
            Debug.Log("FreezingBall");
            if (!isBallFrozen)
            {
                Debug.Log("Ball freezed");
                isBallFrozen = true;
                originalVelocity = enemy.velocity;
                Debug.Log(originalVelocity);
                enemy.velocity = Vector2.zero;
                Debug.Log("After freeezing" + enemy.velocity);
                Invoke("UnfreezeBall", freezeDuration);
            }
        }

        void UnfreezeBall()
        {
            Debug.Log("UnfreezingBall");
            enemy.velocity = originalVelocity;
            isBallFrozen = false;
        }

    }
}
