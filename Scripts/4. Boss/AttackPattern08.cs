using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern08 : AbsBossAttackPattern
{
    public override void Pattern(Transform target, Transform firePoint, Rigidbody2D rb, Transform trans)
    {
        int bulletsAmount = 3;
        List<int> bulletIndices = new List<int>(); //�Ѿ� �ε����� ������ ����Ʈ ����

        //�Ѿ� �ε��� ���� ����
        for (int i = 0; i < bulletsAmount; i++)
        {
            bulletIndices.Add(i);
        }
        //bulletIndices.Shuffle(); //����Ʈ ��Ҹ� �����ϰ� ����

        float startAngle = -30f; //���� ����
        float endAngle = 30f; //������ ����
        float angleStep = (endAngle - startAngle) / (bulletsAmount - 1); //bullet�� ����
        Vector2 targetDirection = target.position - firePoint.position; //���� ����

        for (int i = 0; i < bulletsAmount; i++)
        {
            float angle = startAngle + angleStep * bulletIndices[i]; //������ ������ �߻� ���� ����

            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            GameObject bullet = BulletPool.instance.GetBullet();
            bullet.transform.position = firePoint.position;
            Vector2 bulletDirection = rotation * targetDirection.normalized;
            bullet.GetComponent<DarkWizardBullet>().SetMoveDirection(bulletDirection);
            bullet.SetActive(true);
        }
    }
}
