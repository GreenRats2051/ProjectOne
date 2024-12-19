using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class AnchorManager : MonoBehaviour
{
    [SerializeField]
    private Transform _playerTransform; 

    [SerializeField]
    private float _anchorRadius = 3f; 

    private List<Transform> _anchors = new List<Transform>(); 
    

    private void OnEnable()
    {
        BoxSelection.SelectedUnits += UpdateAnchors;
    }

    private void OnDisable()
    {
        BoxSelection.SelectedUnits -= UpdateAnchors;
    }

    private void UpdateAnchors(List<SupportBase> supports)
    {
        foreach (var anchor in _anchors)
        {
            Destroy(anchor.gameObject);
        }
        _anchors.Clear();

        
        int count = supports.Count;
        if (count == 0) return;

        float angleStep = 180f / count*2; 
        for (int i = 0; i < count; i++)
        {
            
            float angle = -90f + i * angleStep;
            Vector3 offset = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), 0, Mathf.Sin(Mathf.Deg2Rad * angle)) * _anchorRadius;
            Vector3 anchorPosition = _playerTransform.position + offset;

            
            GameObject anchorObj = new GameObject($"Anchor_{i}");
            anchorObj.transform.position = anchorPosition;
            anchorObj.transform.SetParent(_playerTransform);

            
            _anchors.Add(anchorObj.transform);

            
            supports[i].AssignAnchor(anchorObj.transform);
        }
    }
}
