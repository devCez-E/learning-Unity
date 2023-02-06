using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField] int reserveCount = 0;

    [SerializeField] int monsterCount = 0;
    [SerializeField] int keepMonsterCount = 0;

    [SerializeField] Vector3 spawnPos;
    [SerializeField] float spawnRadius = 0f;
    [SerializeField] float spawnTime = 5.0f;

    public void AddMonsterCount(int value) { monsterCount += value; }
    public void SetKeepMonsterCount(int count) { keepMonsterCount = count; }

    private void Start()
    {
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    private void Update()
    {
        while(monsterCount + reserveCount < keepMonsterCount)
        {
            StartCoroutine(ReserveSpawn());
        }
    }

    IEnumerator ReserveSpawn()
    {
        reserveCount++;
        yield return new WaitForSeconds(Random.Range(0, spawnTime));
        GameObject go = Managers.Game.Spawn(Define.GameObj.Monster, "Prefabs/Knight");
        NavMeshAgent nma = go.GetOrAddComponent<NavMeshAgent>();

        Vector3 randPos;

        while (true)
        {
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, spawnRadius);
            randDir.y = 0;
            randPos = spawnPos + randDir;

            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(randPos, path)) break;
        }

        go.transform.position = randPos;
        reserveCount--;
    }
}
