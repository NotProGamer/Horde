﻿using UnityEngine;
using System.Collections;

public class Labels : MonoBehaviour {

    public class Tags
    {
        // Controllers
        public static string GameController = "GameController";
        public static string ZombieLure = "ZombieLure";

        // Mobile Units
        public static string Zombie = "Zombie";
        public static string Human = "Human";
        //public static string Player = "Player";

        public static bool IsZombie(GameObject other)
        {
            return other.CompareTag(Zombie);
        }
        public static bool IsHuman(GameObject other)
        {
            return other.CompareTag(Human);
        }
        public static bool CanBeInfected(GameObject other)
        {
            return IsHuman(other);
        }
        public static bool IsDevourable(GameObject other)
        {
            return IsHuman(other);
        }

        // Buildings
        //public static string GuardTower = "GuardTower";
        //public static string Barricade = "Barricade";

        //public static bool IsDestructible(GameObject other)
        //{
        //    return other.CompareTag(Barricade) || other.CompareTag(GuardTower);
        //}

    }

    public class Layers
    {
        public static string Ground = "Ground";
    }

}