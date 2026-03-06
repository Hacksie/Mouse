using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign
{
    public class EnemyDeadState: IEnemyState
    {

        public bool IsAlive => false;

        public EnemyDeadState()
        {
        }

        public void UpdateBehaviour(AiContext ctx)
        {

        }

        public void Begin()
        {
        }

        public void End()
        {

        }
    }
}
