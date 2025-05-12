using System;
using Game.Durak.Enums;
using UnityEngine;
using Utils;

[Serializable]
public struct EnemyPosition
{
    [field: SerializeField]
    public EDirection2D Direction
    {
        get;
        private set;
    }

    [field: SerializeField]
    public EFlagPosition FlagPosition
    {
        get;
        private set;
    }
            
    [field: SerializeField]
    public Transform Transform
    {
        get;
        private set;
    }
}