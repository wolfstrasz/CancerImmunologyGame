using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using BehaviourTreeBase;
using Cells;
using Pathfinding.RVO;

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
	[SerializeField]
	private float repathRate = 1.0f;
	public float RepathRate => repathRate;
	[SerializeField]
	private float movementLookAhead = 1.0f;
	public float MovementLookAhead => movementLookAhead;
	[SerializeField]
	private float slowdownDistance = 0.5f;
	public float SlowdownDistance => slowdownDistance;
	[SerializeField]
	private RVOController rvoController = null;
	public RVOController RVOController => rvoController;
	[SerializeField]
	private GameObject graphObstacle = null;
	public GameObject GraphObstacle => graphObstacle;
	
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
		tree = new BehaviourTree(); // Garbage collection will clean it if something requires reinitialisation

		BTSelector root = new BTSelector("Root", 2);
		{
			BTSequence healingState = new BTSequence("HealingState", 3);
			{
				BTSelector isCurrentlyInNeedOfHealing = new BTSelector("isCurrentlyInNeedOfHealing", 2);
				{
					// Currently healing actions
					BTActionNode hasAHealingTarget = new AIHasHealingTarget("hasAHealingTarget", tree, this);

					// Should Set healer target
					BTSequence shouldSetHealingTarget = new BTSequence("shouldSetHealingTarget", 2);
					{

						// CriticalCondition Selector
						BTSelector inCriticalCondition = new BTSelector("isInCriticalCondition", 2);
						{
							BTActionNode criticalHealth = new AIHealthConditional("LowHealth", tree, this, ValueConditionalOperator.LESS_THAN_EQUAL, 25f);
							BTActionNode criticalEnergy = new AIEnergyConditional("LowEnergy", tree, this, ValueConditionalOperator.LESS_THAN_EQUAL, 45f);
							inCriticalCondition.AddNode(criticalHealth);
							inCriticalCondition.AddNode(criticalEnergy);
						}

						BTActionNode setInitialHealerTarget = new AIBookHelperCellToReach("Booking HelperCell", tree, this);

						shouldSetHealingTarget.AddNode(inCriticalCondition);
						shouldSetHealingTarget.AddNode(setInitialHealerTarget);
					}

					isCurrentlyInNeedOfHealing.AddNode(hasAHealingTarget);
					isCurrentlyInNeedOfHealing.AddNode(shouldSetHealingTarget);
				}

				BTActionNode goToHealer = new AIReachDestination("Reach Healer", tree, this);

				BTSelector healingWaitOrGo = new BTSelector("Healing (Wait or Go)", 2);
				{
					BTSequence readyToFight = new BTSequence("StopHealing", 3);
					{
						BTActionNode perfectHealth = new AIHealthConditional("perfectHealth", tree, this, ValueConditionalOperator.MORE_THAN, 75f);
						BTActionNode perfectEnergy = new AIEnergyConditional("perfectEnergy", tree, this, ValueConditionalOperator.MORE_THAN, 75f);
						BTActionNode freeHealingTarget = new AIReleaseHelperTarget("ReleaseHealingTarget", tree, this);

						readyToFight.AddNode(perfectHealth);
						readyToFight.AddNode(perfectEnergy);
						readyToFight.AddNode(freeHealingTarget);
					}

					BTActionNode waitToHeal = new AIWait("WaitToHeal", tree, 100f, false);
					healingWaitOrGo.AddNode(readyToFight);
					healingWaitOrGo.AddNode(waitToHeal);
				}

				healingState.AddNode(isCurrentlyInNeedOfHealing);
				healingState.AddNode(goToHealer);
				healingState.AddNode(healingWaitOrGo);
			}


			// Should be last
			BTSequence goToBaseSequence = new BTSequence("GoingToBase", 2);
			{
				BTActionNode findBase = new AIFindClosestTargetOfType<Tutorials.TutorialPopup>("Find Base", tree, this);
				BTActionNode goToBase = new AIReachDestination("Reach Base", tree, this);

				goToBaseSequence.AddNode(findBase);
				goToBaseSequence.AddNode(goToBase);
			}



			root.AddNode(healingState);
			root.AddNode(goToBaseSequence);
		}


		tree.rootNode = root;
	}

	void Update()
	{
		if (!initialised) return;
		movementDirection = zeroVector;
		tree.Evaluate();
		controlledCell.MovementVector = movementDirection;
	}


	void FixedUpdate()
	{
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
