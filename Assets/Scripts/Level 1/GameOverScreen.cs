using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Proyecto26;
using System;

namespace Level1
{
    public class GameOverScreen : MonoBehaviour
    {

        public TMP_Text pointsText;
        public TMP_Text timeText;
        public TMP_Text messageText;
        private readonly string basePath = "https://rich-teal-crayfish-coat.cyclic.app/level1";

        private readonly string basePath1 = "https://rich-teal-crayfish-coat.cyclic.app/retries";
        private RequestHelper currentRequest;

        public void Setup(int score, float time, int state, string message, bool isGettingSmall)
        {
            Post(score, time, state, isGettingSmall);
            gameObject.SetActive(true);
            pointsText.text = score.ToString() + " Points";
            messageText.text = message;
            if (message == "You Won!")
                messageText.color = Color.green;
            timeText.text = Mathf.Round(time).ToString() + " seconds";
            Time.timeScale = 0;
        }

        public void RestartButton()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Level Selector");
        }

        public void Post(int score, float time, int causeOfDeath, bool isGettingSmall)
        {
            Dictionary<string, string> head = new Dictionary<string, string>();
            head.Add("Content-Type", "application/json");
            head.Add("Access-Control-Allow-Origin", "*");

            currentRequest = new RequestHelper
            {
                Uri = basePath,
                Headers = head,
                Body = new PlayerRequest
                {
                    score = score,
                    time = time,
                    causeOfDeath = causeOfDeath, // 3 is win state
                    isGettingSmall = isGettingSmall //if false then player has taken infinite health
                },
                EnableDebug = true
            };
            RestClient.Post<PlayerRequest>(currentRequest)
            .Then(res =>
            {

                // And later we can clear the default query string params for all requests

                Debug.Log("Success");
            })
            .Catch(err => Debug.Log("Error"));
        }
        public void Post1()
        {
            Dictionary<string, string> head = new Dictionary<string, string>();
            head.Add("Content-Type", "application/json");
            head.Add("Access-Control-Allow-Origin", "*");

            currentRequest = new RequestHelper
            {
                Uri = basePath1,
                Headers = head,
                Body = new PlayerRequest1
                {
                    level = "level1"
                },
                EnableDebug = true
            };
            RestClient.Post<PlayerRequest>(currentRequest)
            .Then(res =>
            {

                // And later we can clear the default query string params for all requests

                Debug.Log("Success");
            })
            .Catch(err => Debug.Log("Error"));
        }

    }

    [Serializable]
    public class PlayerRequest
    {
        public int score;

        public float time;

        public int causeOfDeath;
        public bool isGettingSmall;

        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
    }
    public class PlayerRequest1
    {
        public string level;
        
    }

}
