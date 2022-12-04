using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    
    public enum TargetType { 
        Minion,
        item
    }
    public TargetType targetType;

}
