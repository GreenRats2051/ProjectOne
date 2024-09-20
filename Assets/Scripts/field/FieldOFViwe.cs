using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float fov = 90f; // Угол обзора
    public int rayCount = 50; // Количество лучей (точек)
    public float viewDistance = 20f; // Дальность обзора
    public LayerMask obstacleMask; // Маска препятствий
    private List<Transform> visibleEnemies = new List<Transform>(); // Список видимых врагов
    private Mesh mesh; // Меш для визуализации поля зрения
    private Vector3 origin; // Позиция начала обзора
    private float startingAngle; // Стартовый угол для расчета лучей
    public LayerMask enemyMask;
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero; // Начальная позиция для меша
        startingAngle = 0f; // Начальный угол
    }

    private void LateUpdate()
    {
        GenerateFieldOfView();
        // Обработка видимости врагов
        HandleEnemyVisibility();
    }

    // Функция для генерации поля зрения
    private void GenerateFieldOfView()
    {
        float angle = startingAngle - fov / 2f; // Начальный угол - от левого края угла обзора
        float angleIncrease = fov / rayCount; // Угол между лучами

        // Массив вершин, UV-координаты и индексы для треугольников
        Vector3[] vertices = new Vector3[rayCount + 2]; // Вершины для меша (центр + лучи)
        Vector2[] uv = new Vector2[vertices.Length]; // UV (необязательно)
        int[] triangles = new int[rayCount * 3]; // Треугольники для меша

        vertices[0] = Vector3.zero; // Центральная вершина
        visibleEnemies.Clear();
        int vertexIndex = 1;
        int triangleIndex = 0;

        // Для каждого луча
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            // Рассчитываем направление луча относительно объекта
            Vector3 direction = GetVectorFromAngle(angle);
            direction = transform.TransformDirection(direction); // Учитываем поворот объекта

            RaycastHit hit;
            // Если луч касается препятствия, вершина будет на этой точке
            if (Physics.Raycast(transform.position, direction, out hit, viewDistance, obstacleMask))
            {
                vertex = transform.InverseTransformPoint(hit.point); // Точка столкновения с препятствием
                if (hit.collider.gameObject.layer == 9)
                {
                    visibleEnemies.Add(hit.transform);
                }
            }
            else
            {
                vertex = transform.InverseTransformPoint(transform.position + direction * viewDistance); // Если препятствий нет, луч доходит до конца обзора
            }

            vertices[vertexIndex] = vertex; // Добавляем вершину в массив

            // Если не первая вершина, создаем треугольники
            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0; // Центральная точка
                triangles[triangleIndex + 1] = vertexIndex - 1; // Предыдущая вершина
                triangles[triangleIndex + 2] = vertexIndex; // Текущая вершина

                triangleIndex += 3; // Переходим к следующему треугольнику
            }

            vertexIndex++;
            angle += angleIncrease; // Увеличиваем угол для следующего луча
        }

        // Обновляем меш
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals(); // Пересчитываем нормали для корректного освещения
    }

    // Функция для получения вектора направления из угла
    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad; // Конвертация угла в радианы
        return new Vector3(Mathf.Sin(angleRad), 0, Mathf.Cos(angleRad)); // Рассчитываем направление по углу
    }
    // Метод для управления видимостью врагов
    private void HandleEnemyVisibility()
    {
        // Сначала скрываем всех врагов, которые не в списке видимых
        foreach (Transform enemy in visibleEnemies)
        {
            SkinnedMeshRenderer[] renderers = enemy.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var renderer in renderers)
            {
                if (!renderer.enabled) // Проверяем, если рендер выключен
                {
                    renderer.enabled = true; // Включаем рендер
                }
            }
        }

        // Проходим по всем врагам в сцене, чтобы отключить рендеринг невидимых
        foreach (var enemy in FindObjectsOfType<Transform>())
        {
            if (!visibleEnemies.Contains(enemy))
            {
                SkinnedMeshRenderer[] renderers = enemy.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (var renderer in renderers)
                {
                    if (renderer.enabled) // Проверяем, если рендер включен
                    {
                        renderer.enabled = false; // Выключаем рендер
                    }
                }
            }
        }
    }
    // Установка угла обзора
    public void SetDirection(Vector3 direction)
    {
        startingAngle = GetAngleFromVector(direction);
    }

    // Рассчет угла на основе вектора
    private float GetAngleFromVector(Vector3 direction)
    {
        direction = direction.normalized; // Нормализуем направление
        float n = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; // Рассчитываем угол
        if (n < 0) n += 360; // Приводим угол к диапазону [0, 360]
        return n;
    }
    public List<Transform> GetVisibleEnemies()
    {
        return visibleEnemies;
    }
    // Устанавливаем позицию начала обзора
    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }
}