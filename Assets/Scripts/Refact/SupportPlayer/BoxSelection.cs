using UnityEngine;
using System.Collections.Generic;
using System;

public class BoxSelection : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; 
    [SerializeField] private LayerMask selectableLayer; 
    [SerializeField] private GameObject selectableBox; 

    private Vector3 startMousePosition;
    private Vector3 endMousePosition;
    private static List<SupportBase> selectedUnits = new List<SupportBase>();
    public static Action<List<SupportBase>> SelectedUnits;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(1))
        {
            StartSelection();
        }

        if (Input.GetMouseButton(1))
        {
            UpdateSelection();
        }

        if (Input.GetMouseButtonUp(1))
        {
            EndSelection();
        }
    }

    private void StartSelection()
    {
        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity))
        {
            startMousePosition = hitInfo.point;
        }
        selectableBox.SetActive(true);
    }

    private void UpdateSelection()
    {
        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity))
        {
            endMousePosition = hitInfo.point;
            Vector3 minBounds = new Vector3(
     Mathf.Min(startMousePosition.x, endMousePosition.x),
     Mathf.Min(startMousePosition.y + Vector3.up.y * 2, endMousePosition.y),
     Mathf.Min(startMousePosition.z, endMousePosition.z)
 );

            Vector3 maxBounds = new Vector3(
                Mathf.Max(startMousePosition.x, endMousePosition.x),
                Mathf.Max(startMousePosition.y + Vector3.up.y * 2, endMousePosition.y),
                Mathf.Max(startMousePosition.z, endMousePosition.z)
            );
            selectableBox.transform.position = (minBounds + maxBounds) / 2;
            selectableBox.transform.localScale = maxBounds - minBounds;
        }
    }

    private void EndSelection()
    {
        selectedUnits.Clear();

        Vector3 minBounds = new Vector3(
            Mathf.Min(startMousePosition.x, endMousePosition.x),
            Mathf.Min(startMousePosition.y+Vector3.up.y * 2, endMousePosition.y),
            Mathf.Min(startMousePosition.z, endMousePosition.z)
        );

        Vector3 maxBounds = new Vector3(
            Mathf.Max(startMousePosition.x, endMousePosition.x),
            Mathf.Max(startMousePosition.y+Vector3.up.y*2, endMousePosition.y),
            Mathf.Max(startMousePosition.z, endMousePosition.z)
        );

        Collider[] colliders = Physics.OverlapBox(
            (minBounds + maxBounds) / 2, 
            (maxBounds - minBounds) / 2, 
            Quaternion.identity, 
            selectableLayer 
        );

        foreach (Collider col in colliders)
        {
            if (col.gameObject.layer == 10)
            {
                SupportBase supportBase = col.GetComponent<SupportBase>();
                if (supportBase != null)
                {

                    selectedUnits.Add(supportBase);
                }
            }
        }
        Debug.Log(selectedUnits.Count);
        selectableBox.transform.localScale = new Vector3(0, 0, 0);
        selectableBox.SetActive(false);
        SelectedUnits?.Invoke(selectedUnits);
    }

    private void OnDrawGizmos()
    {
        if (startMousePosition != Vector3.zero && endMousePosition != Vector3.zero)
        {
            Vector3 minBounds = new Vector3(
             Mathf.Min(startMousePosition.x, endMousePosition.x),
             Mathf.Min(startMousePosition.y + Vector3.up.y * 2, endMousePosition.y),
             Mathf.Min(startMousePosition.z, endMousePosition.z)
         );

            Vector3 maxBounds = new Vector3(
                Mathf.Max(startMousePosition.x, endMousePosition.x),
                Mathf.Max(startMousePosition.y + Vector3.up.y * 2, endMousePosition.y),
                Mathf.Max(startMousePosition.z, endMousePosition.z)
            );

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(
                (minBounds + maxBounds) / 2, 
                maxBounds - minBounds 
            );
        }
    }
}
