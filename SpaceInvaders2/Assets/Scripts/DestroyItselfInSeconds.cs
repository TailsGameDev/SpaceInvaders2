using System.Collections;
using UnityEngine;

public class DestroyItselfInSeconds : MonoBehaviour
{
    [SerializeField]
    private float timeToDestruction = 0.0f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(timeToDestruction);

        Destroy(gameObject);
    }
}
