﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class HolderObject
{

    [SerializeField, HideInInspector]
    string _name = "HolderObject";
    [SerializeField]
    string _someValue = "";
}