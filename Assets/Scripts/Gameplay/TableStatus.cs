// Данный скрипт позволяет отслеживать и контролировать состояние игрового стола. Включает в себе паттерн Singleton

using System;
using System.Collections.Generic;
using UnityEngine;

public class TableStatus : MonoBehaviour
{
    [SerializeField] private CommandType _commandQueue; // Ссылка на созданное перечисление
    [SerializeField] private CommandType _winnerCommand; // Ссылка на созданное перечисление
    [SerializeField] private bool _canPlay; // флаг который определяет возможно ли игра, подразумивает аналог паузы, или возможность остановить взаимодейтвие с игровыми обьектами

    private DateTime _startDateTime = new DateTime();
    private DateTime _endDateTime = new DateTime();

    private int _totalPlayingMatchCount;

    // Ссылки на другие классы 
    private Interfaces _interfaces;
    private Table _table;

    public static TableStatus Instance; // Паттерн Синглтон
    public bool CanPlay { get => _canPlay; } // Открытие доступа для чтения через свойство
    public CommandType WinnerCommand { get => _winnerCommand; } // Открытие доступа для чтения через свойство

    [Serializable]
    public enum CommandType // Перечисление для определения команды
    {
        None, // Базовое значение
        Cross, // Крестики
        Zero // Нолики
    }

    private void Awake()
    {
        Instance = this; // Реализация Синглтона
    }

    private void Start()
    {
        // Инициализация полей через паттерн Синглтон

        _interfaces = Interfaces.Instance;
        _table = Table.Instance;

        _startDateTime = DateTime.Now;

        SetQueue(CommandType.Cross); // Установка очереди на старте крестикам, они ходят первые
    }

    public CommandType GetQueue() // Возвращает тип команды чей сейчас ход
    {
        return _commandQueue;
    }

    public void SetQueue(CommandType type) // Назначает очередь ходьбы, принимает параметр типа команды
    {
        _commandQueue = type;

        _canPlay = true;

        // Тернарный оператор определяет и назначает интерфейсу информацию о том кто сейчас ходит, тип команды
        _interfaces.CommandTypeQueue_Text.text = _commandQueue == CommandType.Cross ? "КРЕСТИКИ" : "НОЛИКИ";
    }

    public void NextQueue() // Переключает очередь
    {
        if (FindWinner() != CommandType.None) // Проверка на победителя, если он не равен Базовому значению, то победитель уже известен
        {
            _canPlay = false; // Останавливаем игру

            OpenResultWindow(false);
        }
        else // Иначе если победитель не найден, то есть базовое значение то,
        if (_table.CheckForFullness()) // Проверяем стол на заполненность, есть ли на нём место для хода
        {
            OpenResultWindow(true);
        }

        _commandQueue = GetQueue() == CommandType.Cross ? CommandType.Zero : CommandType.Cross; // Вне зависимости от условий выше, узнаем тип команды текущей очереди

        _interfaces.CommandTypeQueue_Text.text = _commandQueue == CommandType.Cross ? "КРЕСТИКИ" : "НОЛИКИ"; // Указываем в интерфейсе кто ходит следующий
    }

    private void OpenResultWindow(bool isDraw)
    {
        if (isDraw)
        {
            _interfaces.OpenDrawWindow(); // Если нет, открывает окно с ничьей
        }
        else
        {
            _interfaces.OpenWinnerWindow();
        }

        _endDateTime = DateTime.Now;

        _totalPlayingMatchCount = PlayerPrefs.GetInt(SaveManager.TOTAL_PLAYING_MATCHS, 0);

        _totalPlayingMatchCount++;

        PlayerPrefs.SetInt(SaveManager.TOTAL_PLAYING_MATCHS, _totalPlayingMatchCount);

        MatchData matchData = new MatchData();
        matchData.index = _totalPlayingMatchCount;
        matchData.winnerCommandType = FindWinner();
        matchData.matchStartDateTime = DateFormater.GetFullShortDate(_startDateTime);
        matchData.matchEndDateTime = DateFormater.GetFullShortDate(_endDateTime);
        matchData.matchDuration = DateFormater.GetFormatingDurationTime(_startDateTime, _endDateTime);

        SaveManager.Instance.SaveMatchData(matchData);
    }

    public CommandType FindWinner() // Поиск победителя, возвращает типа команды
    {
        List<TablePlace> _tablePlaces = _table.TablePlaces; // Создание и инициализация локального списка типа TablePlace, значением которого будет являтся список всех мест на столе.

        foreach (TablePlace place in _tablePlaces) // Проход каждого места
        {
            if (place.GetHorizontalScore() >= 3) // Если очков в горизонтали больше 3, то победитель найден. Обьявляется выход из метода
            {
                _winnerCommand = place.PlaceCommand;
                return _winnerCommand;
            }

            if (place.GetVerticalScore() >= 3) // Если очков в вертикали больше 3, то победитель найден. Обьявляется выход из метода
            {
                _winnerCommand = place.PlaceCommand;
                return _winnerCommand;
            }
            if (place.GetLeftDiagonalScore() >= 3) // Если очков в вертикали больше 3, то победитель найден. Обьявляется выход из метода
            {
                _winnerCommand = place.PlaceCommand;
                return _winnerCommand;
            }
            if (place.GetRightDiagonalScore() >= 3) // Если очков в вертикали больше 3, то победитель найден. Обьявляется выход из метода
            {
                _winnerCommand = place.PlaceCommand;
                return _winnerCommand;
            }
        }

        return CommandType.None; // Если выход из метода не был обьявлен означает что победитель не найден, то есть значение типа команды базовое.
    }
}