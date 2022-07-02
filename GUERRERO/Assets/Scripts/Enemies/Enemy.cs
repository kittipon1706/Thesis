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

    public virtual void Initialize(string Model_Name,Vector3 scale, string ani,string avatar,string BE_name,string name,int hp)
    {
        monsName = name;
        healthPoint = hp;
        if (Model == null)
        {
            Model = Instantiate(Resources.Load<GameObject>("Art/3D/Enemies/Goblin/" + Model_Name), gameObject.transform.position , Quaternion.identity);
            Model.transform.localScale = scale;
            Model.transform.SetParent(gameObject.transform);
        }
        GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Art/3D/Enemies/Goblin/" + ani);
        GetComponent<Animator>().avatar = Resources.Load<Avatar>("Art/3D/Enemies/Goblin/" + avatar);
    }
}

