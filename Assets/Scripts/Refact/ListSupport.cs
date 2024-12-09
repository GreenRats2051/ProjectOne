using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListSupport : MonoBehaviour
{
    public List<GameObject> _supports { get; private set; } = new List<GameObject>();
    private static ListSupport inst;
    public static ListSupport Inst
    {
        get
        {
            if (inst == null)
            {
                inst = new ListSupport();
            }

            return inst;
        }
    }
    public void addSupport(GameObject support)
    {
        _supports.Add(support);
    }
    public void removeSupport(GameObject support)
    {
        _supports.Remove(support);
    }
}
