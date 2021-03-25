using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZoneData", menuName = "ScriptableObjects/ZoneData")]
public class ZoneData : ScriptableObject
{
    [Min(0)] [Tooltip("Le nombre d'entités par lignes")]
        public int EntitiesPerRow;
    [Min(0)] [Tooltip("Le nombre d'entités MAXIMUM par opérations")]
        public int EntitiesPerOperation;
    [Tooltip("Les couleurs que peuvent adopter les entités lors de la génération")]
        public Color[] Colors;
}
