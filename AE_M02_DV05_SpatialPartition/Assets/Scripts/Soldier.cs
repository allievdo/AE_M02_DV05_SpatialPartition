using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialPartitionPattern
{
    public class Soldier
    {
        //To change material
        public MeshRenderer soldierMeshRenderer;

        //to move soldier
        public Transform soldierTrans;

        //the speed the soldier is walking with
        protected float walkSpeed;

        public Soldier previousSoldier;
        public Soldier nextSoldier;

        //the enemy doesnt need outside info
        public virtual void Move()
        {

        }

        //the friendly has to move to which soldier is the closest
        public virtual void Move(Soldier soldier)
        {

        }
    }
}