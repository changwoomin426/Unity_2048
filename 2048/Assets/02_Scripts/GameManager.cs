using System.Collections;
using TMPro;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    [SerializeField] private TileBoard _board;
    [SerializeField] private CanvasGroup _gameOver;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _hiscoreText;
    public int Score { get; private set; } = 0;

    private void Awake() {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnDestroy() {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start() {
        NewGame();
    }

    public void NewGame() {
        SetScore(0);

        _hiscoreText.text = LoadHiscore().ToString();

        _gameOver.alpha = 0f;
        _gameOver.interactable = false;

        _board.ClearBoard();
        _board.CreateTile();
        _board.CreateTile();
        _board.enabled = true;
    }

    public void GameOver() {
        _board.enabled = false;
        _gameOver.interactable = true;

        StartCoroutine(Fade(_gameOver, 1f, 1f));
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay = 0f) {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration) {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    public void IncreaseScore(int points) {
        SetScore(Score + points);
    }

    private void SetScore(int score) {
        Score = score;
        _scoreText.text = Score.ToString();
        SaveHiscore();
    }

    private void SaveHiscore() {
        int hiscore = LoadHiscore();

        if (Score > hiscore) {
            PlayerPrefs.SetInt("hiscore", Score);
        }
    }

    private int LoadHiscore() {
        return PlayerPrefs.GetInt("hiscore", 0);
    }
}
