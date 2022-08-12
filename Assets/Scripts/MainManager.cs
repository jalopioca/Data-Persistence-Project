using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 4;
    public Rigidbody Ball;

    public Text HighScoreText;
    public Text ScoreText;
    public GameObject GameOverText;
    public GameObject SpacebarText;
    
    private bool m_Started = false;
    private int m_Points;
    private HighScores highScores;

    private bool m_GameOver = false;

    public float initialForce = 8f;

    public bool tilesAbove = false;
    private string username = "user";

    private MenuController menuController;
    public GameObject upwardBarrier;
    public GameObject upwardCamera;
    public GameObject downwardBarrier;
    public GameObject downwardCamera;
    public GameObject boxTop;

    // Awake is called prior to start; initialize shared data before other scripts use it
    private void Awake()
    {
        var o = GameObject.Find("MenuManager");
        if (o != null)
        {
            menuController = o.GetComponent<MenuController>();
            tilesAbove = menuController.tilesAbove;
            username = menuController.Username;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        const float xStep = 2.5f;
        const float zStep = 2f;
        int perLine = Mathf.FloorToInt(10.0f / xStep);

        float yStart = (tilesAbove) ? 15f : 0f;
        int[] pointCountArray = (tilesAbove) ? new[] { 1, 1, 2, 2, 5, 5 } : new [] {5,5,2,2,1,1};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                for (int z = 0; z < perLine; z++)
                {
                    Vector3 position = new Vector3(6.25f + xStep * x, yStart + 2.5f + i * 0.3f, -4f + zStep * z);
                    var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                    brick.PointValue = pointCountArray[i];
                    brick.onDestroyed.AddListener(AddPoint);
                }
            }
        }

        if (tilesAbove)
        {
            upwardBarrier.SetActive(true);
            upwardCamera.SetActive(true);
            boxTop.SetActive(true);
            downwardBarrier.SetActive(false);
            downwardCamera.SetActive(false);
        }

        LoadScore();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                SpacebarText.SetActive(false);

                float yDirection = (tilesAbove) ? 1f : -1f;
                Vector3 forceDir = new Vector3(Random.Range(-1.0f, 1.0f), yDirection, Random.Range(-1.0f, 1.0f));
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * initialForce, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        highScores.SetHighScore(username, m_Points);
        SaveScore();
    }

    [System.Serializable]
    public class HighScores
    {
        public static int Length = 3;

        public HighScore Score1;
        public HighScore Score2;
        public HighScore Score3;

        public HighScores()
        {
            Score1 = new HighScore("", 0);
            Score2 = new HighScore("", 0);
            Score3 = new HighScore("", 0);
        }

        public void SetHighScore(string user, int score)
        {
            if (score >= Score1.Score)
            {
                Score3 = Score2;
                Score2 = Score1;
                Score1 = new HighScore(user, score);

            } else if (score >= Score2.Score)
            {
                Score3 = Score2;
                Score2 = new HighScore(user, score);
            } else if (score >= Score3.Score)
            {
                Score3 = new HighScore(user, score);
            }
        }
    }

    [System.Serializable]
    public class HighScore
    {
        public string User;
        public int Score;

        public HighScore(string user, int score)
        {
            User = user;
            Score = score;
        }

    }

    public class SaveData
    {
        public HighScores highScores;
    }

    void LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            highScores = data.highScores;
            if (highScores == null)
            {
                highScores = new HighScores();
            }
        } else
        {
            highScores = new HighScores();
        }

        string newText = "High Scores:\n";
        foreach (var item in new HighScore[]{ highScores.Score1, highScores.Score2, highScores.Score3 })
        {
            if (item.Score == 0)
            {
                continue;
            }
            newText += item.User + ": " + item.Score + "\n";
        }
        HighScoreText.text = newText;
    }

    void SaveScore()
    {
        var s = new SaveData();
        s.highScores = highScores;

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", JsonUtility.ToJson(s));
    }
}
