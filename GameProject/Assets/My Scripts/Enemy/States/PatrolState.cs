using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex;
    public float waitTimer;

    public override void Enter()
    {
    }
    public override void Perform()
    {
        PatrolCycle();
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }
    public override void Exit()
    {
    }
    public void PatrolCycle()
    {
        //implement out patrol logic
        if(enemy.Agent.remainingDistance < 0.2f)
        {
            waitTimer += Time.deltaTime;
            if(waitTimer > 3f)
            {
                if (waypointIndex < enemy.myPath.waypoints.Count)
                {
                    enemy.Agent.SetDestination(enemy.myPath.waypoints[waypointIndex].position);
                    waypointIndex++;
                }
                else
                {
                    waypointIndex = 0;
                }
                waitTimer = 0;
            }
        }
    }
}
