using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SpatialPartitionPattern
{
    public class Enemy : Soldier
    {
        //the pos of the soldier is heading when moving
        Vector3 currentTarget;
        //the pos of the soldier before it had moved
        Vector3 oldPos;
        // the width of the mao to generate random coordinated within the map
        float mapWidth;
        //the grid
        Grid grid;

        //initialize enemy
        public Enemy(GameObject soldierObj, float mapWidth, Grid grid)
        {
            //save what we need to save
            this.soldierTrans = soldierObj.transform;

            this.soldierMeshRenderer = soldierObj.GetComponent<MeshRenderer>();

            this.mapWidth = mapWidth;
            this.grid = grid;

            //Add this unit to the grid
            grid.Add(this);

            //Init the old pos
            oldPos = soldierTrans.position;

            this.walkSpeed = 5f;

            //give it a rand coordinate to move towards
            GetNewTarget();
        }

        public override void Move()
        {
            //Move towards the target
            soldierTrans.Translate(Vector3.forward * Time.deltaTime * walkSpeed);

            //See if the cube has moved to another cell
            grid.Move(this, oldPos);

            //save old pos
            oldPos = soldierTrans.position;

            //if the soldier has reached the target, find a new target
            if ((soldierTrans.position - currentTarget).magnitude < 1f)
            {
                GetNewTarget();
            }
        }

        void GetNewTarget()
        {
            currentTarget = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));

            //Rotate toward the target
            soldierTrans.rotation = Quaternion.LookRotation(currentTarget - soldierTrans.position);
        }
    }
}