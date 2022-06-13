using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interface : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject restartButton;
    [SerializeField]
    private TextMeshProUGUI resultText;

    public delegate void StartButtonClickHandle();
    public static event StartButtonClickHandle StartButtonClickEvent;

    //Подпись и отпись от ивентов.
    private void OnEnable()
    {
        BattleController.EndBattleEvent += OnEndBattleEvent;
    }

    private void OnDisable()
    {
        BattleController.EndBattleEvent -= OnEndBattleEvent;
    }

    private void Start()
    {
        restartButton.SetActive(false);
        resultText.enabled = false;
    }

    //Обработки кнопок.
    public void OnStartButtonClick()
    {
        StartButtonClickEvent?.Invoke();
        startButton.SetActive(false);
    }

    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene(0);
    }

    //При получении ивента о завершении битвы выводится текст с результатом и показывается кнопка рестарта.
    private void OnEndBattleEvent(string result)
    {
        resultText.enabled = true;
        resultText.text = result;
        restartButton.SetActive(true);
    }
}