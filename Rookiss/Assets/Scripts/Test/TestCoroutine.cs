using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCoroutine : MonoBehaviour
{
    class Test { public int Id = 0; }

    class CoroutineTest : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new Test() { Id = 1 };
            yield return new Test() { Id = 2 };
            yield return new Test() { Id = 3 };
            yield return new Test() { Id = 4 };
        }
    }

    float deltaTime = 0;

    Coroutine co;

    private void Start()
    {
        CoroutineTest test = new CoroutineTest();
        foreach (System.Object t in test)
        {
            Test value = (Test)t;
            Debug.Log(value.Id);
        }

        co = StartCoroutine(ExplodeVolcano(4.0f));

        StartCoroutine(CoStopExplode(2.0f));
    }

    public IEnumerator ExplodeVolcano(float seconds)
    {
        Debug.Log("Explode Enter");
        yield return new WaitForSeconds(seconds);
        Debug.Log("Explode Execute!!!");
    }

    public IEnumerator CoStopExplode(float seconds)
    {
        Debug.Log("Stop Enter");
        yield return new WaitForSeconds(seconds);
        Debug.Log("Stop Execute!!!");

        if(co != null)
        {
            StopCoroutine(co);
            co = null;
        }
    }
}
