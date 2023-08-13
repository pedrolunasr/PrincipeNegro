using Platformer.Core;
using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Attack when the player collider
    /// </summary>
    /// <typeparam name="EnemyAttack"></typeparam>
    public class EnemyAttack : Simulation.Event<EnemyAttack>
    {
        public EnemyController enemy;

        public override void Execute()
        {
            enemy._collider.enabled = false;
            enemy.control.enabled = false;

            AttackCheckSimpleWarrior.checkAttack = false;

            enemy._animator.SetTrigger("Attack");
            //moveSpeed = 0;

            //yield return new WaitForSeconds(1.5f); //Velocidade de ataque/recupera��o

            //moveSpeed = 1.2f;
            //colliderCheckAtk.enabled = true;

            enemy._collider.enabled = true;
            enemy.control.enabled = true;

            if (enemy._audio && enemy.ouch)
                enemy._audio.PlayOneShot(enemy.ouch);
        }
    }
}