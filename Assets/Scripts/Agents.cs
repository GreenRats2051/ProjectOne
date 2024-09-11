using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agents : MonoBehaviour
{
    public float random_point_radius = 10f; // ������ ��������� ����� ����� � ������� � ����� ��������� �� ������ �� ����, ��� ������� ��� ����� ����� ����� ������
    Vector3 random_point;

    public Transform player;

    public GameObject near_player_prefab; // ��� ������������ ��������� �����

    Transform near_player;
    Transform my_transform;
    Transform target;
    NavMeshAgent agent;
    NavMeshPath nav_mesh_path; // ���� �� ���� �� NavMesh

    public bool draw_radius = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        my_transform = GetComponent<Transform>();
        nav_mesh_path = new NavMeshPath();
        target = player;
        near_player = Instantiate(near_player_prefab, Vector3.zero, Quaternion.identity).transform;
        StartCoroutine(Move_COR());
    }

    IEnumerator Move_COR()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (Vector3.Distance(my_transform.position, player.position) > random_point_radius) // ���� ���������� �� ���� �� ������ ������ ���������
            {
                if (Vector3.Distance(near_player.position, player.position) > random_point_radius) // ���� ���������� �� ��������� ����� ����� ������ �� ������ ������ ���������
                {
                    near_player.gameObject.SetActive(true); // ������������� ��������� �����
                    Go_to_near_random_point();
                }
            }
            else
            {
                near_player.gameObject.SetActive(false);
                target = player;
                agent.SetDestination(target.position); // ���� � ������
            }
        }
    }

    void Go_to_near_random_point() // ��������� ��������� ����� �� NavMesh � ��������� ������������
    {
        bool get_correct_point = false; // ��������������� �� ���������� ����� �� NavMesh
        while (!get_correct_point)
        {
            NavMeshHit navmesh_hit;
            NavMesh.SamplePosition(Random.insideUnitSphere * random_point_radius + player.position, out navmesh_hit, random_point_radius, NavMesh.AllAreas);
            random_point = navmesh_hit.position;

            // �������� �� ������������ �����
            if (random_point.y > -10000 && random_point.y < 10000) // �������� �� ����������� ��������, ��� �������� ������
            {
                agent.CalculatePath(random_point, nav_mesh_path);
                if (nav_mesh_path.status == NavMeshPathStatus.PathComplete &&
                    !NavMesh.Raycast(player.position, random_point, out navmesh_hit, NavMesh.AllAreas))
                {
                    get_correct_point = true; // ���� ���� ���������� � ����� ������� � ��������� ������ ��� �����������
                }
            }
        }

        near_player.position = random_point;
        target = near_player;
    }

    void FixedUpdate()
    {
        Debug.DrawLine(my_transform.position, target.position, Color.yellow); // ���������� ����� ����� ������� � �����
    }

    void OnDrawGizmos() // ������������ �������
    {
        if (draw_radius)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, random_point_radius);
        }
    }
}