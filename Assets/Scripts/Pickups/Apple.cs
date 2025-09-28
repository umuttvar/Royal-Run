using UnityEngine;

public class Apple : Pickup
{
    [SerializeField] float gainSpeedAmount = 3f;

    LevelGenerator levelGenerator;

    public void Init(LevelGenerator levelGenerator)
    {
        
        this.levelGenerator = levelGenerator; //dependency injenction
    }
    protected override void OnPickup()
    {
        levelGenerator.ChangeChunkMovespeed(gainSpeedAmount);
        
    }
}
