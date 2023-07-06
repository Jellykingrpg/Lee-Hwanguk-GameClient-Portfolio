using UnityEngine;

public class AttackPattern05 : AbsBossAttackPattern
{
    public override void Pattern(Transform target, Transform firePoint, Rigidbody2D rb, Transform trans)
    {
        int bulletsAmount = 3;
        float startAngle = -20f; //���� ����
        float endAngle = 20f; //������ ����
        float angleStep = (endAngle - startAngle) / (bulletsAmount - 1); //bullet�� ����
        Vector2 targetDirection = target.position - firePoint.position; //���� ����
        for (int i = 0; i < bulletsAmount; i++)
        {
            float angle = startAngle + angleStep * i;
            // �߻� ������ ȸ���� ���
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            // ������Ʈ Ǯ���� �Ѿ� ��������
            GameObject bullet = BulletPool.instance.GetBullet();
            bullet.transform.position = firePoint.position;
            Vector2 bulletDirection = rotation * targetDirection.normalized;
            bullet.GetComponent<EvilWizardBullet>().SetMoveDirection(bulletDirection);
            bullet.SetActive(true);
        }
    }
}
