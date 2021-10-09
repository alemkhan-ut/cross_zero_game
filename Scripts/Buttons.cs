/// Данный скрипт служит для управления кнопками, содержит только методы

using UnityEngine;
using UnityEngine.SceneManagement; // Библиотека необходимая для Метода загрузки сцен

public class Buttons : MonoBehaviour
{

    public void RestartGame() // Перезагружает игру
    {
        SceneManager.LoadScene(1); // В игре к моменту разработки 2 сцены, 0 - Главное меню, 1 - Игровое поле. Данный метод перезапускает/загружает игровое поле
    }

    public void PauseGame()
    {
        // Место для реализации паузы в игре
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1); // В игре к моменту разработки 2 сцены, 0 - Главное меню, 1 - Игровое поле. Данный метод перезапускает/загружает игровое поле
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0); // В игре к моменту разработки 2 сцены, 0 - Главное меню, 1 - Игровое поле. Данный метод перезапускает/загружает главное меню
    }

    /// Методы для установки размерности стола игрового поля

    public void SetSmallTableSize() // Установка маленького размера
    {
        PlayerPrefs.SetInt(Table.TABLE_SIZE_PREFS, 3); // Использование класса PlayerPrefs и метод SetInt() позволяет сохранять и хранить данные типа int для использования в любом другом классе
    }
    public void SetMediumTableSize() // Установка среднего размера
    {
        PlayerPrefs.SetInt(Table.TABLE_SIZE_PREFS, 6); // Использование класса PlayerPrefs и метод SetInt() позволяет сохранять и хранить данные типа int для использования в любом другом классе
    }
    public void SetLargeTableSize() // Установка большого размера
    {
        PlayerPrefs.SetInt(Table.TABLE_SIZE_PREFS, 9); // Использование класса PlayerPrefs и метод SetInt() позволяет сохранять и хранить данные типа int для использования в любом другом классе
    }
}
