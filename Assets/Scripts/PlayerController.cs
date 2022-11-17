using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public int health;
    public int maxHealth;
    public GameObject[] weapons;
    public GameDirector director;
    bool wDown;
    bool fDown;
    float hAxis;
    float vAxis;

    bool isBorder;
    bool isDamage;
    bool isFireReady;
    bool isDead;

    Vector3 moveVec;

    Rigidbody rigid;
    Animator anim;
    MeshRenderer[] meshs;

    Weapon equipWeapon;
    float fireDelay;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        Swap();
    }

    void FreezeRotation(){
        rigid.angularVelocity = Vector3.zero;
    }
    void FixedUpdate() {
        FreezeRotation();
        StopToWall();
    }
    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    
    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Attack();
    }

    void GetInput(){
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        fDown = Input.GetButton("Fire1");
    }

    void Move(){
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //걷는 로직

        if(!isBorder)
            transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;
        if(isDead)
            moveVec = Vector3.zero;
        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void Turn(){
        transform.LookAt(transform.position + moveVec); //회전로직
    }

    void Attack(){
        fireDelay += Time.deltaTime;

        isFireReady = equipWeapon.rate < fireDelay;
        if(fDown && isFireReady){
            equipWeapon.Use();
            anim.SetTrigger("doShot");
            fireDelay = 0;
        }
    }

    void Swap(){
        int weaponIndex = 0;
        equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
        equipWeapon.gameObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyBullet"){
            if(!isDamage){
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;
                StartCoroutine(OnDamage());
            }
            

        }
    }
    void OnDie(){
        anim.SetTrigger("doDie");
        isDead = true;
        director.GameOver();
    }
    IEnumerator OnDamage()
    {
        isDamage = true;
        foreach(MeshRenderer mesh in meshs){
            mesh.material.color = Color.yellow;
        }
        yield return new WaitForSeconds(1f);

        isDamage = false;
        foreach(MeshRenderer mesh in meshs){
            mesh.material.color = Color.white;
        }

    }
}