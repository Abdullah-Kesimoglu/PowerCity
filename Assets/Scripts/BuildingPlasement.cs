using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Building Preset",menuName = "New Building Preset")]

public class BuildingPlasement : ScriptableObject
{
     public int cost;
     public int costPerTurn;
     public int population;
     public int food;
     public int jobs;
     public int energyPerTurn;
     public int spentEnergyPerTurn;
     
     public GameObject prefab;

}
