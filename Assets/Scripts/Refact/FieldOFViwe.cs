using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float fov = 90f; 
    public int rayCount = 50; 
    public float viewDistance = 20f; 
    public LayerMask obstacleMask; 
    public LayerMask enemyLayerMask; 
    private List<Transform> visibleEnemies = new List<Transform>();
    public List<Transform> VisibleEnemies => visibleEnemies;
    private Mesh mesh; 
    private Vector3 origin; 
    private float startingAngle; 
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero; 
        startingAngle = 0f; 
    }

    private void LateUpdate()
    {
        GenerateFieldOfView();
        UpdateInfoOfEnemy();
    }

    private void GenerateFieldOfView()
    {
        float angle = startingAngle - fov / 2f; // Начальный угол - от левого края угла обзора
        float angleIncrease = fov / rayCount; 

        // Массив вершин, UV-координаты и индексы для треугольников
        Vector3[] vertices = new Vector3[rayCount + 2]; // Вершины для меша (центр + лучи)
        Vector2[] uv = new Vector2[vertices.Length]; // UV (необязательно)
        int[] triangles = new int[rayCount * 3]; // Треугольники для меша

        vertices[0] = Vector3.zero; // Центральная вершина
        visibleEnemies.Clear();
        int vertexIndex = 1;
        int triangleIndex = 0;
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
                vertex = transform.InverseTransformPoint(hit.point); 
                if (hit.collider.gameObject.layer == 9)
                {
                    visibleEnemies.Add(hit.transform);
                }
            }
            else
            {
                vertex = transform.InverseTransformPoint(transform.position + direction * viewDistance); 
            }

            vertices[vertexIndex] = vertex; // Добавляем вершину в массив

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
        float angleRad = angle * Mathf.Deg2Rad; 
        return new Vector3(Mathf.Sin(angleRad), 0, Mathf.Cos(angleRad)); // Рассчитываем направление по углу
    }

    private void UpdateInfoOfEnemy()
    {
        foreach(var enemy in visibleEnemies)
        {
            if (enemy == null)
            {
                visibleEnemies.Remove(enemy);
            }
        }
    }


}