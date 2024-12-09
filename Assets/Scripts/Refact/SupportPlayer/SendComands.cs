using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour
{
    public RectTransform selectionBox; // UI объект для визуализации рамки выбора
    public LayerMask selectableLayer; // Слой для выделяемых объектов
    public Camera mainCamera; // Основная камера для Raycast

    private Vector2 startPosition; // Начальная позиция мыши
    private Vector2 endPosition; // Конечная позиция мыши
    private bool isSelecting = false; // Проверка, идет ли сейчас выделение
    [SerializeField]
    private LayerMask LayerMask;
    private List<GameObject> selectedObjects = new List<GameObject>(); // Массив выделенных объектов

    void Update()
    {
        // Начало выделения по клику левой кнопкой мыши
        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            startPosition = Input.mousePosition; // Запоминаем начальную позицию
            selectionBox.gameObject.SetActive(true); // Показываем рамку выбора
        }

        // Если происходит выделение, обновляем конечную позицию
        if (isSelecting)
        {
            endPosition = Input.mousePosition; // Обновляем конечную позицию
            UpdateSelectionBox(); // Обновляем рамку выбора
        }

        // Окончание выделения по отпусканию левой кнопки мыши
        if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;
            selectionBox.gameObject.SetActive(false); // Скрываем рамку выбора
            SelectObjectsInRectangle(); // Выделяем объекты внутри рамки
        }
    }

    // Метод для обновления размера рамки выбора
    void UpdateSelectionBox()
    {
        Vector2 boxStart = startPosition;
        Vector2 boxSize = endPosition - startPosition;

        // Проверяем направление рамки (положительное или отрицательное)
        if (boxSize.x < 0)
        {
            boxStart.x += boxSize.x;
            boxSize.x = -boxSize.x;
        }

        if (boxSize.y < 0)
        {
            boxStart.y += boxSize.y;
            boxSize.y = -boxSize.y;
        }

        selectionBox.anchoredPosition = boxStart; // Начальная позиция рамки
        selectionBox.sizeDelta = boxSize; // Размер рамки
    }

    // Метод для проверки объектов внутри рамки
    void SelectObjectsInRectangle()
    {
        // Очищаем массив от предыдущих выделенных объектов
        selectedObjects.Clear();

        // Преобразуем координаты мыши в мировые координаты
        Vector3[] corners = new Vector3[4];
        selectionBox.GetWorldCorners(corners);

        // Получаем углы рамки в мировых координатах
        Vector2 bottomLeft = corners[0];
        Vector2 topRight = corners[2];



        //foreach (GameObject obj in selectableObjects)
        //{
        //     Преобразуем позицию объекта в экранные координаты
        //    Vector3 screenPos = mainCamera.WorldToScreenPoint(obj.transform.position);

        //     Проверяем, находится ли объект внутри рамки
        //    if (screenPos.x >= bottomLeft.x && screenPos.x <= topRight.x && screenPos.y >= bottomLeft.y && screenPos.y <= topRight.y)
        //    {
        //        selectedObjects.Add(obj); // Добавляем объект в список выделенных
        //        HighlightObject(obj, true); // Визуально выделяем объект (например, изменяя цвет)
        //    }
        //    else
        //    {
        //        HighlightObject(obj, false); // Снимаем выделение, если объект не внутри рамки
        //    }
        //}

        // Вывести количество выделенных объектов
        Debug.Log("Количество выделенных объектов: " + selectedObjects.Count);
    }

    // Визуальное выделение объектов
    void HighlightObject(GameObject obj, bool highlight)
    {
        Renderer objRenderer = obj.GetComponent<Renderer>();
        if (objRenderer != null)
        {
            objRenderer.material.color = highlight ? Color.green : Color.white; // Изменяем цвет объекта
        }
    }
}

