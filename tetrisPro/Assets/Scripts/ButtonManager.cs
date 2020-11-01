using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject OptionUI;

    public void OnGoTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void OnGoMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnPlay40Line()
    {
        SceneManager.LoadScene("40Line");
    }

    public void OnOption()
    {
        if (true)
        {
            OptionUI.SetActive(true);
        }
    }

    public void OnQuitGames()
    {
        Application.Quit();
    }
}
