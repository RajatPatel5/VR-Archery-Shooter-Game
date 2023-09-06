using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GameOverCanvas : BaseScreen
{    
    public Button MainMenuButton;
    public Button RestartGameButton;
    public TMP_Text yourScore;
    public TMP_Text HighScoreTillNow;
    [SerializeField] private BowController bow;

    //private void Awake()
    //{
    //    inst = this;
    //}

    private void Start()
    {
        MainMenuButton.onClick.AddListener(BackHomeScreen);
        RestartGameButton.onClick.AddListener(RestartGame);
    }
    
    void BackHomeScreen()
    {
        ScreenManager.instance.ShowNextScreen(ScreenType.HomeScreen);
        //SaveManager.instance.CrystalsaveData();
    }

    public void RestartGame()
    {
        ScreenManager.instance.ShowNextScreen(ScreenType.GamePlayCanvas);
        bow.SpwanNewArrow();
        Debug.Log("is Working And Reset");
        ScoreManager.instance.LoadHighScore(ScreenManager.instance.screens[1].GetComponent<GamePlayScreen>());
    }
}
