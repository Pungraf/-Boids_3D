using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehavior behavior;
    public string VectorLayer = "Vector";
    Renderer rend;

    [Range(10, 500)] public int startingCount = 250;
    const float AgentDensity = 0.08f;
    [Range(1f, 100f)] public float driveFactor = 10f;
    [Range(1f, 100f)] public float maxSpeed = 5f;
    [Range(1f, 10f)] public float neighborRadius = 1.5f;
    [Range(0f, 1f)] public float avoidanceRadiusMultipler = 0.5f;

    private float squareMaxSpeed;
    private float squareNeighborRadius;
    private float squareAvoidanceRadius;

    public float SquareAvoidanceRadius => squareAvoidanceRadius;


    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultipler * avoidanceRadiusMultipler;

        for (int i = 0; i < startingCount; i++)
        {
            Vector3 spawn = new Vector3(Random.insideUnitCircle.x, 0f, Random.insideUnitCircle.y);
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                spawn * startingCount * AgentDensity,
                Quaternion.Euler(0f, Random.Range(0f, 360f), 0f),    
                transform
            );
            newAgent.name = "Agent " + i;
            agents.Add(newAgent);
        }

    }

    // Update is called once per frame
    void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);
            
            //Switching color according to neighbors intensity
            /*rend = agent.GetComponentInChildren<Renderer> ();
            rend.material.color = Color.Lerp(Color.white, Color.red, context.Count / 6f);*/

            Vector3 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            
            agent.Move(move);
        }
        
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius, LayerMask.GetMask(VectorLayer));
        foreach (Collider c in contextColliders)
        {
            if (c != agent.AgentCollider && c)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
    
}
