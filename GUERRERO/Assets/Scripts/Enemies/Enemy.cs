using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;
public class Enemy<T> where T : Enemy
{
    public GameObject GameObject;
    public T ScriptComponant;

    public Enemy(string name)
    {
        //GameObject = new GameObject(name);
        ScriptComponant = GameObject.AddComponent<T>();
    }
}

public abstract class Enemy : MonoBehaviour
{
    public string monsName;
    public Rigidbody Body;
    public NavMeshAgent navmesh;
    public BehaviorTree BE;
    public Animator Animator;
    public CapsuleCollider Collider;
    public GameObject Model;
    public int healthPoint;
    //Waypoint

    //protected abstract void AttackPattern();

    private void Awake()
    {
        //Add common componants
        Body = gameObject.AddComponent<Rigidbody>();
        Body.isKinematic = true;
        Collider = gameObject.AddComponent<CapsuleCollider>();
        navmesh = gameObject.AddComponent<NavMeshAgent>();
        BE = gameObject.AddComponent<BehaviorTree>();
        //Set common Model
        Body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Animator = gameObject.AddComponent<Animator>();

        gameObject.tag = "Enemy";
        gameObject.layer = LayerMask.NameToLayer("Enemies");


    }

    protected virtual void Initialize(int index,Vector3 scale,string BE_name,string name,int hp)
    {
        monsName = name;
        healthPoint = hp;
        if (Model == null)
        {
            Model = Instantiate(EnemyFactory.Instance.EnemyLists[index].model, gameObject.transform.position, Quaternion.identity);
            Model.transform.localScale = scale;
            Model.transform.SetParent(gameObject.transform);
        }
        GetComponent<Animator>().runtimeAnimatorController = EnemyFactory.Instance.EnemyLists[index].animator;
        GetComponent<Animator>().avatar = EnemyFactory.Instance.EnemyLists[index].avatar;
    }
}

