using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using BehaviourTreeBase;
using Cells;

[RequireComponent(typeof(Seeker))]
public class AIController : MonoBehaviour, IAIKillerCellController, ICellController
{
	[Header("AI Data")]
	[SerializeField]
	private BehaviourTree tree = null;
	[SerializeField]
	private bool initialised = false;

	[Header("Interface Data (Cell controll)")]
	[SerializeField]
	private KillerCell controlledCell = null;
	public KillerCell ControlledCell { get => controlledCell; set => controlledCell = value; }

	[Header("Interface Data (Targeting) (ReadOnly)")]
	[SerializeField]
	private GameObject target = null;
	public GameObject Target { get => target; set => target = value; }
	[SerializeField]
	private float acceptableDistanceFromTarget = 1.0f;
	public float AcceptableDistanceFromTarget { get => acceptableDistanceFromTarget; set => acceptableDistanceFromTarget = value; }

	[Header("Interface Data (Movement) (ReadOnly)")]
	[SerializeField]
	private Vector2 movementDirection = Vector2.zero;
	public Vector2 MovementDirection { get => movementDirection; set => movementDirection = value; }
	private Vector2 zeroVector = Vector2.zero;
	[SerializeField]
	private Seeker pathSeeker = null;
	public Seeker PathSeeker { get => pathSeeker; set => pathSeeker = value; }

	[Header ("Interface Data (Helper Booking) (Read Only)")]
	[SerializeField]
	private HelperTCell bookedHelperTcell = null;
	public HelperTCell BookedHelperTCell { get => bookedHelperTcell; set => bookedHelperTcell = value; }
	[SerializeField]
	private GameObject bookingSpot = null;
	public GameObject BookingSpot { get => bookingSpot; set => bookingSpot = value; }


	public void Start()
	{
		pathSeeker = GetComponent<Seeker>();
		InitialiseBehaviourTree();
		controlledCell.controller = this;
		controlledCell.Sense.controller = this;

		initialised = true;
	}

	private void InitialiseBehaviourTree()
	{
		tree = new BehaviourTree();
		// ACTIONS
		BTActionNode hasToHeal = new AINeedToHealKillerCell("HasToHeal", tree, this);
		BTActionNode findClosestHelper = new AIBookHelperCellToReach("Booking HelperCell", tree, this);
		BTActionNode goToHeal = new AIReachDestination("Reach Healer", tree, this);
		BTActionNode canFight = new AICanFight("Can Fight", tree, this);
		BTActionNode findBase = new AIFindClosestTargetOfType<Tutorials.TutorialPopup>("Find Base", tree, this);
		BTActionNode goToBase = new AIReachDestination("Reach Base", tree, this);

		// GO AND HEAL
		List<BTNode> shouldHealList = new List<BTNode>();
		shouldHealList.Add(hasToHeal);
		shouldHealList.Add(findClosestHelper);
		shouldHealList.Add(goToHeal);
		BTSequence shouldHeal = new BTSequence("HealSequence", shouldHealList);

		// GO TO BASE
		List<BTNode> shouldGoToBaseList = new List<BTNode>();
		shouldGoToBaseList.Add(canFight);
		shouldGoToBaseList.Add(findBase);
		shouldGoToBaseList.Add(goToBase);
		BTSequence shouldGoToBase = new BTSequence("GoToBaseSequence", shouldGoToBaseList);

		// Set ROOT 
		List<BTNode> rootSelectorList = new List<BTNode>();
		rootSelectorList.Add(shouldHeal);
		rootSelectorList.Add(shouldGoToBase);
		tree.rootNode = new BTSelector("rootNode", rootSelectorList);
	}

	void Update()
	{
		if (!initialised) return;
		movementDirection = zeroVector;
		tree.Evaluate();
	}


	void FixedUpdate()
	{
		controlledCell.MovementVector = movementDirection;
	}

	public void OnCellDeath()
	{
		Debug.LogWarning("AI Controller found " + controlledCell + " to die");
	}

	public void OnEnemiesInRange()
	{
		Debug.LogWarning("AI Controller found " + controlledCell + " to reached enemies");

	}

	public void OnEnemiesOutOfRange()
	{
		Debug.LogWarning("AI Controller found " + controlledCell + " to went away from enemies");

	}
}
