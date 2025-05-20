using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] UIDocument document;
    private Button retryBt;
    private Button menuBt;
    private Label pointsLb;

    private void Awake()
    {
        Debug.Log("Game over scene loaded");
        pointsLb = document.rootVisualElement.Q<Label>("PointsLb");
        pointsLb.text = GameLogic.Score.ToString();

        retryBt = document.rootVisualElement.Q<Button>("RetryBt");
        menuBt = document.rootVisualElement.Q<Button>("MenuBt");
        menuBt.clickable.clicked += () => { SceneManager.LoadScene("MainMenuScene"); };
        retryBt.clickable.clicked += Replay;
    }

    private void Replay()
    {
        if(GameLogic.enemyCount > 0)    // maradt még enemy, player halt meg, döntetlennél player nyer
            GameLogic.SetScore(0);

        GameLogic.ResetEnemies();
        SceneManager.LoadScene("GameScene");
    }
}
