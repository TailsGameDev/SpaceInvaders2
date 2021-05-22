using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField]
    private int barrierHorizontalDivisions = 0;
    [SerializeField]
    private int divisionsMaxDurability = 0;

    [SerializeField]
    private Transform topPosition = null;
    [SerializeField]
    private Transform bottomPosition = null;
    [SerializeField]
    private Transform rightmostPosition = null;

    [SerializeField]
    private SpriteMask spriteMaskPrototype = null;

    private float height;

    private DestructionLevel[] destructionLevels;

    private void Awake()
    {
        DestructionLevel.MaxDurability = divisionsMaxDurability;
        destructionLevels = new DestructionLevel[barrierHorizontalDivisions];
        for (int d = 0; d < destructionLevels.Length; d++)
        {
            destructionLevels[d] = new DestructionLevel();
        }

        this.height = topPosition.position.y - bottomPosition.position.y;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.activeInHierarchy && other.tag == "bullet")
        {
            float length = 2 * (rightmostPosition.position.x - transform.position.x);
            float leftmostPosition = transform.position.x - (length / 2);

            float currentComparePosition = leftmostPosition;
            int partition = -1;
            float increment = length / barrierHorizontalDivisions;
            do
            {
                // currentComparePosition += increment;
                partition++;
                currentComparePosition = leftmostPosition + (increment*(partition+1));
            } while (currentComparePosition < other.transform.position.x);

            DestructionLevel divisionToBreak;
            if (partition < destructionLevels.Length)
            {
                divisionToBreak = destructionLevels[partition];
            }
            else
            {
                // it's possible to overcome as bullet.x is in the middle of the bullet, not in the left corner.
                divisionToBreak = destructionLevels[destructionLevels.Length - 1];
            }
            
            bool isTopDestruction = other.transform.position.y > transform.position.y;
            if (!divisionToBreak.IsFullyDestroyed())
            {
                if (isTopDestruction)
                {
                    divisionToBreak.BreakTop();
                    // TODO: make bullet be destroyed only when it reaches the hit point
                    float limitY = divisionToBreak.GetHitPointOnTop(height, other.transform.position.y);

                    float yPosition = (limitY + other.transform.position.y) / 2;
                    float xPosition = (currentComparePosition - (increment / 2));
                    SpriteMask newSpriteMask = Instantiate(spriteMaskPrototype, 
                        new Vector3(xPosition, yPosition, 0.0f ), Quaternion.identity);

                    newSpriteMask.transform.parent = null;
                    float ySize = (other.transform.position.y - limitY);
                    newSpriteMask.transform.localScale = new Vector3(increment, ySize, 1.0f);

                    newSpriteMask.transform.parent = spriteMaskPrototype.transform.parent;

                    // It was getting a little weird at the top and bottom, let's paint it black
                    if (divisionToBreak.DestructionTop == 1)
                    {
                        Instantiate(newSpriteMask, other.transform.position, Quaternion.identity);
                    }
                }
                else
                {
                    divisionToBreak.BreakBottom();
                    
                    float limitY = divisionToBreak.GetHitPointOnBottom(height, other.transform.position.y);

                    float yPosition = (limitY + other.transform.position.y) / 2;
                    float xPosition = (currentComparePosition - (increment / 2));
                    SpriteMask newSpriteMask = Instantiate(spriteMaskPrototype,
                        new Vector3(xPosition, yPosition, 0.0f), Quaternion.identity);
                    newSpriteMask.transform.parent = spriteMaskPrototype.transform.parent;
                    newSpriteMask.transform.localScale = spriteMaskPrototype.transform.localScale;
                    /*

                    float ySize = limitY - other.transform.position.y;
                    newSpriteMask.transform.localScale = new Vector3(increment, ySize, 1.0f);

                    newSpriteMask.transform.parent = spriteMaskPrototype.transform.parent;

                    // It was getting a little weird at the top and bottom, let's paint it black
                    if (divisionToBreak.DestructionBottom == 1)
                    {
                        Instantiate(newSpriteMask, other.transform.position - Vector3.up*0.25f, Quaternion.identity);
                    }
                    */
                }

                other.gameObject.SetActive(false);
                Destroy(other.gameObject);

            }
        }
    }

    private class DestructionLevel
    {
        private static int maxDurability;
        private int destructionTop;
        private int destructionBottom;

        public static int MaxDurability 
        {
            set 
            { 
                maxDurability = value; 
            }
        }

        public int DestructionTop { get => destructionTop; }
        public int DestructionBottom { get => destructionBottom; }

        public void BreakTop()
        {
            destructionTop++;
        }
        
        public void BreakBottom()
        {
            destructionBottom++;
        }

        public bool IsFullyDestroyed()
        {
            return (destructionBottom + destructionTop) >= maxDurability;
        }

        public float GetHitPointOnTop(float height, float otherYPosition)
        {
            float ySegmentSize = height / maxDurability;
            float hitPointY = otherYPosition - (destructionTop * ySegmentSize);
            return hitPointY;
        }

        public float GetHitPointOnBottom(float height, float otherYPosition)
        {
            float ySegmentSize = height / maxDurability;
            float hitPointY = otherYPosition + (destructionBottom * ySegmentSize);
            return hitPointY;
        }
    }
}
