using System;
using Traps.TrapsGenerationLogic;
using UnityEngine;

namespace Traps
{
    public class Branch : MovableTrap
    {
  
        protected override void Move()
        {
            transform.position -= new Vector3(_moveSpeed * Time.deltaTime, 0, 0);

        }
    }
}