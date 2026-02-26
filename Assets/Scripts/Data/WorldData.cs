using UnityEngine;
using System.Collections.Generic;

public class WorldData : MonoBehaviour
{
    public static WorldData Instance;

    public Vector3 playerPosition;
    public bool hasSavedPosition = false;

    public Vector3 currentCheckpoint;
    public bool hasCheckpoint = false;

    public bool win;

    public HashSet<string> completedEvents = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (WorldData.Instance != null)
        {
            transform.position = WorldData.Instance.playerPosition;
        }
    }

    public void SaveCheckpoint(Vector3 position)
    {
        currentCheckpoint = position;
        hasCheckpoint = true;
    }

    public void RegisterEnemyDefeated(string enemyID) => completedEvents.Add(enemyID);
    public bool IsEnemyDefeated(string enemyID) => completedEvents.Contains(enemyID);

    public void CompleteEvent(string enemyId)
    {
        PlayerParty pParty = FindObjectOfType<PlayerParty>();
        RescuedCharacterData rCharData = FindObjectOfType<RescuedCharacterData>();
        switch (enemyId)
        {
            case "paquirrín":
            {
                pParty.AddCharacter(rCharData.rescuableCombatants[2]);
                break;
            }
            case "cigala":
            {
                pParty.AddCharacter(rCharData.rescuableCombatants[1]);
                break;
            }
            case "fariV1":
            {
                pParty.AddCharacter(rCharData.rescuableCombatants[0]);
                break;
            }
            default:
            {
                break;
            }
        }
    }
}