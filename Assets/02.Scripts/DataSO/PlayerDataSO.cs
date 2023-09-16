using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    public int maxHp;

    public int moveSpeed;

    public int atkPower;
}
