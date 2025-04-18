using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public int Enemy_Count;
    // public int Boss_Count;
    //public Text PlayerHealth;
    //public Text EnemyCounter;
    //public Text AmmoCount;
    public GameObject Advancement;
  //  public int Level; don't know why this didn't work. The Bools did. 
    public bool Level1;
    public bool Level2;
    public bool Level3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      Advancement.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
 
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (Enemy_Count == 0 && Input.GetKeyDown(KeyCode.E))
            
        {
           if (Level1)
            {
                SceneManager.LoadScene("Level2");
                Advancement.SetActive(true);
            }
          else if (Level2)
            {
                SceneManager.LoadScene("Level3");
                Advancement.SetActive(true);
            }
            else if (Level3)
            {

                SceneManager.LoadScene("LevelB");
                Advancement.SetActive(true);
            }
           else
            {
                SceneManager.LoadScene("Level 1");
                Advancement.SetActive(true);
            }
        }
    }
}
