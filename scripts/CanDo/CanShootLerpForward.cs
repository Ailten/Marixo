using Godot;

public class CanShootLerpForward : CanShoot
{
    
	public CanShoot(CharacterBody2D owner, Node2D startMarker = null, Node2D endMarker = null)
	{
		this.owner = owner;
		this.startMarker = startMarker;
		this.endMarker = endMarker;
	}
}