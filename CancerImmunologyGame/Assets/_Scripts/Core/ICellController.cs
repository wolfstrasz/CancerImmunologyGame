namespace ImmunotherapyGame
{
	public interface ICellController :
		ICellDeathObserver,
		ICellEnemiesDetector
	{ }

	public interface ICellDeathObserver
	{
		void OnCellDeath();
	}

	public interface ICellEnemiesDetector
	{
		void OnEnemiesInRange();
		void OnEnemiesOutOfRange();
	}
}