using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Composite")]
public class CompositeBehavior : FlockBehavior
{
    // Start is called before the first frame update
    public FlockBehavior[] behavior;
    public float[] weights;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (weights.Length != behavior.Length)
        {
            Debug.LogError("Data mismatch in " + name, this);
            return Vector3.zero;
        }

        Vector3 move = Vector3.zero;

        for (int i = 0; i < behavior.Length; i++)
        {
            Vector3 partialMove = behavior[i].CalculateMove(agent, context, flock) * weights[i];

            if (partialMove.sqrMagnitude > weights[i] * weights[i])
            {
                partialMove.Normalize();
                partialMove *= weights[i];
            }

            move += partialMove;
        }

        return move;
    }
}
