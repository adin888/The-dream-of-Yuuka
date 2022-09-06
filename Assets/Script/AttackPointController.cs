using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPointController : MonoBehaviour
{
    string m_numOfAttack;

    public Animator m_animator;
    public Transform AttackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;
    public int attackDamage = 40;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public int numOfAttackFrames;

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Attack1"))
            {
                m_numOfAttack = "Attack1";
                Attack(m_numOfAttack);
                nextAttackTime = Time.time + 1f / attackRate;
            }
            if (Input.GetButtonDown("Attack2"))
            {
                m_numOfAttack = "Attack2";
                Attack(m_numOfAttack);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

    }
    void Attack(string numOfAttack)
    {
        m_animator.SetTrigger(numOfAttack);
        //让AttackPoint在每一帧具有不同的位置，输入参数为（位置，范围，（形状），间隔时间）
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyController>().TakeDamage(attackDamage);
        }
    }
    void AttackPointTrace(Vector3[] position, float attackRange, float intervalTime)
    {

    }
    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null)
            return;
        Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
    }
}
