using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEditor;

[System.Flags]
public enum Team { Neutral, Player, Monster }

public class Actor : MonoBehaviour
{
    public Rigidbody bdy;
    public Team TeamEnum;
    public int health = 4;
    public int health_max = 2;
    const int health_max_limit = 12;
    public float speed = 4;
    public float turnTowardSpeed = 0.1f;

    protected virtual void Awake()
    {
        if (bdy == null) bdy = GetComponent<Rigidbody>();
    }
    protected virtual void Start()
    {
    }
    public virtual bool addHealthPoint(int amount)
    {
        if (health_max * 2 <= health) return false;
        health += amount;
        if (health > health_max * 2) health = health_max * 2;
        print(name + " : regen " + amount + "hp");
        return true;
    }
    public virtual bool addHealthMax(int amount)
    {
        if (health_max >= health_max_limit)
        {
            health += amount * 2;
            if (health > health_max * 2) health = health_max * 2;
            print(name + " : no extra heart (+2hp)");
        }
        else
        {
            health += amount * 2;
            health_max += amount;
            print(name + " : add " + amount + " extra heart");
        }
        return true;
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
        print(name + " : Dead !");
        Instantiate(MNG_Game.instance.fx_deathSmoke, transform.position, Quaternion.identity);
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Trigger")
        {
            other.GetComponent<TRG>().trigger(this, other);
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

    public AANIM_Humanoid aanim;

    Coroutine hitRecovery;
    public bool flag_Invulnerable = false;

    Coroutine attackRecovery;
    public bool flag_canAttack = true;
    public bool flag_isAttacking = false;

    Coroutine moveRecovery;
    public bool flag_canMove = true;

    public GameObject SwordAttackBox;

    public int rubyCount = 0;
    public int bombCount = 0;
    protected override void Awake()
    {
        base.Awake();
        if (aanim == null) aanim = GetComponent<AANIM_Humanoid>();

    }

    protected override void Start()
    {
        base.Start();
        TeamEnum = Team.Player;
        InvokeRepeating("GetLastValidPosition", 0.5f, 0.2f);
        RefreshHUD();
        Camera.main.GetComponent<SmoothFollow>().target = this.transform;

        //Time.timeScale = 0.1f;
    }

    void RefreshHUD() { RefreshHealth(); RefreshBomb(); RefreshRubys(); }
    void RefreshHealth() { MNG_Game.instance.hud.Refresh_Health(health, health_max); }

    //GESTION ITEM COLLECTABLES
    void RefreshBomb() { MNG_Game.instance.hud.Refresh_Bomb(bombCount); }
    void RefreshRubys() { MNG_Game.instance.hud.Refresh_Rubys(rubyCount); }
    public void takeRuby(int amount) { rubyCount += amount; if (rubyCount > 999) rubyCount = 999; RefreshRubys(); }
    public void takeBomb(int amount) { bombCount += amount; if (bombCount > 999) bombCount = 999; RefreshBomb(); }
    /*MODIFIE███████████████████████████████████████████████████████████████████████████*/
    public override bool addHealthPoint(int amount)
    {
        bool pop = base.addHealthPoint(amount);
        RefreshHealth();
        return pop;
    }
    public override bool addHealthMax(int amount)
    {
        bool pop = base.addHealthMax(amount);
        RefreshHealth();
        return pop;
    }

    //public List<int> item_list = new List<int>();
    public void takeObject(int item_id)
    {
        //todo - gestion object
    }


    void Update()
    {
        if (MNG_Game.instance.inPause) return;

        //PAUSING 
        if (Input.GetKey(KeyCode.Escape)) MNG_Game.instance.setPause(true);

        //MOVEMENT
        if (flag_canMove)
        {
            Vector3 in_direction = Vector3.zero;
            int in_horizontal = Input.GetKey(KeyCode.Q) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
            int in_vertical = Input.GetKey(KeyCode.S) ? -1 : Input.GetKey(KeyCode.Z) ? 1 : 0;
            in_direction = (new Vector3(in_horizontal, 0, 0) + new Vector3(0, 0, in_vertical)).normalized;

            if (aanim.state != AANIM_Humanoid.MyEnum.attack)
                if (in_horizontal == 0 && in_vertical == 0) aanim.state = AANIM_Humanoid.MyEnum.idle;
                else aanim.state = AANIM_Humanoid.MyEnum.running;

            //MOVE
            transform.position += in_direction * speed * Time.deltaTime;

            //ROTATION
            Quaternion dirRotation = Quaternion.FromToRotation(transform.forward, in_direction);
            transform.rotation = Quaternion.Lerp(
                Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y + 0.1f, 0f)) // FIX = (in_horizontal!=0 && in_vertical!=0?1f:0) = Rotation Y
                , Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * dirRotation
                , turnTowardSpeed);
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
        /*if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), Vector3.down, out _, 4.0f, 0, QueryTriggerInteraction.Ignore))
        {
            print("AAAAAAA");
            lastValidPosition = transform.position;
        } */
    }
    public void RespawnToLastValidPosition()
    {
        print("[PLAYER] RESPAWN");
        takeHit(1);
        transform.position = lastValidPosition;
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
        flag_isAttacking = true;
        flag_canMove = false;
        SwordAttackBox.SetActive(true);
        aanim.state = AANIM_Humanoid.MyEnum.attack;
        attackRecovery = StartCoroutine(ActionTimer(() => {
            flag_canMove = true;
            flag_canAttack = true;
            aanim.state = AANIM_Humanoid.MyEnum.idle;
            SwordAttackBox.SetActive(false);
            flag_isAttacking = false;
        }, 0.6f));
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