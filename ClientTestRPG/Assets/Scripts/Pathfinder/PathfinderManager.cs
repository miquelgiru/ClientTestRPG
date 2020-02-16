using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class PathfinderManager : MonoBehaviour
{
    #region Instance
    private static PathfinderManager instance;
    public static PathfinderManager Instance { get { return instance; } }
    #endregion

    List<Pathfinder> currentJobs = new List<Pathfinder>();
    List<Pathfinder> toDoJobs = new List<Pathfinder>();
    public int MaxJobs = 3;
    public float MaxTimePerJob = 5;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        int i = 0;
        float delta = Time.deltaTime;

        while(i < currentJobs.Count)
        {
            if (currentJobs[i].isDone)
            {
                currentJobs[i].JobFinished();
                currentJobs.RemoveAt(i);
            }

            else
            {
                currentJobs[i].timer += delta;

                if(currentJobs[i].timer > MaxTimePerJob)
                {
                    currentJobs[i].isDone = true;
                }
                ++i;
            }
        }

        if(toDoJobs.Count > 0 && currentJobs.Count < MaxJobs)
        {
            Pathfinder job = toDoJobs[0];
            toDoJobs.RemoveAt(0);
            currentJobs.Add(job);

            Thread jobThread = new Thread(job.FindPath);
            jobThread.Start();
        }
    }

    public void RequestPathfind(Unit unit, GridNode start, GridNode end, Pathfinder.PathfindFinished callback)
    {
        Pathfinder job = new Pathfinder(unit, start, end, callback);
        toDoJobs.Add(job);
    }
}
