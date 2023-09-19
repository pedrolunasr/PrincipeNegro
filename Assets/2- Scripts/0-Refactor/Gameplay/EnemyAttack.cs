using System.Collections;
using System.Collections.Generic;
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
        public SimpleWarriorController enemy;

        public override void Execute()
        {

            //_coroutineRunner.StartCoroutine("Attack");
            enemy.StartCoroutine(Attack());

        }

        IEnumerator Attack()
        {
           
            AttackCheckSimpleWarrior.checkAttack = false;
            enemy.colliderCheckAttack.enabled = false;
            enemy.colliderAttack.enabled = true;
            enemy.animator.SetTrigger("Attack");
            enemy.moveSpeed = 0;

            yield return new WaitForSeconds(1.5f); //Velocidade de ataque/recuperaço
            
            enemy.moveSpeed = 1.2f;
            enemy.colliderCheckAttack.enabled = true;
            enemy.colliderAttack.enabled = false;
            if (enemy._audio && enemy.ouch)
                enemy._audio.PlayOneShot(enemy.ouch);

        }

    }
}