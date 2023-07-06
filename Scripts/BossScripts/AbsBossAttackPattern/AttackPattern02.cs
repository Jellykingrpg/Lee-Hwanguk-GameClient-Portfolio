using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern02 : AbsBossAttackPattern
{
    public override void Pattern(Transform player, Transform firePoint, Rigidbody2D rb, Transform trans)
    {
        int bulletsAmount = 15;
        float startAngle = 0f; // ���� ����
        float endAngle = 360f; // ������ ����
        float angleStep = (endAngle - startAngle) / bulletsAmount; // bullet�� ����

        for (int i = 0; i < bulletsAmount; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector3 bulMoveVector = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
            var bulDir = (player.position - firePoint.transform.position).normalized;
            GameObject bul = BulletPool.instance.GetBullet();
            bul.transform.position = firePoint.transform.position;

            // ȸ���� ����
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, bulDir);
            bul.transform.rotation = rotation;

            // ȸ�� �������� �̵� ���� ����
            bul.GetComponent<FireWormBullet>().SetMoveDirection(bulMoveVector);

            bul.SetActive(true);
        }
    }
}
