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

    public enum Weapon
    {
        None,
        Pistol,
        MachineGun,
        GrenadeLauncher,
        RockerLauncher
    }

    public GameObject SpawnWeapon(Vector2 spawnPosition,  Weapon weaponSpawn)
    {
        GameObject weaponToSpawn = null;
        Controller AI = null;

        switch (weaponSpawn)
        {
            case Weapon.None: 
                return null;
            case Weapon.Pistol:
                weaponToSpawn = PistolPrefab;
                AI = pistolController;
                break;
            case Weapon.MachineGun: 
                weaponToSpawn = MachineGunPrefab;
                //TODO: AI is set to Machine gun AI
                break;
            case Weapon.GrenadeLauncher:
                weaponToSpawn = GrenadeLauncherPrefab;
                //TODO: AI is set to grenade launcher AI
                break;
            case Weapon.RockerLauncher:
                weaponToSpawn= RocketLauncherPrefab;
                //TODO: AI is set to rocket launcher AI
                break;

        }

        GameObject weapon = Instantiate(weaponToSpawn);

        weapon.transform.position = spawnPosition;
        weapon.GetComponent<Gun>().setController(AI);

        return weapon;
    }
    public void SpawnEnemy(Vector2 groundSpawnPosition, Weapon weaponToGive)
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
        SpawnWeapon(Vector2.zero, Weapon.MachineGun);


        SpawnEnemy(new Vector2(3, 0), Weapon.None);

        SpawnWeapon(new Vector2(1, 0), Weapon.Pistol);

        //SpawnEnemy(new Vector2(5, 1), WeaponSpawnWith.None);
        //SpawnEnemy(new Vector2(5, 1), WeaponSpawnWith.None);
        //SpawnEnemy(new Vector2(5, 1), WeaponSpawnWith.None);
        //SpawnEnemy(new Vector2(5, 1), WeaponSpawnWith.None);
    }
}
