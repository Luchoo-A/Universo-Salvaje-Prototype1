using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Nombre de la escena de juego")]
    [SerializeField] private string gameplaySceneName = "Gameplay";

    // Llamado desde el botón Start
    public void OnStartGame()
    {
        Debug.Log("▶ Iniciando juego...");
        SceneManager.LoadScene(gameplaySceneName);
    }

    // Llamado desde el botón Options
    public void OnOpenOptions()
    {
        Debug.Log("⚙ Abriendo opciones...");
        // Aquí podrías mostrar un panel de UI con opciones (volumen, resolución, etc.)
    }

    // Llamado desde el botón Exit
    public void OnExitGame()
    {
        Debug.Log("🚪 Saliendo del juego...");
        Application.Quit();

        // Para pruebas en el editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
