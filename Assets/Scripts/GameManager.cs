using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerControllerPrefab;
    [SerializeField] private GameObject UnarmedControllerPrefab;
    [SerializeField] private GameObject pistolControllerPrefab;

    [SerializeField] private GameObject ActorPrefab;

    [SerializeField] private GameObject PistolPrefab;
    [SerializeField] private GameObject MachineGunPrefab;
    [SerializeField] private GameObject GrenadeLauncherPrefab;
    [SerializeField] private GameObject RocketLauncherPrefab;

    PlayerController playerController;
    UnarmedController unarmedController;
    PistolController pistolController;

    Actor playerActor;

    public enum WeaponSpawnWith
    {
        None,
        Pistol,
        MachineGun,
        GrenadeLauncher,
        RockerLauncher
    }

    public GameObject SpawnWeapon(Vector2 spawnPosition,  WeaponSpawnWith weaponSpawn)
    {
        GameObject weaponToSpawn = null;
        Controller AI = null;

        switch (weaponSpawn)
        {
            case WeaponSpawnWith.None: 
                return null;
            case WeaponSpawnWith.Pistol:
                weaponToSpawn = PistolPrefab;
                AI = pistolController;
                break;
            case WeaponSpawnWith.MachineGun: 
                weaponToSpawn = MachineGunPrefab;
                //TODO: AI is set to Machine gun AI
                break;
            case WeaponSpawnWith.GrenadeLauncher:
                weaponToSpawn = GrenadeLauncherPrefab;
                //TODO: AI is set to grenade launcher AI
                break;
            case WeaponSpawnWith.RockerLauncher:
                weaponToSpawn= RocketLauncherPrefab;
                //TODO: AI is set to rocket launcher AI
                break;

        }

        GameObject weapon = Instantiate(weaponToSpawn);

        weapon.transform.position = spawnPosition;
        weapon.GetComponent<Gun>().setController(AI);

        return weapon;
    }
    public void SpawnEnemy(Vector2 groundSpawnPosition, WeaponSpawnWith weaponToGive)
    {
        //Generate spawn position offset
        float spawnOffsetAngle = Random.value * 3.14159f * 2;
        Vector2 offsetDir = new Vector2(Mathf.Cos(spawnOffsetAngle), Mathf.Sin(spawnOffsetAngle));
        Vector2 spawnPosition = groundSpawnPosition + offsetDir;

        //Give enemy their weapon
        SpawnWeapon(spawnPosition,weaponToGive);


        GameObject enemy = Instantiate(ActorPrefab);
        enemy.transform.position = spawnPosition;
        Actor enemyActor = enemy.GetComponent<Actor>();
        enemyActor.setUnarmedController(unarmedController);

    }

    public void SpawnGroup(int enemyCount)
    {

    }
    
    private void Start()
    {

        //Create controllers
        playerController = new PlayerController();
        unarmedController = new UnarmedController();
        pistolController = new PistolController();


        //Create player game object
        GameObject PlayerObject = Instantiate(ActorPrefab);
        PlayerObject.name = "Player";
        PlayerObject.GetComponent<Actor>().SetController(playerController);

        //Get camera
        GameObject cameraGO = Camera.main.gameObject;
        cameraGO.transform.parent = PlayerObject.transform;
        playerController.SetCamera(Camera.main);

        playerActor = playerController.getPlayer();

        //Give player a machine gun
        SpawnWeapon(Vector2.zero, WeaponSpawnWith.MachineGun);


        SpawnEnemy(new Vector2(3, 0), WeaponSpawnWith.Pistol);

        SpawnEnemy(new Vector2(5, 1), WeaponSpawnWith.None);
        SpawnEnemy(new Vector2(5, 1), WeaponSpawnWith.None);
        //SpawnEnemy(new Vector2(5, 1), WeaponSpawnWith.None);
        //SpawnEnemy(new Vector2(5, 1), WeaponSpawnWith.None);
    }
}
