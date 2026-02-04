using System;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthCreator : MonoBehaviour
{
    [SerializeField] private int _gridW;
    [SerializeField] private int _gridH;

    [SerializeField] private Transform _gridPosition;

    [SerializeField] private Grid _grid;
    public void GenerateLabyrinth()
    {
        _grid.GenerateGrid(_gridW,_gridH,_gridPosition.position);
    }
}
