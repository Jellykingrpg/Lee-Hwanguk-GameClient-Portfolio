using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilWizardBullet : MonoBehaviour
{
    private Vector2 moveDirection;
    private float moveSpeed;
    public GameObject explosion;
    private Rigidbody2D rb;

    private void OnEnable() //컴포넌트 활성 시 호출
    {
        Invoke("Destroy", 3f);

    }
    private void Start()
    {
        this.moveSpeed = 3f;
        this.rb = GetComponent<Rigidbody2D>();

    }
    void FixedUpdate()
    {
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rb.velocity = moveDirection * moveSpeed;
    }
    public void SetMoveDirection(Vector2 dir)
    {
        this.moveDirection = dir.normalized;

    }
    private void Destroy()
    {
        this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //this.gameObject.SetActive(false);
            var explosionGo = Instantiate(this.explosion);
            explosionGo.transform.position = this.transform.position;
            EventDispatcher.Instance.Dispatch(EventDispatcher.EventName.MainCameraControllerHitEffects);
        }
        if (collision.name == "BossRoomColider")
        {
            Debug.Log("BulletDestroy");
            this.Destroy();
        }
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
}
