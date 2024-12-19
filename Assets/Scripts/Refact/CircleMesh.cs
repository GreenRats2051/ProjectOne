    using UnityEngine;

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class CircleMesh : MonoBehaviour
    {
        public float radius = 1f; // Радиус круга
        public int segments = 36; // Количество сегментов (точек на окружности)

        void Start()
        {
            CreateCircleMesh();
        }

        void CreateCircleMesh()
        {
            // Создание меша
            Mesh mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;

            // Вершины: центр + окружность
            Vector3[] vertices = new Vector3[segments + 1];
            vertices[0] = Vector3.zero; // Центр
            for (int i = 0; i < segments; i++)
            {
                float angle = i * Mathf.PI * 2f / segments;
                vertices[i + 1] = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
            }

            // Треугольники
            int[] triangles = new int[segments * 3];
            for (int i = 0; i < segments; i++)
            {
                triangles[i * 3] = 0;                  
                triangles[i * 3 + 1] = (i + 1) % segments + 1; 
                triangles[i * 3 + 2] = i + 1;         

            }

            // UV координаты
            Vector2[] uv = new Vector2[vertices.Length];
            uv[0] = new Vector2(0.5f, 0.5f);
            for (int i = 0; i < segments; i++)
            {
                float x = (vertices[i + 1].x / radius + 1) * 0.5f;
                float y = (vertices[i + 1].z / radius + 1) * 0.5f;
                uv[i + 1] = new Vector2(x, y);
            }

            // Обновление данных меша
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            // Проверка материала
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if (renderer.material == null)
            {
                renderer.material = new Material(Shader.Find("Standard"));
                renderer.material.color = Color.white; // Устанавливаем базовый цвет
            }
        }
    }
