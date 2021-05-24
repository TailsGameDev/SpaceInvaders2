using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplifiedAnimatorRandomizor : MonoBehaviour
{
    [SerializeField]
    private SimplifiedAnimator[] simplifiedAnimators = null;

    private void Awake()
    {
        foreach (SimplifiedAnimator anim in simplifiedAnimators)
        {
            anim.enabled = false;
        }

        simplifiedAnimators[Random.Range(0, simplifiedAnimators.Length)].enabled = true;
    }
}
