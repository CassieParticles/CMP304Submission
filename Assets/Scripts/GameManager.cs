using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject ActorPrefab;

    [SerializeField] private GameObject PistolPrefab;
    [SerializeField] private GameObject MachineGunPrefab;
    [SerializeField] private GameObject GrenadeLauncherPrefab;
    [SerializeField] private GameObject RocketLauncherPrefab;

    PlayerController playerController;
    UnarmedController unarmedController;
    PistolController pistolController;
    MachineGunController machineGunController;
    GrenadeController grenadeController;
    RocketLauncherController rocketLauncherController;

    Actor playerActor;

    public enum Weapon
    {
        None,
        Pistol,
        MachineGun,
        GrenadeLauncher,
        RocketLauncher
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
                AI = machineGunController;
                break;
            case Weapon.GrenadeLauncher:
                weaponToSpawn = GrenadeLauncherPrefab;
                AI = grenadeController;
                break;
            case Weapon.RocketLauncher:
                weaponToSpawn= RocketLauncherPrefab;
                AI = rocketLauncherController;
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
        Vector2 offsetDir = new Vector2(Mathf.Cos(spawnOffsetAngle), Mathf.Sin(spawnOffsetAngle)) * 2;
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
        machineGunController = new MachineGunController();
        grenadeController = new GrenadeController();
        rocketLauncherController = new RocketLauncherController();

        //Create player game object
        GameObject PlayerObject = Instantiate(ActorPrefab);
        playerActor = PlayerObject.GetComponent<Actor>();
        PlayerObject.name = "Player";
        playerActor.SetController(playerController);

        //Get camera
        GameObject cameraGO = Camera.main.gameObject;
        cameraGO.transform.parent = PlayerObject.transform;
        playerController.SetCamera(Camera.main);

        //Give player a machine gun
        SpawnWeapon(Vector2.zero, Weapon.MachineGun);


        SpawnEnemy(new Vector2(7, 0), Weapon.RocketLauncher);
        SpawnEnemy(new Vector2(7, 0), Weapon.RocketLauncher);
        SpawnEnemy(new Vector2(7, 0), Weapon.RocketLauncher);
        SpawnEnemy(new Vector2(7, 0), Weapon.RocketLauncher);
    }

    private void FixedUpdate()
    {
        if(!playerActor)
        {

            SceneManager.LoadScene("Main Menu");
        }
    }
}
