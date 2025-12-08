using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple launch menu hook to load the gameplay scene (with level generation on start).
/// Wire the Start Game button's OnClick to MainMenu.StartGame.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "JeuCollecte";

    /// <summary>
    /// Called by the UI button to enter the game.
    /// </summary>
    public void StartGame()
    {
        if (string.IsNullOrEmpty(gameplaySceneName))
        {
            Debug.LogError("MainMenu: gameplaySceneName is not set.");
            return;
        }

        SceneManager.LoadScene(gameplaySceneName, LoadSceneMode.Single);
    }

    /// <summary>
    /// Optional exit hook for desktop builds.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
