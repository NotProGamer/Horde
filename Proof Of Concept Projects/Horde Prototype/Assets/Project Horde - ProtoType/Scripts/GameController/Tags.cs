using UnityEngine;
using System.Collections;

/// <summary>
///  Code Author: Reece Howe
///  Project: Horde
///  Engine: Unity
///  Platform: Mobile and PC
///  Notes: 
///  Status: Complete
/// </summary>

public class Tags : MonoBehaviour {

    public static string Zombie = "Zombie";
    //public static string ZombieZero = "ZombieZero";
    public static string Human = "Human";
    public static string Barricade = "Barricade";
    public static string ZombieLure = "ZombieLure";
    public static string Player = "Player";
    public static string GameController = "GameController";

    public class Layers
    {
        public static string Ground = "Ground";
    }

    public static bool IsZombie(GameObject other)
    {
        return other.CompareTag(Zombie);
    }
    public static bool IsHuman(GameObject other)
    {
        return other.CompareTag(Human);
    }
    public static bool IsDestructible(GameObject other)
    {
        return other.CompareTag(Barricade);
    }
    public static bool CanBeInfected(GameObject other)
    {
        return IsHuman(other);
    }
    public static bool IsDevourable(GameObject other)
    {
        return IsHuman(other);
    }
}
