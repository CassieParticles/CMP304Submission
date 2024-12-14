using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerControllerPrefab;
    [SerializeField] private GameObject AITestControllerPrefab;

    [SerializeField] private GameObject ActorPrefab;

    PlayerController playerController;
    TestAIController testAIController;

    Actor playerActor;
    
    private void Start()
    {
        playerController = Instantiate(playerControllerPrefab).GetComponent<PlayerController>();
        testAIController = Instantiate(AITestControllerPrefab).GetComponent<TestAIController>();

        playerActor = playerController.getPlayer();

        for(int i=0;i<0;++i)
        {
            GameObject actor = Instantiate(ActorPrefab);
            actor.transform.position = new Vector3(Random.value * 10, Random.value * 10, 0);
            actor.GetComponent<Actor>().SetController(testAIController);
            actor.GetComponent<Actor>().AddNewTarget(playerActor);
        }
    }
}
