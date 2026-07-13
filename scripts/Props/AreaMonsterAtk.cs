using Godot;

public partial class AreaMonsterAtk : Area2D
{
	[Export]
	public Character monsterWhoAtk;
	[Export]
	public int damageMake;

	public override void _Ready()
	{
		BodyEntered += (Node2D body) =>
		{
			if (body is Character characterGetHit)
			{
				monsterWhoAtk.makeDamage(damageMake, characterGetHit);
			}
		};
	}
}
