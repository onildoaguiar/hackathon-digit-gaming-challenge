using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MyGamesGooglePlay()
    {
        Application.OpenURL("https://play.google.com/store/apps/dev?id=7756438245277265206");
    }

    public void MyGamesAppStore()
    {
        Application.OpenURL("https://itunes.apple.com/us/developer/onildo-aguiar/id1091865120");
    }

    public void SourceCode()
    {
        Application.OpenURL("https://github.com/onildoaguiar/VanhackathonDigitGamingChallenge");
    }

    public void StartDemo()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OpenLevel(GameObject button)
    {
        switch (button.tag)
        {
            case "Level1":
                SceneManager.LoadScene("Level1");
                break;
            case "Level2":
                SceneManager.LoadScene("Level2");
                break;
            case "Level3":
                SceneManager.LoadScene("Level3");
                break;
            default:
                break;
        }
    }


}
