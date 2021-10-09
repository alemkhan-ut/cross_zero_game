// Данный скрипт позволяет управлять каждым местом на столе, местом считает клетка на игровом поле куда игрок имеет возможность разместить свою фигуру (крестик, нолик)
// Для отображения фигур, используем буквы "Х" (икс), и "О" (оу)

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))] // Необходимость в компоненте (скрипте), для использования ниже, во избежание ошибок
public class TablePlace : MonoBehaviour
{
    [SerializeField] private TMP_Text _placeTMP_Text; // Ссылка текст на отображение фигур
    [SerializeField] private TableStatus.CommandType _placeCommand; // Ссылка на тип команды текущего места, хранит в себе типа команды данной клетки
    [SerializeField] private bool _isBusy; // Флаг описывающий занят ли он какой либо фигурой
    [Space]
    [SerializeField] private int _horizontalScore; // Очки в горизонтали, кол-во одинаковых фигур подряд
    [SerializeField] private int _verticalScore; // Очки в вертикали, кол-во одинаковых фигур подряд
    [SerializeField] private int _leftDiagonalScore; // Очки в диагонали, кол-во одинаковых фигур подряд
    [SerializeField] private int _rightDiagonalScore; // Очки в диагонали, кол-во одинаковых фигур подряд

    [SerializeField] private List<TablePlace> _horizontalNeighboringPlaces; // Кол-во соседних клеток одинаковой фигуры по горизонтали
    [SerializeField] private List<TablePlace> _verticalNeighboringPlaces; // Кол-во соседних клеток одинаковой фигуры по вертикали
    [SerializeField] private List<TablePlace> _leftDiagonalNeighboringPlaces; // Кол-во соседних клеток одинаковой фигуры по вертикали
    [SerializeField] private List<TablePlace> _rightDiagonalNeighboringPlaces; // Кол-во соседних клеток одинаковой фигуры по вертикали

    private Table _table;
    private TableStatus _tableStatus;

    private Transform _transform;
    private Button _attachedButton;
    private int _index;

    public TableStatus.CommandType PlaceCommand { get => _placeCommand; }
    public bool IsBusy { get => _isBusy; }

    private void Awake()
    {
        _transform = transform;
        _attachedButton = GetComponent<Button>(); // Инициализируем поле ссылкой на компонент Button добавленный на данный обьект
        _index = _transform.GetSiblingIndex(); // Инициализируем поле методом который возвращает порядковый индекс данного обьекта у родителя

        string oldName = name; // Сохраняем предыдущее имя, для установки в имени номера индекса в локальную переменную
        name = "[" + _index + "] " + oldName; // изменяем имя где соединяем его индекс и старое имя

        _attachedButton.onClick.AddListener(OnClick); // Подписываемся на метод OnClick во вложенной кнопке
    }

    private void Start()
    {
        _table = Table.Instance;
        _tableStatus = TableStatus.Instance;

        FindNeighboringPlaces();
    }

    private void FindNeighboringPlaces() // Поиск соседний клеток
    {
        ClearNeighboringPlaces(); // Очистка списков соседей на случай некорректного отображения списка

        if (_index > 0) // Проверка клетки на то что он не первый
        {
            if (_index < _table.TablePlaces.Count - 1) // И если он не первый и не последний то ...
            {
                #region ПОИСК ГОРИЗОНАЛЬНЫХ СОСЕДЕЙ

                if (_table.TablePlaces.Count > (_index + 1)) // Если количество элементов больше чем (индекс + 1 т.е справа), то это значит что индекс существует, и не выходит за области списка
                {
                    if (!_horizontalNeighboringPlaces.Contains(_table.TablePlaces[_index + 1])) // Если данный элемент не находится в списке, то его нужно добавить, исключает возможности добавления одинаковых элементов
                    {
                        _horizontalNeighboringPlaces.Add(_table.TablePlaces[_index + 1]);
                    }
                }
                if ((_index - 1) >= 0) // Если индекс - 1, то есть элемент слева больше или равно 0, то это значит что индекс не выходит за области списка
                {
                    if (!_horizontalNeighboringPlaces.Contains(_table.TablePlaces[_index - 1])) // Проверка если данный элемент не находится в списке, то его нужно добавить
                    {
                        _horizontalNeighboringPlaces.Add(_table.TablePlaces[_index - 1]);
                    }
                }

                #endregion

                #region ПОИСК ВЕРТИКАЛЬНЫХ СОСЕДЕЙ

                if (_table.TablePlaces.Count > (_index + _table.TableSize)) // Если количество элементов в списке больше чем индекс + размерность стола т.е. элемент сверху, то его можно добавить, он не выходит за области списка
                {
                    if (!_verticalNeighboringPlaces.Contains(_table.TablePlaces[_index + _table.TableSize]))
                    {
                        _verticalNeighboringPlaces.Add(_table.TablePlaces[_index + _table.TableSize]);
                    }
                }
                if ((_index - _table.TableSize) >= 0)
                {
                    if (!_verticalNeighboringPlaces.Contains(_table.TablePlaces[_index - _table.TableSize]))
                    {
                        _verticalNeighboringPlaces.Add(_table.TablePlaces[_index - _table.TableSize]);
                    }
                }

                #endregion

                #region ПОИСК ДИАГОНАЛЬНЫХ СОСЕДЕЙ

                if (_table.TablePlaces.Count > (_index + (_table.TableSize - 1))) // Если количество элементов в списке больше чем (индекс + (размерность - 1) т.е элемент с левой нижней стороны), то его можно добавить
                {
                    if (!_rightDiagonalNeighboringPlaces.Contains(_table.TablePlaces[_index + (_table.TableSize - 1)]))
                    {
                        _rightDiagonalNeighboringPlaces.Add(_table.TablePlaces[_index + (_table.TableSize - 1)]);
                    }

                    if (_table.TablePlaces.Count > (_index + (_table.TableSize + 1)))  // Если количество элементов в списке больше чем (индекс + (размерность + 1) т.е элемент с правой нижней стороны), то его можно добавить
                    {
                        if (!_leftDiagonalNeighboringPlaces.Contains(_table.TablePlaces[_index + (_table.TableSize + 1)]))
                        {
                            _leftDiagonalNeighboringPlaces.Add(_table.TablePlaces[_index + (_table.TableSize + 1)]);
                        }
                    }
                }

                if ((_index - (_table.TableSize - 1)) >= 0) // Если (индекс - (размерность стола - 1) больше или равен 0 т.е. элемент с правой верхней стороны), то его можно добавить)
                {
                    if (!_rightDiagonalNeighboringPlaces.Contains(_table.TablePlaces[_index - (_table.TableSize - 1)]))
                    {
                        _rightDiagonalNeighboringPlaces.Add(_table.TablePlaces[_index - (_table.TableSize - 1)]);
                    }

                    if ((_index - (_table.TableSize + 1)) >= 0)// Если (индекс - (размерность стола + 1) больше или равен 0 т.е. элемент с левой верхней стороны), то его можно добавить)
                    {
                        if (!_leftDiagonalNeighboringPlaces.Contains(_table.TablePlaces[_index - (_table.TableSize + 1)]))
                        {
                            _leftDiagonalNeighboringPlaces.Add(_table.TablePlaces[_index - (_table.TableSize + 1)]);
                        }
                    }
                }

                #endregion

                #region РАБОТА С ИСКЛЮЧЕНИЯМИ


                if ((_index + 1) % _table.TableSize == 0) // Если индекс + 1 делением на остаток на Размерность стола получаем 0, то это значит что элемент справой стороны
                {
                    if (_table.TablePlaces.Count > (_index + (_table.TableSize - 1)))
                    {
                        if (_table.TablePlaces.Count > (_index + (_table.TableSize + 1)))
                        {
                            if (_leftDiagonalNeighboringPlaces.Contains(_table.TablePlaces[_index + (_table.TableSize + 1)])) // Удаляем у элементов с правой стороны, диагональный сосед на правой нижней стороне, так как такого иметь не может
                            {
                                _leftDiagonalNeighboringPlaces.Remove(_table.TablePlaces[_index + (_table.TableSize + 1)]);
                            }
                        }
                    }

                    if ((_index - (_table.TableSize - 1)) >= 0)
                    {
                        if (_rightDiagonalNeighboringPlaces.Contains(_table.TablePlaces[_index - (_table.TableSize - 1)])) // Удаляем у элемента правого левого угла, диагональный сосед на правой верхней стороне, так как такого иметь не может
                        {
                            _rightDiagonalNeighboringPlaces.Remove(_table.TablePlaces[_index - (_table.TableSize - 1)]);
                        }
                    }

                    if (_horizontalNeighboringPlaces.Contains(_table.TablePlaces[_index + 1]))
                    {
                        _horizontalNeighboringPlaces.Remove(_table.TablePlaces[_index + 1]);  // Удаляем у элемента, горизонтальный сосед справа, так как такого иметь не может
                    }
                }

                if ((_index) % _table.TableSize == 0) // Если индекс делением на остаток на Размерность стола получаем 0, то это значит что элемент слевой стороны
                {
                    if (_table.TablePlaces.Count > (_index + (_table.TableSize - 1)))
                    {
                        if (_rightDiagonalNeighboringPlaces.Contains(_table.TablePlaces[_index + (_table.TableSize - 1)])) // Удаляем у элементов слевой стороны, диагональный сосед на левой нижней стороне, так как такого иметь не может
                        {
                            _rightDiagonalNeighboringPlaces.Remove(_table.TablePlaces[_index + (_table.TableSize - 1)]);
                        }
                    }

                    if ((_index - (_table.TableSize + 1)) >= 0)
                    {
                        if (_horizontalNeighboringPlaces.Contains(_table.TablePlaces[_index - (_table.TableSize + 1)])) // Удаляем у элемента левого левого угла, диагональный сосед на левой верхней стороне, так как такого иметь не может
                        {
                            _leftDiagonalNeighboringPlaces.Remove(_table.TablePlaces[_index - (_table.TableSize + 1)]);
                        }
                    }

                    if (_horizontalNeighboringPlaces.Contains(_table.TablePlaces[_index - 1]))
                    {
                        _horizontalNeighboringPlaces.Remove(_table.TablePlaces[_index - 1]);  // Удаляем у элемента, горизонтальный сосед слева, так как такого иметь не может
                    }
                }

                #endregion
            }
            else // И если он последний то ...
            {

                if (!_horizontalNeighboringPlaces.Contains(_table.TablePlaces[_index - 1]))
                {
                    _horizontalNeighboringPlaces.Add(_table.TablePlaces[_index - 1]); // У него может быть только один горизональный сосед по индексу ниже (т.е слева)
                }
                if (!_verticalNeighboringPlaces.Contains(_table.TablePlaces[_index - _table.TableSize]))
                {
                    _verticalNeighboringPlaces.Add(_table.TablePlaces[_index - _table.TableSize]); // и только один вертикальный сосед выше (т.е сверху)
                }
                if (!_leftDiagonalNeighboringPlaces.Contains(_table.TablePlaces[_index - (_table.TableSize + 1)]))
                {
                    _leftDiagonalNeighboringPlaces.Add(_table.TablePlaces[_index - (_table.TableSize + 1)]); // и только один диагональный слева сверху
                }
            }
        }
        else // Если клетка оказалась первой, т.е. индекс ниже или равен 0
        {
            if (!_horizontalNeighboringPlaces.Contains(_table.TablePlaces[_index + 1]))
            {
                _horizontalNeighboringPlaces.Add(_table.TablePlaces[_index + 1]); // то может быть только один горизонтальный сосед выше (т.е справа)
            }
            if (!_verticalNeighboringPlaces.Contains(_table.TablePlaces[_index + _table.TableSize]))
            {
                _verticalNeighboringPlaces.Add(_table.TablePlaces[_index + _table.TableSize]); // и только один вертикальный сосед ниже (т.е снизу)
            }
            if (!_leftDiagonalNeighboringPlaces.Contains(_table.TablePlaces[_index + (_table.TableSize + 1)]))
            {
                _leftDiagonalNeighboringPlaces.Add(_table.TablePlaces[_index + (_table.TableSize + 1)]); // и только один диагональный справа снизу
            }
        }
    }
    private void ClearNeighboringPlaces() // Очистка списков соседей
    {
        _horizontalNeighboringPlaces.Clear();
        _verticalNeighboringPlaces.Clear();
        _leftDiagonalNeighboringPlaces.Clear();
        _rightDiagonalNeighboringPlaces.Clear();
    }
    private void OnClick()
    {
        if (_tableStatus.CanPlay) // Если статус стола позволяет играть
        {
            if (!_isBusy) // И данное место не занято
            {
                switch (_tableStatus.GetQueue()) // Проверяем ключи
                {
                    case TableStatus.CommandType.Cross: // И если это крестики
                        _placeCommand = TableStatus.CommandType.Cross; // назначаем данное место под крестики
                        _placeTMP_Text.text = "X"; // Обозначаем фигуру
                        _placeTMP_Text.color = Interfaces.Instance.CrossColor; // Указываем цвет
                        break;
                    case TableStatus.CommandType.Zero: // Аналогично с ноликами
                        _placeCommand = TableStatus.CommandType.Zero;
                        _placeTMP_Text.text = "O";
                        _placeTMP_Text.color = Interfaces.Instance.ZeroColor;
                        break;
                    default:
                        break;
                }

                _isBusy = true; // Указываем что место теперь занято

                #region РАСЧЁТ ГОРИЗОНТАЛЬНЫХ ОЧКОВ

                int horizontalCommandNeihgborn = 0; // подсчитываем кол-во соседей одной фигуры по горизонтали

                foreach (TablePlace horizontalNeighborningPlace in _horizontalNeighboringPlaces)
                {
                    if (horizontalNeighborningPlace._placeCommand == _placeCommand) // Проверка на наличие у соседа одинаковой фигуры
                    {
                        horizontalCommandNeihgborn += 1;

                        horizontalNeighborningPlace.AddHorizontalScore(); // При наличии нужного соседа увеличиваем его очки
                    }
                }

                if (horizontalCommandNeihgborn <= 0) // Если соседей нет, то увеличиваем очки самому себе
                {
                    _horizontalScore = 1;
                }

                #endregion

                #region РАСЧЁТ ВЕРТИКАЛЬНЫХ ОЧКОВ

                int verticalCommandNeihgborn = 0; // подсчитываем кол-во соседей одной фигуры по вертикали

                foreach (TablePlace verticalNeighborningPlace in _verticalNeighboringPlaces)
                {
                    if (verticalNeighborningPlace._placeCommand == _placeCommand) // Проверка на наличие у соседа одинаковой фигуры
                    {
                        verticalCommandNeihgborn += 1;

                        verticalNeighborningPlace.AddVerticalScore();
                    }
                }

                if (verticalCommandNeihgborn <= 0)
                {
                    _verticalScore = 1;
                }

                #endregion

                #region РАСЧЁТ ДИАГОНАЛЬНЫХ ОЧКОВ

                int leftDiagonalNeigborn = 0; // подсчитываем кол-во соседей одной фигуры по диагонали

                foreach (TablePlace leftDiagonalNeighborningPlace in _leftDiagonalNeighboringPlaces)
                {
                    if (leftDiagonalNeighborningPlace._placeCommand == _placeCommand) // Проверка на наличие у соседа одинаковой фигуры
                    {
                        leftDiagonalNeigborn += 1;

                        leftDiagonalNeighborningPlace.AddLeftDiagonalScore();
                    }
                }

                if (leftDiagonalNeigborn <= 0)
                {
                    _leftDiagonalScore = 1;
                }

                int rightDiagonalNeigborn = 0; // подсчитываем кол-во соседей одной фигуры по диагонали

                foreach (TablePlace rightDiagonalNeighborningPlace in _rightDiagonalNeighboringPlaces)
                {
                    if (rightDiagonalNeighborningPlace._placeCommand == _placeCommand) // Проверка на наличие у соседа одинаковой фигуры
                    {
                        rightDiagonalNeigborn += 1;

                        rightDiagonalNeighborningPlace.AddRightDiagonalScore();
                    }
                }

                if (rightDiagonalNeigborn <= 0)
                {
                    _rightDiagonalScore = 1;
                }

                #endregion

                _tableStatus.NextQueue(); // Меняем ход
            }
        }
    }

    public int GetHorizontalScore()
    {
        return _horizontalScore;
    }

    public int GetVerticalScore()
    {
        return _verticalScore;
    }
    public int GetLeftDiagonalScore()
    {
        return _leftDiagonalScore;
    }
    public int GetRightDiagonalScore()
    {
        return _rightDiagonalScore;
    }

    public void AddHorizontalScore()
    {
        foreach (TablePlace horizontalNeighborningPlace in _horizontalNeighboringPlaces) // поиск среди соседей одинаковых фигур если такие имеются, то к их очкам добавляем очки данного элемента + 1, для суммирования очков с текущим элементом
        {
            if (horizontalNeighborningPlace._placeCommand == _placeCommand)
            {
                horizontalNeighborningPlace._horizontalScore += _horizontalScore + 1;
            }
        }
    }

    public void AddVerticalScore()
    {
        foreach (TablePlace verticalNeighborningPlace in _verticalNeighboringPlaces) // поиск среди соседей одинаковых фигур если такие имеются, то к их очкам добавляем очки данного элемента + 1, для суммирования очков с текущим элементом
        {
            if (verticalNeighborningPlace._placeCommand == _placeCommand)
            {
                verticalNeighborningPlace._verticalScore += _verticalScore + 1;
            }
        }
    }
    public void AddLeftDiagonalScore()
    {
        foreach (TablePlace leftDiagonalNeighboringPlace in _leftDiagonalNeighboringPlaces) // поиск среди соседей одинаковых фигур если такие имеются, то к их очкам добавляем очки данного элемента + 1, для суммирования очков с текущим элементом
        {
            if (leftDiagonalNeighboringPlace._placeCommand == _placeCommand)
            {
                leftDiagonalNeighboringPlace._leftDiagonalScore += _leftDiagonalScore + 1;
            }
        }
    }
    public void AddRightDiagonalScore()
    {
        foreach (TablePlace rightDiagonalNeighboringPlace in _rightDiagonalNeighboringPlaces) // поиск среди соседей одинаковых фигур если такие имеются, то к их очкам добавляем очки данного элемента + 1, для суммирования очков с текущим элементом
        {
            if (rightDiagonalNeighboringPlace._placeCommand == _placeCommand)
            {
                rightDiagonalNeighboringPlace._rightDiagonalScore += _rightDiagonalScore + 1;
            }
        }
    }
}
