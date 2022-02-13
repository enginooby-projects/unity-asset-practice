using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Gaps {
  public class GamePlayManager : MonoBehaviour {
    public static int CURRENT_STAGE;

    public static int currentGamePlayed;

    [HideInInspector] public int currentPercentage;

    public GameObject StageParent;

    public GameObject stageDisplayParent;

    public Slider stageGraph;

    public Image NextStageRectangle;

    public Text CurrentStageNumber;

    public Text NextStageNumber;

    [HideInInspector] public int CurrentScore;

    [HideInInspector] public float percent;

    public Text bestScoreAndLevel;

    public GameObject GameOverPanel;

    [HideInInspector] public bool isStarted;

    public GameObject menuPanel;

    private int _targetScore;

    private int _currentPoints;

    private int _goalPoints = 12;

    private bool _newStage;

    private void Awake() {
      Time.timeScale = 1f;
      CURRENT_STAGE = PlayerPrefs.GetInt("CURRENT_STAGE", 1);
    }

    private void Start() {
      PlayerPrefs.SetInt("CURRENT_STAGE", 1);
      InitScore();
    }

    private void Update() {
      if (Input.GetMouseButton(0) && !isStarted) {
        Vector3 mousePosition = UnityEngine.Input.mousePosition;
        if (mousePosition.y < (float) (Screen.height / 2)) {
          isStarted = true;
          AudioManager.Instance.PlayMusic(AudioManager.Instance.gameMusic);
          AudioManager.Instance.PlaySfx(AudioManager.Instance.buttonClick);
          stageDisplayParent.SetActive(value: true);
          menuPanel.SetActive(value: false);
        }
      }
    }

    private void InitScore() {
      currentPercentage = 0;
      _goalPoints = 20 * CURRENT_STAGE;
      ResetSlider();
    }

    public void CollectLittleBall() {
      if (!_newStage) {
        AddScore(CURRENT_STAGE);
        UpdateGraph();
      }
    }

    private void UpdateGraph() {
      _currentPoints++;
      stageGraph.value = (float) _currentPoints / (float) _goalPoints;
      percent = (float) _currentPoints / (float) _goalPoints * 100f;
      if (_currentPoints >= _goalPoints) {
        _newStage = true;
        StartCoroutine(StageCompleteEffect());
        _currentPoints = 0;
        percent = 0f;
      }
    }

    private IEnumerator StageCompleteEffect() {
      AddStageNumber();
      AudioManager.Instance.PlaySfx(AudioManager.Instance.highscore);
      for (int i = 0; i < 3; i++) {
        ChangeNextStageColor(Color.white, Color.black);
        yield return new WaitForSecondsRealtime(0.08f);
        ChangeNextStageColor(Color.black, Color.white);
        yield return new WaitForSecondsRealtime(0.08f);
      }

      ChangeNextStageColor(Color.white, Color.black);
      yield return new WaitForSecondsRealtime(0.05f);
      GameObject.Find("ColorManager").GetComponent<ColorManager>().ChangeColors();
      StartCoroutine(StageFadeOutCoroutine());
    }

    private void AddScore(int n) => _targetScore += n;

    private IEnumerator StageFadeOutCoroutine() {
      ResetSlider();
      stageGraph.value = (float) _targetScore / (float) _goalPoints;
      _newStage = false;
      yield break;
    }

    private void ChangeNextStageColor(Color rectangleColor, Color textColor) {
      NextStageRectangle.color = rectangleColor;
      NextStageNumber.color = textColor;
    }

    private void AddStageNumber() {
      CURRENT_STAGE++;
      PlayerPrefs.SetInt("CURRENT_STAGE", CURRENT_STAGE);
      if (CURRENT_STAGE > PlayerPrefs.GetInt("BEST_STAGE", 0)) {
        PlayerPrefs.SetInt("BEST_STAGE", CURRENT_STAGE);
      }
    }

    private void ResetSlider() {
      ChangeNextStageColor(Color.black, Color.white);
      CurrentStageNumber.text = CURRENT_STAGE.ToString();
      NextStageNumber.text = (CURRENT_STAGE + 1).ToString();
    }

    public void DeleteStageUI() => stageDisplayParent.SetActive(value: false);

    public void GameOver() => StartCoroutine(GameOverCoroutine());

    private IEnumerator GameOverCoroutine() {
      yield return new WaitForSecondsRealtime(1.5f);
      OpenGameOverPanel();
    }

    public void Restart() => SceneUtils.ReloadScene();

    public void OpenGameOverPanel() {
      DeleteStageUI();
      GameOverPanel.SetActive(value: true);
      stageDisplayParent.SetActive(value: false);
      bestScoreAndLevel.text = PlayerPrefs.GetInt("BEST_STAGE") + string.Empty;
      AudioManager.Instance.PlayMusic(AudioManager.Instance.bgMusic);
    }
  }
}