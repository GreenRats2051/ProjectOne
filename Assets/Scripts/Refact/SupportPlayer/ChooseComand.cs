using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Unity.VisualScripting;

public class ChooseComand : MonoBehaviour
{

    [SerializeField] private CurcleComands[] curcleComands;
    [SerializeField] private GameObject curcle;
    [SerializeField] private List<SupportBase> supports = new List<SupportBase>();

    private bool _behindCheck;
    private bool _wasLeft;
    private void OnEnable()
    {
        BoxSelection.SelectedUnits += GetList;
    }
    private void OnDisable()
    {
        BoxSelection.SelectedUnits -= GetList;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            curcle.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            ExecuteCommands();
            ResetStats();
            curcle.SetActive(false);
        }
    }
    void GetList(List<SupportBase> supportGet)
    {
        supports = supportGet;
    }
    private void ExecuteCommands()
    {
        foreach (var command in curcleComands)
        {
            if (command.istrue)
            {
                switch (command.comand)
                {
                    case Comand.Checkbehind:
                        _behindCheck = true;
                        break;
                    case Comand.Leftbehind:
                        _wasLeft = true;
                        break;
                    case Comand.TakeBack:
                        _wasLeft = false;
                        break;
                    case Comand.Forward:
                        _behindCheck = false;
                        break;
                    default:
                        continue;
                }
                GiveComand();
            }
        }

        foreach (var command in curcleComands)
        {
            command.Reset();
        }
    }

    private void GiveComand()
    {
        foreach (var support in supports)
        {
            support.SetOrder(_wasLeft, _behindCheck);
        }
        Debug.Log(_wasLeft);
        Debug.Log("left");
        Debug.Log(_behindCheck);
        Debug.Log("behind");
        //ResetStats();
    }

    private void ResetStats()
    {
        _wasLeft = false;
        _behindCheck = false;
    }
}