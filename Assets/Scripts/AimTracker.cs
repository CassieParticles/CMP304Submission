using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public static class AimTracker
{
    private static List<Actor> enemies;

    public static List<int> ShotsFired = new List<int>(5);
    public static List<int> ShotsHit = new List<int>(5);
    public static List<int> ShotsMissed = new List<int>(5);
    public static List<int> ShotsHitTeam = new List<int>(5);

    public static void Init()
    {
        ShotsFired.Clear();
        ShotsHit.Clear();
        ShotsMissed.Clear();
        ShotsHitTeam.Clear();


        for(int i=0;i<5;++i)
        {
            ShotsFired.Add(0);
            ShotsHit.Add(0);
            ShotsMissed.Add(0);
            ShotsHitTeam.Add(0);
        }
    }
    public static void addEnemy(Actor enemy)
    {
        enemies.Add(enemy);
    }

    public static Weapon GetEnumFromWeapon(Gun weapon)
    {
        if (weapon is Pistol)
        {
            return Weapon.Pistol;
        }
        else if (weapon is MachineGun)
        {
            return Weapon.MachineGun;
        }
        else if (weapon is GrenadeThrow)
        {
            return Weapon.GrenadeLauncher;
        }
        else if (weapon is RocketLauncher)
        {
            return Weapon.RocketLauncher;
        }
        return Weapon.None;
    }

    public static Weapon GetEnumFromBullet(Bullet bullet)
    {
        if (bullet is PistolBullet)
        {
            return Weapon.Pistol;
        }
        else if (bullet is MachineGunBullet)
        {
            return Weapon.MachineGun;
        }
        else if (bullet is GrenadeProj)
        {
            return Weapon.GrenadeLauncher;
        }
        else if (bullet is Rocket)
        {
            return Weapon.RocketLauncher;
        }
        return Weapon.None;
    }

    public static void resetValues()
    {
        for(int i=0;i<ShotsFired.Count;++i)
        {
            ShotsFired[i] = 0;
            ShotsHit[i] = 0;
            ShotsMissed[i] = 0;
            ShotsHitTeam[i] = 0;
        }
    }
    public static void RegisterFire(Weapon weapon)
    {
        int index = (int)weapon;
        ShotsFired[index]++;
    }

    public static void RegisterHit(Weapon weapon)
    {
        int index = (int)weapon;
        ShotsHit[index]++;
    }

    public static void RegisterMiss(Weapon weapon)
    {
        int index = (int)weapon;
        ShotsMissed[index]++;
    }

    public static void RegisterFriendlyFire(Weapon weapon)
    {
        int index = (int)weapon;
        ShotsHitTeam[index]++;
    }

    public static void PrintResults()
    {
        for(int i=0;i<5;++i)
        {
            Weapon weapon = (Weapon)i;
            string outputMessage = "Weapon: " + (weapon).ToString() + '\n';
            outputMessage += "Shots fired: " + ShotsFired[i] + '\n';
            outputMessage += "Shots hit: " + ShotsHit[i] + '\n';
            outputMessage += "Shots missed: " + ShotsMissed[i] + '\n';
            outputMessage += "Friendly fire shots: " + ShotsHitTeam[i] + '\n';

            Debug.Log(outputMessage);
        }
    }
}
