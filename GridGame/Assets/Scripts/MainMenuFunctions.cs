using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuFunctions : MonoBehaviour
{
    public void LoadScene(string name) { SceneManager.LoadScene(name); }
    public void Quit() { Application.Quit(); }
}
