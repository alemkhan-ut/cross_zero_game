// Данный скрипт управляет настройками стола. Включает в себе паттерн Синглтон

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class Table : MonoBehaviour
{
    public const string TABLE_SIZE_PREFS = "TABLE_SIZE"; // Ссылка на ключ для PlayerPrefs класса, для определения из реестра размерности стола
    private const int DEFAULT_TABLE_SIZE = 3; // базовое значение

    [SerializeField] private int _tableSize; // Размерность стола
    [SerializeField] private int _tableScoreTarget = 3; // Цель которую нужно достичь в очкам
    [SerializeField] private GameObject _tablePrefab;
    [SerializeField] private List<TablePlace> _tablePlaces; // Список мест для фигур

    private GridLayoutGroup _gridLayoutGroup;

    // размеры для клеток под каждую размерноть стола
    private const int SMALL_CELL_SIZE = 100;
    private const int MEDIUM_CELL_SIZE = 150;
    private const int LARGE_CELL_SIZE = 200;

    private int _gridCellSize;

    public static Table Instance; // Синглтон
//
    public List<TablePlace> TablePlaces { get => _tablePlaces; }
    public int TableSize { get => _tableSize; }

    private void Awake()
    {
        Instance = this;

        if (PlayerPrefs.HasKey(TABLE_SIZE_PREFS)) // Проверка если в реестре есть ключ с размерностью стола то,
        {
            _tableSize = PlayerPrefs.GetInt(TABLE_SIZE_PREFS); // Назначаем его в наше поле размерности
        }
        else
        {
            _tableSize = DEFAULT_TABLE_SIZE; // Иначе берём базовое значение
        }

        switch (_tableSize) // Проверяем размерность, и назначает размер каждой клетки под фигуру
        {
            case 3:
                _gridCellSize = LARGE_CELL_SIZE;
                break;
            case 6:
                _gridCellSize = MEDIUM_CELL_SIZE;
                break;
            case 9:
                _gridCellSize = SMALL_CELL_SIZE;
                break;
            default:
                break;
        }

        _gridLayoutGroup = GetComponent<GridLayoutGroup>();

        _gridLayoutGroup.constraintCount = _tableSize; // Обозначает сколько обьектов может расположится по заданной настройке, наша настройка установлена на кол-во обьектов в строке. Что означает 3, 6 или 9 обьект в одной строке
        _gridLayoutGroup.cellSize = new Vector2(_gridCellSize, _gridCellSize); // Установка одной клетки по двум осям (квадрат)

        int _tableTotalSize = _tableSize * _tableSize; // Определение общ. кол-ва клеток

        for (int i = 0; i < _tableTotalSize; i++) // Создание клеток
        {
            GameObject tablePlace = Instantiate(_tablePrefab, transform);
            _tablePlaces.Add(tablePlace.GetComponent<TablePlace>());
        }
    }
    public bool CheckForFullness() // Проверка На заполненность
    {
        foreach (TablePlace place in _tablePlaces) // Проверяем все клетки
        {
            if (!place.IsBusy) // Если хоть одна клетка будет не занята, возвращает false
            {
                return false;
            }
        }

        return true;
    }
}
