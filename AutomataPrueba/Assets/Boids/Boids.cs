using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{

    [SerializeField]
    GameObject agentPrefab;

    [SerializeField]
    int numBoids = 10;

    Agent[] agents;

    [SerializeField]
    float agentRadius = 2.0f;

    private float distance;

    Vector3 separateForece;
 

    [SerializeField]
    float separationWeight = 1.0f, cohesionWeight = 1.0f, alignmentWeight = 1.0f;

    private void Awake()
    {
        List<Agent> agentlist = new List<Agent>();

        for(int i = 0; i<numBoids; i++)
        {
            Vector3 position = Vector3.up * Random.Range(0, 10)
                + Vector3.right * Random.Range(0, 10) + Vector3.forward * Random.Range(0, 10);
            agentlist.Add(Instantiate(agentPrefab, position, Quaternion.identity).GetComponent<Agent>());
            agentlist[agentlist.Count - 1].radius = agentRadius;
        }
        agents = agentlist.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Agent a in agents)
        {
            a.velocity = new Vector3(1, 0, 0);
            checkForNeightBours(a);
            calculateSeparation(a);
            calculateAlignment(a);
            calculateCohesion(a);
            a.updateAgent();
            a.neightbours.Clear();
         
        }
    }

    void checkForNeightBours(Agent a)
    {
        a.checkNeightbours();
    }

    void calculateSeparation(Agent a)
    {
        foreach (Agent neightbours in a.neightbours)
        {
            distance = Vector3.Distance(a.transform.position, neightbours.transform.position);
            distance /= agentRadius;
            separateForece = distance * (a.transform.position - neightbours.transform.position) * separationWeight;

            a.addForce(separateForece, Agent.DEBUGforceType.SEPARATION);
        }           
    }

    void calculateCohesion(Agent a)
    {

        Vector3 centralPosistion = Vector3.zero;

        foreach (Agent neightbours in a.neightbours)
        {
            centralPosistion += neightbours.transform.position;
        }

        centralPosistion += a.transform.position;
        centralPosistion /= a.neightbours.Count + 1;

        a.addForce((centralPosistion - a.transform.position) * cohesionWeight , Agent.DEBUGforceType.COHESION);

    }

    void calculateAlignment(Agent a)
    {
        Vector3 dirVec = Vector3.zero;
        foreach (Agent neightbours in a.neightbours)
        {
            dirVec += neightbours.velocity;        
        }

        dirVec += a.velocity;
        dirVec /= a.neightbours.Count + 1;

        a.addForce(dirVec, Agent.DEBUGforceType.ALIGNMENT);
    }
}
