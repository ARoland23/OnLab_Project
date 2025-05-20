using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] UIDocument document;
    private Button playBt;
    private Button difficultyBt;
    private Button exitBt;
    private VisualElement settingsContainer;
    private Button okBt;

    private void Awake()
    {
        playBt = document.rootVisualElement.Q<Button>("PlayBt");
        difficultyBt = document.rootVisualElement.Q<Button>("DifficultyBt");
        exitBt = document.rootVisualElement.Q<Button>("ExitBt");
        settingsContainer = document.rootVisualElement.Q<VisualElement>("SettingsContainer");
        okBt = document.rootVisualElement.Q<Button>("OkBt");

        playBt.clickable.clicked += ()=> { SceneManager.LoadScene("GameScene"); };
        exitBt.clickable.clicked += ()=> { Debug.Log("Exiting game!"); Application.Quit(); };
        difficultyBt.clickable.clicked += () => { settingsContainer.visible = true; };
        okBt.clickable.clicked += ApplySettings;
    }

    private void ApplySettings()
    {
        RadioButton easyButton = document.rootVisualElement.Q<RadioButton>("RadioButtonEasy");
        RadioButton mediumButton = document.rootVisualElement.Q<RadioButton>("RadioButtonMedium");
        RadioButton hardButton = document.rootVisualElement.Q<RadioButton>("RadioButtonHard");
        if (easyButton == null || mediumButton == null || hardButton == null)
            return;

        if (easyButton.value == true)
        {
            GameLogic.difficulty = GameLogic.Difficulty.Easy;
            GameLogic.enemyCount = 2;
        }
        else if (mediumButton.value == true)
        {
            GameLogic.difficulty = GameLogic.Difficulty.Medium;
            GameLogic.enemyCount = 4;
        }
        else if (hardButton.value == true)
        {
            GameLogic.difficulty = GameLogic.Difficulty.Hard;
            GameLogic.enemyCount = 5;
        }

        settingsContainer.visible = false;
        Debug.Log("Difficulty is: " +  GameLogic.difficulty);
    }
}
