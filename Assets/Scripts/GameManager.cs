using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerControllerPrefab;
    [SerializeField] private GameObject AITestControllerPrefab;

    [SerializeField] private GameObject ActorPrefab;

    [SerializeField] private GameObject PistolPrefab;
    [SerializeField] private GameObject MachineGunPrefab;
    [SerializeField] private GameObject GrenadeLauncherPrefab;
    [SerializeField] private GameObject RocketLauncherPrefab;

    PlayerController playerController;
    TestAIController testAIController;

    Actor playerActor;

    public enum WeaponSpawnWith
    {
        None,
        Pistol,
        MachineGun,
        GrenadeLauncher,
        RockerLauncher
    }
    public void SpawnEnemy(Vector2 groundSpawnPosition, WeaponSpawnWith weaponToGive)
    {
        //Generate spawn position offset
        float spawnOffsetAngle = Random.value * 3.14159f * 2;
        Vector2 offsetDir = new Vector2(Mathf.Cos(spawnOffsetAngle), Mathf.Sin(spawnOffsetAngle));
        Vector2 spawnPosition = groundSpawnPosition + offsetDir;

        //Get which gun and AI to spawn
        GameObject weaponPrefab = null;
        Controller AI = null;

        switch (weaponToGive)
        {
            case WeaponSpawnWith.None:
                
            case WeaponSpawnWith.Pistol:
                weaponPrefab = PistolPrefab;
                AI = testAIController;
                break;
            default:
                weaponPrefab = PistolPrefab;
                AI = null;
                break;
        }


        //Spawn enemy, put it in location, and then give it the weapon
        GameObject weapon = Instantiate(weaponPrefab);
        weapon.transform.position = spawnPosition;
        weapon.GetComponent<Gun>().setController(AI);
        GameObject enemy = Instantiate(ActorPrefab);
        enemy.transform.position = spawnPosition;

    }

    public void SpawnGroup(int enemyCount)
    {

    }
    
    private void Start()
    {
        playerController = Instantiate(playerControllerPrefab).GetComponent<PlayerController>();
        testAIController = Instantiate(AITestControllerPrefab).GetComponent<TestAIController>();

        playerActor = playerController.getPlayer();





        SpawnEnemy(new Vector2(3, 0), WeaponSpawnWith.Pistol);
    }
}
