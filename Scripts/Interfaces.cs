/// Данный скрипт выполняет роль управления всем интерфейсом в игре. Включает в себе паттерн Singleton

using TMPro;
using UnityEngine;

public class Interfaces : MonoBehaviour
{
    [SerializeField] private TMP_Text _queueTMP_Text; // Ссылка обьект с текстом, который отражает в игровом поле надпись очереди
    [SerializeField] private TMP_Text _commandTypeQueue_Text; // Ссылка обьект с текстом, который отражает в игровом поле какой тип команды сейчас в очереди
    // Тип команды будет описан в другом скрипте
    [Space]
    [SerializeField] private GameObject _winnerWindow; // Ссылка обьект всплывающего окна с победителем партии
    [SerializeField] private TMP_Text _winnerTextTMP_Text; // Ссылка текст, на надпись победы
    [SerializeField] private TMP_Text _winnerCommandTMP_Text; // Ссылка текст, на победителя
    [Space]
    [SerializeField] private GameObject _drawWindow; // Ссылка обьект всплывающего окна с ничьей
    [SerializeField] private TMP_Text _drawTextTMP_Text; // Ссылка текст, на надпись нечьи
    [Space]
    [SerializeField] private Color _crossColor; // Установка стандарата цвета для крестиков, и ссылка на него
    [SerializeField] private Color _zeroColor; // Установка стандарата цвета для нолико, и ссылка на него

    public static Interfaces Instance; // Паттерн Синглтон

    public TMP_Text CommandTypeQueue_Text { get => _commandTypeQueue_Text; } // Доступ к чтению 
    public Color CrossColor { get => _crossColor; } // Доступ к чтению 
    public Color ZeroColor { get => _zeroColor; } // Доступ к чтению 

    private void Awake()
    {
        Instance = this; // Реализация Синглтона

        _winnerWindow.SetActive(false); // Отключение окон на случай если они включены
        _drawWindow.SetActive(false); // Отключение окон на случай если они включены

        _queueTMP_Text.text = "СЕЙЧАС ХОДЯТ:"; // Установка текста
    }

    public void OpenWinnerWindow() // Окно победы
    {
        _winnerWindow.SetActive(true);

        _winnerTextTMP_Text.text = "ПОБЕЖДАЮТ:";
        _winnerCommandTMP_Text.text = TableStatus.Instance.WinnerCommand == TableStatus.CommandType.Cross ? "КРЕСТИКИ" : "НОЛИКИ"; 
        // Использование тернарного оператора для быстрой проверки на победителя, если это крестики то указывает имя команды
        _winnerCommandTMP_Text.color = TableStatus.Instance.WinnerCommand == TableStatus.CommandType.Cross ? CrossColor : ZeroColor;
        // Повторное использование оператора для покраски текста в цвет команды
    }

    public void OpenDrawWindow() // Окно ничьи
    {
        _drawWindow.SetActive(true);

        _drawTextTMP_Text.text = "НИЧЬЯ";
    }
}
