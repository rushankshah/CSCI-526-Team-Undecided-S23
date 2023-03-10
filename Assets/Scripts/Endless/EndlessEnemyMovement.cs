using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Endless
{
    public class EndlessEnemyMovement : MonoBehaviour
    {
        public EndlessBallMovement ballmovementscript;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        public GameObject player;
        public float speed = 2.0f;

        void Update()
        {
            if (!ballmovementscript.isBallFrozen)
            {
                Vector3 targetPosition = player.transform.position;
                Vector3 enemyPosition = transform.position;
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(enemyPosition, targetPosition, step);
            }
        }
    }
}

