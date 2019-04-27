using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour
{
    public int health = 4;
    public int health_max = 2;
    const int health_max_limit = 24;

    public float speed = 4;
    protected Vector3 in_velocity = Vector3.zero;
    protected float in_rotation = 180; // 180 = UP / 0 = DOWN / 270 = RIGHT / 90 = LEFT

    public virtual void addHealthPoint(int amount)
    {
        health += amount;
        if (health > health_max * 2) health = health_max * 2;
        print(name + " : regen " + amount + "hp");
    }
    public virtual void addHealthMax(int amount)
    {
        health += amount*2;
        health_max += amount;
        if (health_max > health_max_limit) health_max = health_max_limit;
        print(name + " : add " + amount + " extra heart");
    }
    public virtual void takeHit(int amount)
    {
        GameObject go = Instantiate(MNG_Game.instance.fx_hitPoc, transform.position, Quaternion.identity);
        print(go.name);
        print(name + " : Take damage  (" + amount + "pt)");
        health -= amount;
        if (health <= 0)
        {
            onDeath();
        }
    }
    public virtual void giveDamage(GameObject other, int amount)
    {
        print(name + " : Give damage  (" + amount + "pt) on " + other.name + " ! ");
    }
    public virtual void onDeath()
    {
        print(name + " : Dead ! ");
        Instantiate(MNG_Game.instance.fx_deathSmoke, transform.position, Quaternion.identity);
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Trigger")
        {
            other.GetComponent<TRG>().trigger(this);
        }
    }

    public IEnumerator ActionTimer(System.Action action, float countdownValue = 2.0f)
    {
        yield return new WaitForSeconds(countdownValue);
        action();
    }

}

public class Player : Actor
{
    public Rigidbody bdy;

    Coroutine hitRecovery;
    public bool flag_Invulnerable = false;

    Coroutine attackRecovery;
    public bool flag_canAttack = true;

    Coroutine moveRecovery;
    public bool flag_canMove = true;

    public GameObject SwordAttackBox;

    public int rubyCount = 0;
    public int bombCount = 0;

    private void Start()
    {
        InvokeRepeating("GetLastValidPosition", 0.5f, 0.5f);
        RefreshHUD();
    }

    void RefreshHUD() { RefreshHealth(); RefreshBomb(); RefreshRubys(); }
    void RefreshHealth() { MNG_Game.instance.hud.Refresh_Health(health, health_max); }

    //GESTION ITEM COLLECTABLES
    void RefreshBomb() { MNG_Game.instance.hud.Refresh_Bomb(bombCount); }
    void RefreshRubys() { MNG_Game.instance.hud.Refresh_Rubys(rubyCount); }
    public void takeRuby(int amount) { rubyCount += amount;if (rubyCount > 999) rubyCount = 999; RefreshRubys(); }
    public void takeBomb(int amount) { bombCount += amount;if (bombCount > 999) bombCount = 999; RefreshBomb(); }
    public override void addHealthPoint(int amount)
    {
        base.addHealthPoint(amount);
        RefreshHealth();
    }
    public override void addHealthMax(int amount)
    {
        base.addHealthMax(amount);
        RefreshHealth();
    }

    //public List<int> item_list = new List<int>();
    public void takeObject(int item_id)
    {
        //todo - gestion object
    }


    void Update()
    {
        //MOVEMENT
        if (flag_canMove)
        {
            int in_horizontal = Input.GetKey(KeyCode.Q) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
            int in_vertical = Input.GetKey(KeyCode.S) ? -1 : Input.GetKey(KeyCode.Z) ? 1 : 0;
            in_velocity = (new Vector3(in_horizontal, 0, 0) + new Vector3(0, 0, in_vertical)) * speed * Time.deltaTime;
            if (in_horizontal != 0) in_rotation = in_horizontal > 0 ? 270 : in_horizontal < 0 ? 90 : 0;
            else if (in_vertical != 0) in_rotation = in_vertical > 0 ? 180 : in_vertical < 0 ? 0 : 0;
            transform.position += in_velocity;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, in_rotation, 0), 0.3f);
        }

        // ATTACK
        if (flag_canAttack && Input.GetKey(KeyCode.F))
        {
            attack1();
        }
    }


    Vector3 lastValidPosition;
    /// <summary>
    /// RESPAWN UTILITY : Permet de réaparaître à une position valide (Pour plus tard)
    /// </summary>
    public void GetLastValidPosition()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 2.0f))
        {
            lastValidPosition = transform.position;
        }
    }

    private void FixedUpdate()
    {
    }

    public override void takeHit(int amount)
    {
        if (!flag_Invulnerable)
        {
            base.takeHit(amount);
            flag_Invulnerable = true;
            flag_canMove = false;
            hitRecovery = StartCoroutine(ActionTimer(() => { flag_Invulnerable = false; }, 1.0f));
            moveRecovery = StartCoroutine(ActionTimer(() => { flag_canMove = true; }, 0.2f));
            RefreshHUD();
        }

    }

    public void attack1()
    {
        //DO ANIMATION
        flag_canAttack = false;
        SwordAttackBox.SetActive(true);
        StartCoroutine(ActionTimer(() => { SwordAttackBox.SetActive(false); }, 0.1f));
        attackRecovery = StartCoroutine(ActionTimer(() => { flag_canAttack = true; }, 0.2f));
    }

    public override void onDeath()
    {
        base.onDeath();
        flag_canMove = false;
        MNG_Game.instance.pnl_gameover.SetActive(true);
    }

}

public class CTRL_EnemySoldier : Actor
{
    public NavMeshAgent nva;

    Coroutine moveRecovery;
    public bool flag_canMove = true;

    public GameObject onDeathSmoke;

    void Update()
    {



    }
    private void FixedUpdate()
    {
    }

    public override void takeHit(int amount)
    {
        base.takeHit(amount);
    }

    public void attack1()
    {
        //DO ANIMATION

    }

}