using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ConveyorMode
{
    MaterialOffset, ShaderOffset, MeshAnimation
}
public enum RotationDirection
{
    Identity, None, Backwards
}

public class ConveyorBelt : Effector
{
    public MeshRenderer BeltRenderer;
    public float Acceleration;
    public float Deceleration;
    float currentAccel = 0;
    public float MaxSpeed;
    public float AnimationSpeed;
    public float SpeedFactor;
    public bool LockRotation;
    public ConveyorMode AnimationMode;
    public RotationDirection RotationDirection;
    private float currentSpeed;
    bool BeltIsRunning = false;
    //Shader BeltShader;
    Vector2 BeltVelocity = Vector2.zero;
    private void Start()
    {
        StartCoroutine(RunBelt());
    }

    public override void Interaction(Rigidbody rbToAffect)
    {
        if (LockRotation) { 
            rbToAffect.freezeRotation = true;
            switch (RotationDirection)
            {
                case RotationDirection.Identity:
                    rbToAffect.transform.rotation = Quaternion.identity;
                    break;
                case RotationDirection.None:
                    break;
                case RotationDirection.Backwards:
                    rbToAffect.transform.rotation = Quaternion.Euler(-Quaternion.identity.eulerAngles);
                    break;
            }
        }
        rbToAffect.velocity = transform.forward * ((currentSpeed * SpeedFactor) / 3.5f); 
        // 3.5f is a hardcoded value, doesn't really matter, but it made speed vs force more in sync without changing speedfactor,
        // Leaving that free as an option for changing the "power output" of the conveyorbelt (Heavier objects could move slower?)
        // More testing in other scenarios is needed to tweak and/or remove the value

    }

    public override void ExitInteraction(Rigidbody rbToAffect)
    {
        if (LockRotation) rbToAffect.freezeRotation=false;
    }

    public IEnumerator RunBelt()
    {
        BeltIsRunning=true;
        while (BeltIsRunning)
        {
            /*
            if(currentSpeed != MaxSpeed)
            {
                // currentAccel Gets clamped between 0 and 1. In between, it should increase exponentially before leveling out when nearing it's top value.
                currentAccel = Mathf.Clamp(currentAccel + (Acceleration  * Time.deltaTime * (currentAccel+0.1f)), 0, 1);
                currentSpeed = Mathf.Lerp(currentSpeed, MaxSpeed, currentAccel);
                // The resulting currentSpeed should now be Lerped frame-rate independantly, with a controllable output curve (Now a kind of S curve, roughly approximating real acceleration I hope)
            }
            */
            // Adjust acceleration or deceleration depending on whether we're speeding up or slowing down

            if(currentSpeed != MaxSpeed)
            {
                if (currentSpeed < MaxSpeed)
                {
                    // Accelerating towards higher MaxSpeed
                    currentAccel = Mathf.Clamp(currentAccel + (Acceleration * Time.deltaTime * (currentAccel + 0.1f)), 0, 1);
                }
                else
                {
                    // Decelerating towards lower MaxSpeed
                    currentAccel = Mathf.Clamp(currentAccel - (Deceleration * Time.deltaTime * (1 - currentAccel + 0.1f)), 0, 1);
                }

                // Smoothly interpolate currentSpeed toward MaxSpeed using currentAccel
                currentSpeed = Mathf.Lerp(currentSpeed, MaxSpeed, currentAccel);
            }


            BeltVelocity = new Vector2(BeltVelocity.x, BeltVelocity.y+(currentSpeed * AnimationSpeed / 100));

            // Div by 100: Another hardcoded value, texture scroll-speed is way too high, lowered it with hardcoded variable.
            // No real point in accessing this as a variable, that can be done through acceleration, currentSpeed or BeltVelocity
            if(AnimationMode == ConveyorMode.MaterialOffset)
            {
                // Uses the built in SetTextureOffset in the standard shader to manipulate the material offset, unsure how it works under the hood
                BeltRenderer.material.SetTextureOffset("_MainTex", BeltVelocity);
            }
            else if(AnimationMode == ConveyorMode.ShaderOffset)
            {
                // Uses my custom shader which currently only has one Float; _MainTexOffset, which directly sets the UV's y coordinate.
                // This shader currently does nothing else but setting the texture.
                
                BeltRenderer.material.SetFloat("_MainTexOffset", BeltVelocity.y);
            }
            else if(AnimationMode == ConveyorMode.MeshAnimation)
            {

            }
            yield return new WaitForEndOfFrame();
        }

    }

    public void StopBelt()
    {
        BeltIsRunning = false;
        // This is a good spot for running a Deccelerate function to controll the speed and velocity output when turning off the belt.
        // For now, we just stop the coroutine to stop the belt. BeltIsRunning = false should also stop the while-loop, but I've been betrayed by while loops in the past
        
        StopCoroutine(RunBelt());
    }
}
