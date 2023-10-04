using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartitionPattern
{
    public class GameController : MonoBehaviour
    {
        public GameObject friendlyObj;
        public GameObject enemyObj;

        public Material enemyMaterial;
        public Material closestEnemyMaterial;

        public Transform enemyParent;
        public Transform friendlyParent;

        List<Soldier> enemySoldiers = new List<Soldier>();
        List<Soldier> friendlySoldiers = new List<Soldier>();

        List<Soldier> closestEnemies = new List<Soldier>();

        float mapWidth = 50f;
        int cellSize = 10;

        int numberOfSoldiers = 100;

        Grid grid;

        void Start()
        {
            grid = new Grid((int)mapWidth, cellSize);

            //Add random enemies and friendly and store them in a list :)
            for (int i = 0; i < numberOfSoldiers; i++)
            {
                //Give the enemy a random position
                Vector3 randomPos = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));

                //create a new enemy
                GameObject newEnemy = Instantiate(enemyObj, randomPos, Quaternion.identity) as GameObject;

                //add the enmy to a list
                enemySoldiers.Add(new Enemy(newEnemy, mapWidth, grid));

                //parent it
                newEnemy.transform.parent = enemyParent;

                //give teh friendky a random position
                randomPos = new Vector3(Random.Range(0f, mapWidth), 0.5f, Random.Range(0f, mapWidth));

                //Create a new friendly
                GameObject newFriendly = Instantiate(friendlyObj, randomPos, Quaternion.identity) as GameObject;

                //Add the friendly to a list
                friendlySoldiers.Add(new Friendly(newFriendly, mapWidth));

                //Parent it
                newFriendly.transform.parent = friendlyParent;
            }
        }

        void Update()
        { 
            //elapsed time is more at the bottom make a start time variable.
            //Move the enemies
            for(int i = 0; i < enemySoldiers.Count; i++)
            {
                enemySoldiers[i].Move();
            }

            //reset material of the closest enemies
            for (int i = 0; i < closestEnemies.Count; i++)
            {
                closestEnemies[i].soldierMeshRenderer.material = enemyMaterial;
            }

            //reset the list with closest enemies
            closestEnemies.Clear();

            //For each friendly, find the closest enemy and change its color and chase it.
            for (int i = 0; i < friendlySoldiers.Count; i++)
            {
                Soldier closestEnemy = grid.FindClosestEnemy(friendlySoldiers[i]);

                //if we found an enemy
                if (closestEnemy != null)
                {
                    //change material
                    closestEnemy.soldierMeshRenderer.material = closestEnemyMaterial;

                    closestEnemies.Add(closestEnemy);

                    //move the friendly in the direction of the enemy
                    friendlySoldiers[i].Move(closestEnemy);
                }
            }
        }

        //Find the closest enemy slow ver
        Soldier FindClosestEnemySlow(Soldier soldier)
        {
            Soldier closestEnemy = null;

            float bestDistSqr = Mathf.Infinity;

            //Loop through all enemies
            for (int i = 0; i < enemySoldiers.Count; i++) 
            { 
                // the distance sqr between the soldier and this enemy
                float distSqr = (soldier.soldierTrans.position - enemySoldiers[i].soldierTrans.position).sqrMagnitude;

                //If this distance is better than the prev best distance, the we have to find a closer enemy
                if(distSqr < bestDistSqr)
                {
                    bestDistSqr = distSqr;
                    closestEnemy = enemySoldiers[i];
                }
            }

            return closestEnemy;
        }
    }
}
