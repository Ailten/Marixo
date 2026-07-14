using Godot;

public partial class Character : CharacterBody2D
{
	protected AnimatedSprite2D animatedSprite;
	private bool _isLookAtRight = true;
	protected bool isLookAtRight
	{
		get => _isLookAtRight;
		set
		{
			if (_isLookAtRight != value)
			{
				_isLookAtRight = value;
				applyFlipH();
			}
		}
	}

	protected float intencityVelocityTaken = 0.95f;

    public override void _Ready()
    {
        // get and set default value to AnimatedSprite2D child.
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        animatedSprite.FlipH = false;
        animatedSprite.Play("default");

        canBeHit = new CanBeHit(this);
	}

	// ------>

	protected virtual void applyFlipH()
	{
		animatedSprite.FlipH = !isLookAtRight;
	}

	protected void evalFlipH(float moveH) {
		if (moveH > 0f ^ isLookAtRight)
		{
			isLookAtRight = !isLookAtRight;
			applyFlipH();
		}
	}

	protected Vector2 getVelocity()
	{
		return Velocity * intencityVelocityTaken;
	}

	protected void applyVelocity(Vector2 velocity)
	{
		Velocity = velocity;
		MoveAndSlide();
	}

    // ------>

    protected int hp = 1;
    protected int maxHp = 1;
    private int hpLeft
    {
        get => maxHp - hp;
    }
    protected int setMaxHp 
    {
        set { 
            maxHp = value;
            hp = value;
        }
    }
    public bool isLiving
    {
        get => hp > 0;
    }
    protected CanBeHit canBeHit;

    public void refillLive()
    {
        heal(maxHp);
    }

    public bool heal(int heal)
    {
        if (hp == maxHp)
            return false;
        if (heal <= 0)
            return false;

        hp = Mathf.Clamp(hp + heal, 0, maxHp);

        return true;
    }

    public virtual bool takeDamage(int damage, Character damageMaker=null, bool isCheckDeath=true)
    {
        if (canBeHit.isCooldownDamaged)
            return false;
        if (damage <= 0)
            return false;

        hp = Mathf.Clamp(hp - damage, 0, maxHp);

        if (hp == 0 && isCheckDeath)
            death(damageMaker);

        canBeHit.beHit(damageMaker);  // set time world hit (for cooldown invu).

        return true;
    }

    public void makeDamage(int damage, Character target)
    {
        target.takeDamage(damage, this);
    }

    public virtual void death(Character killer=null)
    {
        QueueFree();
    }

}
