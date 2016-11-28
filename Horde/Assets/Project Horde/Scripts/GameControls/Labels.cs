using UnityEngine;
using System.Collections;

public class Labels : MonoBehaviour {

    public class Tags
    {
        // Controllers
        public static string GameController = "GameController";
        public static string UIController = "UIController";
        public static string ZombieLure = "ZombieLure";
        public static string Beacon = "Beacon";
        public static string Ground = "Ground";
        

        // Mobile Units
        public static string Zombie = "Zombie";
        public static string ZombieScreamer = "ZombieScreamer"; // Abnormal
        public static string ZombieLittleGirl = "ZombieLittleGirl"; // Abnormal
        public static string ZombieDictator = "ZombieDictator"; // Abnormal
        public static string ZombieGlutton = "ZombieGlutton"; // Abnormal
        public static string Human = "Human";
        public static string NoiseVisualisation = "NoiseVisualisation";
        //public static string Player = "Player";

        // UI Elements
        public static string DestructibleIndicator = "DestructibleIndicator";
        public static string EnemyIndicator = "EnemyIndicator";
        public static string PlusZombieIndicator = "PlusZombieIndicator";


        public static bool IsZombie(GameObject other)
        {
            return other.CompareTag(Zombie) 
                || other.CompareTag(ZombieScreamer) 
                || other.CompareTag(ZombieLittleGirl) 
                || other.CompareTag(ZombieDictator) 
                || other.CompareTag(ZombieGlutton);
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
        public static string GuardTower = "GuardTower";
        public static string Barricade = "Barricade";
        public static string Destructible = "Destructible";

        public static bool IsDestructible(GameObject other)
        {
            return other.CompareTag(Barricade) || other.CompareTag(GuardTower) || other.CompareTag(Destructible);
        }

    }

    public class Layers
    {
        public static string Ground = "Ground";
    }

    public class Memory
    {
        public static string CurrentTarget = "CurrentTarget";
        public static string LastUserTap = "LastUserTap";
        public static string LastPriorityNoise = "LastPriorityNoise";
        public static string ClosestEnemy = "ClosestEnemy";
        public static string ClosestCorpse = "ClosestCorpse";
        public static string ClosestDestructible = "ClosestDestructible";
    }

    public class Scenes
    {
        public const string Credits = "Credits";
        public const string MainMenu = "MainMenu";
    }
}
