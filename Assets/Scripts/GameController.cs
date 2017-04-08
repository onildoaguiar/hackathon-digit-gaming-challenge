using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject keyStage;
    public GameObject magicSword;
    public GameObject keyStageInfo;
    public GameObject LevelInfo;

    // Use this for initialization
    void Start () {
        keyStageInfo.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GetKeyStage()
    {
        keyStageInfo.SetActive(true);
        keyStage.SetActive(false);
    }

    public void GetMagicSword()
    {
        magicSword.SetActive(false);
    }

    public void FinishDoor( )
    {
        switch (LevelInfo.tag)
        {
            case "Level1":
                SceneManager.LoadScene("Level2");
                break;
            case "Level2":
                SceneManager.LoadScene("Level3");
                break;
            case "Level3":
                SceneManager.LoadScene("Home");
                break;
            default:
                break;
        }

    }

    public void Home()
    {
        SceneManager.LoadScene("Home");
    }
}
