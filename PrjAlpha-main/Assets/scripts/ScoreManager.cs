using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public skeletonSpawner skeletonSpawner;
    public EnemySpawner enemySpawner; 
    public int score = 0;
    public Text scoreText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreUI();

    }

    public void AddScore(int points)
    {

        score += points;
        UpdateScoreUI();
        //spawn skeletons each time score fullfils parameter
        if (score %15==0)
        {
            skeletonSpawner.SpawnSkeleton();
            Debug.Log("Score reached 6, spawning skeletons");
        }
        //increase ghose speed
        if (score % 8 == 0)
        {
            Enemy.speed += 0.5f;
            Debug.Log("score reached" + score + ", increased enemy speed");

        }
        //increase difficulty
        if (score % 10 == 0)
        {
            enemySpawner.IncreaseDifficulty();
            Debug.Log("score reached" + score + ", increased maxEnemies in EnemySpawner");
        }
    }
    public int GetScore()
    {
        return score;
    }
    
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
    /*
    private void SpawnSkeleton()
    {
        if (score%10!=0)
        {
            skeletonSpawner.SpawnSkeleton();
            Debug.Log("Skeleton spawned.");
        }
        
    }
    */
}

