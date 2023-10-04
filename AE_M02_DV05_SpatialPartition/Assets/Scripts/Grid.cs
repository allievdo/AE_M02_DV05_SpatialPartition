using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SpatialPartitionPattern
{
    public class Grid
    {
        int cellSize;

        Soldier[,] cells;
        
        //Init the grid
        public Grid(int mapWidth, int cellSize)
        {
            this.cellSize = cellSize;

            int numberOfCells = mapWidth/cellSize;

            cells = new Soldier[numberOfCells, numberOfCells];
        }

        public void Add(Soldier soldier)
        {
            //determine which cell the soldier is in
            int cellX = (int)(soldier.soldierTrans.position.x / cellSize);
            int cellZ = (int)(soldier.soldierTrans.position.z / cellSize);

            //add the soldier to the front of the list
            soldier.previousSoldier = null;
            soldier.nextSoldier = cells[cellX, cellZ];

            //associate this cell with this soldier
            cells[cellX, cellZ] = soldier;

            if(soldier.nextSoldier != null)
            {
                soldier.nextSoldier.previousSoldier = soldier;
            }
        }

        public Soldier FindClosestEnemy(Soldier friendlySoldier)
        {
            //determine which cell the friendly soldier is in
            int cellX = (int)(friendlySoldier.soldierTrans.position.x / cellSize);
            int cellZ = (int)(friendlySoldier.soldierTrans.position.z / cellSize);

            Soldier enemy = cells[cellX, cellZ];

            //Find the closest soldier of all in the linked list
            Soldier closestSoldier = null;

            float bestDistSqr = Mathf.Infinity;

            //Loop through the linked list
            while (enemy != null)
            {
                //the distance sqr between the soldier and this enemy
                float distSqr = (enemy.soldierTrans.position - friendlySoldier.soldierTrans.position).sqrMagnitude;

                //if this distance is better than the previous dist, then we have found a closer enemy
                if(distSqr < bestDistSqr)
                {
                    bestDistSqr = distSqr;

                    closestSoldier = enemy;
                }

                //Get the next enemy in the list
                enemy = enemy.nextSoldier;
            }

            return closestSoldier;
        }

        public void Move(Soldier soldier, Vector3 oldPos)
        {
            //see which cell it was in
            int oldCellX = (int)(oldPos.x / cellSize);
            int oldCellZ = (int)(oldPos.z / cellSize);

            //see which cell it is in now
            int cellX = (int)(soldier.soldierTrans.position.x / cellSize);
            int cellZ = (int)(soldier.soldierTrans.position.z / cellSize);

            //if it didnt change cell, were done\
            if (oldCellX == cellX && oldCellZ == cellZ)
            {
                return;
            }

            //unlink it from the list of its old cell
            if(soldier.previousSoldier != null)
            {
                soldier.previousSoldier.nextSoldier = soldier.previousSoldier;
            }

            //if its the head of a list, remove it
            if (cells[oldCellX, oldCellZ] == soldier)
            {
                cells[oldCellX, oldCellZ] = soldier.nextSoldier;
            }

            //add it back to the grid at its new cell
            Add(soldier);
        }
    }
}
