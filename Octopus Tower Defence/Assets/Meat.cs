using System.Collections;
using UnityEngine;

public class Meat : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] public float pollution_value = 1f;
    [SerializeField] private float chewDelay = 0.5f;

    private bool isChewing = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Carnivorous_Clam" && !isChewing)
        {
            CC_Chew_Attack chewAttack = collision.GetComponent<CC_Chew_Attack>();
            if (chewAttack != null)
            {
                StartCoroutine(TriggerChewAfterDelay(chewAttack));
            }
        }
    }

    private IEnumerator TriggerChewAfterDelay(CC_Chew_Attack chewAttack)
    {
        isChewing = true;
        yield return new WaitForSeconds(chewDelay);
        chewAttack.ChewMeat(gameObject);
    }
}
