using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private Team team;
    [SerializeField]
    private Unit target;
    [SerializeField]
    private float health;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float radius;
    [SerializeField]
    private float attack;

    [Header("References")]
    [SerializeField]
    private NavMeshAgent navMeshAgent;
    [SerializeField]
    private Renderer unitRenderer;
    [SerializeField]
    private LineRenderer lineRenderer;

    public delegate void UnitDestroyHandle(Unit unit, Team team);
    public static event UnitDestroyHandle UnitDestroyEvent;

    private void Start()
    {
        navMeshAgent.speed = speed;
    }

    //≈сли у юнита есть цель, но рассто€ние до нее больше радиуса атаки, то цель становитс€ назначением дл€ NavMeshAgent и юнит начинает двигатьс€ к ней.
    //≈сли рассто€ние меньш радиуса атаки, то юнит останавливаетс€ и начинает атаковать цель.
    private void Update()
    {
        if (target != null)
        {
            Debug.DrawLine(transform.position + Vector3.up, target.transform.position + Vector3.up);
            if (Vector3.Distance(transform.position, target.transform.position) >= radius)
            {
                navMeshAgent.destination = target.transform.position;
                lineRenderer.enabled = false;
            }
            else
            {
                navMeshAgent.destination = transform.position;
                lineRenderer.SetPosition(0, transform.position + Vector3.up);
                lineRenderer.SetPosition(1, target.transform.position + Vector3.up);
                target.Health -= attack * Time.deltaTime;
                lineRenderer.enabled = true;
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    //Get-Set дл€ полей, значение которых мен€етс€ другими классами.
    public Unit Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
        }
    }

    public Renderer UnitRenderer
    {
        get
        {
            return unitRenderer;
        }
        set
        {
            unitRenderer = value;
        }
    }
    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if (health <= 0)
            {
                UnitDestroyEvent(this, team);
                Destroy(gameObject);
            }
        }
    }
}