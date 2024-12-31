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

    [SerializeField] private float[] InitialWeaponSpawnWeights = new float[5];

    PlayerController playerController;
    UnarmedController unarmedController;
    PistolController pistolController;
    MachineGunController machineGunController;
    GrenadeController grenadeController;
    RocketLauncherController rocketLauncherController;

    Actor playerActor;

    Ratio WeaponSpawnRatio;

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
        Vector2 offsetDir = new Vector2(Mathf.Cos(spawnOffsetAngle), Mathf.Sin(spawnOffsetAngle));
        Vector2 spawnPosition = groundSpawnPosition + offsetDir * 2;

        //Give enemy their weapon
        SpawnWeapon(spawnPosition,weaponToGive);


        GameObject enemy = Instantiate(ActorPrefab);
        enemy.transform.position = spawnPosition;
        Actor enemyActor = enemy.GetComponent<Actor>();
    }

    public void SpawnGroup(Vector2 position,int enemyCount)
    {
        float[] spawnWeights = WeaponSpawnRatio.GetRatio();

        for (int i = 0; i < enemyCount; i++)
        {
            float r = Random.value;
            for(int j=0;j<spawnWeights.Length;++j)
            {
                if(r < spawnWeights[j])
                {
                    //Spawn relevant weapon, then break
                    SpawnEnemy(position, (Weapon)j);
                    break;
                }
                r-=spawnWeights[j]; //Reduce r by that value, then continue
            }
        }
    }

    private IEnumerator SpawnWaves()
    {
        int spawnAmount = 5;
        int waveCount = 0;

        while(true)
        {
            Vector2 playerPos = playerActor.transform.position;
            float spawnOffsetAngle = Random.value * 3.14159f * 2;
            Vector2 offsetDir = new Vector2(Mathf.Cos(spawnOffsetAngle), Mathf.Sin(spawnOffsetAngle));


            //Spawn enemies
            SpawnGroup(playerPos+ offsetDir * 10, spawnAmount);

            //Increase difficulty
            WeaponSpawnRatio[2] += 3f;
            WeaponSpawnRatio[3] += 2f;
            WeaponSpawnRatio[4] += 1f;

            

            if(waveCount % 3==0)
            {
                spawnAmount++;
            }

            playerActor.TakeDamage(-30);

            yield return new WaitForSeconds(15);
        }
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

        pistolController.SetDefaultController(unarmedController);
        machineGunController.SetDefaultController(unarmedController);
        grenadeController.SetDefaultController(unarmedController);
        rocketLauncherController.SetDefaultController(unarmedController);

        //Create player game object
        GameObject PlayerObject = Instantiate(ActorPrefab);
        playerActor = PlayerObject.GetComponent<Actor>();
        PlayerObject.name = "Player";
        playerActor.SetController(playerController);

        //Get camera
        GameObject cameraGO = Camera.main.gameObject;
        cameraGO.transform.parent = PlayerObject.transform;
        playerController.SetCamera(Camera.main);

        WeaponSpawnRatio = new Ratio(InitialWeaponSpawnWeights);

        //Give player a machine gun
        SpawnWeapon(Vector2.zero, Weapon.MachineGun);


        StartCoroutine(SpawnWaves());
    }

    private void FixedUpdate()
    {
        if(!playerActor)
        {

            SceneManager.LoadScene("Main Menu");
        }
    }
}
