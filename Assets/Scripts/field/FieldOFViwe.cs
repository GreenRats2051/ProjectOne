using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float fov = 90f; // ���� ������
    public int rayCount = 50; // ���������� ����� (�����)
    public float viewDistance = 20f; // ��������� ������
    public LayerMask obstacleMask; // ����� �����������
    public LayerMask enemyLayerMask; // ����� ������ (��������, InvisibleEnemy)
    public LayerMask visibleEnemyLayerMask; // ����� ������� ������ (VisibleEnemy)
    private List<Transform> visibleEnemies = new List<Transform>(); // ������ ������� ������
    private Mesh mesh; // ��� ��� ������������ ���� ������
    private Vector3 origin; // ������� ������ ������
    private float startingAngle; // ��������� ���� ��� ������� �����
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero; // ��������� ������� ��� ����
        startingAngle = 0f; // ��������� ����
    }

    private void LateUpdate()
    {
        GenerateFieldOfView();
        // ��������� ��������� ������
        HandleEnemyVisibility();
    }

    // ������� ��� ��������� ���� ������
    private void GenerateFieldOfView()
    {
        float angle = startingAngle - fov / 2f; // ��������� ���� - �� ������ ���� ���� ������
        float angleIncrease = fov / rayCount; // ���� ����� ������

        // ������ ������, UV-���������� � ������� ��� �������������
        Vector3[] vertices = new Vector3[rayCount + 2]; // ������� ��� ���� (����� + ����)
        Vector2[] uv = new Vector2[vertices.Length]; // UV (�������������)
        int[] triangles = new int[rayCount * 3]; // ������������ ��� ����

        vertices[0] = Vector3.zero; // ����������� �������
        visibleEnemies.Clear();
        int vertexIndex = 1;
        int triangleIndex = 0;

        // ��� ������� ����
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            // ������������ ����������� ���� ������������ �������
            Vector3 direction = GetVectorFromAngle(angle);
            direction = transform.TransformDirection(direction); // ��������� ������� �������

            RaycastHit hit;
            // ���� ��� �������� �����������, ������� ����� �� ���� �����
            if (Physics.Raycast(transform.position, direction, out hit, viewDistance, obstacleMask))
            {
                vertex = transform.InverseTransformPoint(hit.point); // ����� ������������ � ������������
                if (hit.collider.gameObject.layer == 9)
                {
                    visibleEnemies.Add(hit.transform);
                }
            }
            else
            {
                vertex = transform.InverseTransformPoint(transform.position + direction * viewDistance); // ���� ����������� ���, ��� ������� �� ����� ������
            }

            vertices[vertexIndex] = vertex; // ��������� ������� � ������

            // ���� �� ������ �������, ������� ������������
            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0; // ����������� �����
                triangles[triangleIndex + 1] = vertexIndex - 1; // ���������� �������
                triangles[triangleIndex + 2] = vertexIndex; // ������� �������

                triangleIndex += 3; // ��������� � ���������� ������������
            }

            vertexIndex++;
            angle += angleIncrease; // ����������� ���� ��� ���������� ����
        }

        // ��������� ���
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals(); // ������������� ������� ��� ����������� ���������
    }

    // ������� ��� ��������� ������� ����������� �� ����
    private Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad; // ����������� ���� � �������
        return new Vector3(Mathf.Sin(angleRad), 0, Mathf.Cos(angleRad)); // ������������ ����������� �� ����
    }
    // ����� ��� ���������� ���������� ������
    private Dictionary<Transform, bool> enemyVisibilityState = new Dictionary<Transform, bool>();

    private float visibilityCooldown = 0.5f; // �������� � ��������
    private Dictionary<Transform, float> enemyVisibilityTimers = new Dictionary<Transform, float>();

    private void HandleEnemyVisibility()
    {
        visibleEnemies.Clear();

        Collider[] enemiesInView = Physics.OverlapSphere(transform.position, viewDistance, enemyLayerMask);
        float checkRadius = viewDistance + 1f; // ������� ����������� ������
        foreach (Collider enemyCollider in enemiesInView)
        {
            Transform enemyTransform = enemyCollider.transform;
            Vector3 directionToEnemy = (enemyTransform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToEnemy) < fov / 2f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToEnemy, out hit, viewDistance, obstacleMask | enemyLayerMask))
                {
                    bool isVisible = hit.transform == enemyTransform;

                    if (isVisible)
                    {
                        // ���� ���� �����, �������� ��� ���������
                        Renderer[] enemyRenderers = enemyTransform.GetComponentsInChildren<Renderer>();
                        foreach (Renderer renderer in enemyRenderers)
                        {
                            renderer.enabled = true;
                        }
                        visibleEnemies.Add(enemyTransform);
                        enemyVisibilityTimers[enemyTransform] = Time.time + visibilityCooldown; // ������������� ������
                    }
                    else
                    {
                        // ���� ���� ����� � ����� ��������� ��� �� �������
                        if (enemyVisibilityTimers.TryGetValue(enemyTransform, out float endTime) && Time.time < endTime)
                        {
                            // �������� ���������
                            Renderer[] enemyRenderers = enemyTransform.GetComponentsInChildren<Renderer>();
                            foreach (Renderer renderer in enemyRenderers)
                            {
                                renderer.enabled = true;
                            }
                        }
                        else
                        {
                            // ��������� ���������
                            Renderer[] enemyRenderers = enemyTransform.GetComponentsInChildren<Renderer>();
                            foreach (Renderer renderer in enemyRenderers)
                            {
                                renderer.enabled = false;
                            }
                        }
                    }
                }
            }
            else
            {
                // ���� ��� ���� ������, ��������� ��� ���������
                Renderer[] enemyRenderers = enemyTransform.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in enemyRenderers)
                {
                    renderer.enabled = false;
                }
            }
        }
    }
    // ��������� ���� ������
    public void SetDirection(Vector3 direction)
    {
        startingAngle = GetAngleFromVector(direction);
    }

    // ������� ���� �� ������ �������
    private float GetAngleFromVector(Vector3 direction)
    {
        direction = direction.normalized; // ����������� �����������
        float n = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; // ������������ ����
        if (n < 0) n += 360; // �������� ���� � ��������� [0, 360]
        return n;
    }
    public List<Transform> GetVisibleEnemies()
    {
        return visibleEnemies;
    }
    // ������������� ������� ������ ������
    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }
}