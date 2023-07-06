using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissilePool : MonoBehaviour
{
    public static HomingMissilePool instance;
    [SerializeField]
    private GameObject pooledHomingMissile;
    private bool notEnoughBulletsInPool = true;
    private List<GameObject> homingMissilesList;

    private void Awake()
    {
        instance= this;
    }
    void Start()
    {
        this.homingMissilesList = new List<GameObject>(); //homingMissiles list �ʱ�ȭ

    }
    public GameObject GetHomingMissile()
    {
        if (this.homingMissilesList.Count > 0)
        {
            for (int i = 0; i < this.homingMissilesList.Count; i++)
            {
                if (!this.homingMissilesList[i].activeInHierarchy) //�θ� ������Ʈ�� Ȱ��ȭ üũ
                {
                    return this.homingMissilesList[i];
                }
            }
        }

        if (this.notEnoughBulletsInPool)
        {
            GameObject bul = Instantiate(this.pooledHomingMissile);
            bul.SetActive(false);
            this.homingMissilesList.Add(bul);
            return bul;
        }
        return null;
    }
}
