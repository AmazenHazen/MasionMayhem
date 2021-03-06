﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : CharacterMovement
{
    #region Additional Movement Variables
    // Variables for enemy targeting
    private GameObject player;
    private bool readyToMove;
    private bool resettingMovement;

    // Attributes for CalcSteeringForces Method


    [Header("Path Following")]
    public List<GameObject> pathPoints;
    private int currentPathPoint;

    [Header("Seperation Forces")]
    // Variables for seperation force
    public float seperationBubble; // basic force is 1.0
    public float seperationForce; // basic seperation force is 1.5 for enemies, 4 for allies

    // Current enemy phase
    int phase;
    #endregion

    #region Start Method
    public override void Start()
    {
        //maxSpeed = 6;
        minSpeed = .25f;

        base.Start();

        readyToMove = true;
        resettingMovement = false;
        player = GameObject.FindGameObjectWithTag("player");
    }
    #endregion

    #region Update Method
    protected override void Update()
    {
        base.Update();
        phase = gameObject.GetComponent<EnemyManager>().Phase;

    }
    #endregion

    #region Enemy Movement Method
    // Call the necessary Forces on the enemy
    protected override void CalcSteeringForces()
    {
        // Create a new ultimate force that is zeroed out
        Vector3 ultimateForce = Vector3.zero;

        // Apply forces to the enemy
        if ((player.transform.position - transform.position).magnitude < awareDistance)
        {
            // Have the enemy face the player
            Rotate();

            // Basic Enemy Movement
            switch (gameObject.GetComponent<EnemyManager>().monster)
            {
                #region spiders
                case enemyType.smallSpider:
                    // Seek
                    ultimateForce += seek(player.transform.position);
                    ultimateForce += Seperation();
                    break;
                case enemyType.blackWidow:
                case enemyType.redTermis:
                case enemyType.silkSpinnerSpider:
                case enemyType.spiderQueen:
                    ultimateForce += pursue(player);
                    ultimateForce += Seperation();
                    break;
                case enemyType.tarantula:
                    // No movement
                    break;
                case enemyType.wolfSpider:
                    // Jumping Movement
                    if (readyToMove)
                    {
                        ultimateForce += seek(player.transform.position) * 50;
                        readyToMove = false;
                        resettingMovement = true;
                    }
                    else if (resettingMovement)
                    {
                        resettingMovement = false;
                        Invoke("ResetMoveBool", 1);
                    }
                    break;
                case enemyType.fatalCrimson:
                    if (readyToMove)
                    {
                        ultimateForce += seek(player.transform.position) * 100;
                        readyToMove = false;
                        resettingMovement = true;
                    }
                    else if (resettingMovement)
                    {
                        resettingMovement = false;
                        Invoke("ResetMoveBool", 1);
                    }
                    break;
                #endregion

                #region ghosts
                // Ghosts
                case enemyType.basicGhost:
                case enemyType.banshee:
                case enemyType.ghosthead:
                    ultimateForce += seek(player.transform.position);
                    ultimateForce += Seperation();
                    break;
                case enemyType.ghostknight:
                    ultimateForce += seek(player.transform.position);
                    if (player.GetComponent<PlayerManager>().IsSprinting)
                    {
                        maxSpeed = 9f;
                    }
                    else
                    {
                        maxSpeed = 6.5f;
                    }
                    break;

                case enemyType.wraith:
                    ultimateForce += pursue(player);

                    // Jumping Movement
                    if ((player.transform.position - transform.position).magnitude < 3f)
                    {
                        //maxSpeed = 5.5f;
                        if (readyToMove)
                        {
                            ultimateForce += pursue(player) * 50;
                            readyToMove = false;
                            resettingMovement = true;
                        }
                        else if (resettingMovement)
                        {
                            resettingMovement = false;
                            Invoke("ResetMoveBool", 5);

                        }
                    }
                    break;

                case enemyType.bansheeMistress:

                    break;

                case enemyType.prisonLeader:
                    // First Phase: Stationary summoning ghost heads
                    // Do nothing here for movement

                    // Second Phase: Seek Player and throwing weights
                    if (phase == 1)
                    {
                        // Seek
                        ultimateForce += seek(player.transform.position);
                    }
                    if (phase == 2)
                    {
                        // Seek
                        ultimateForce += pursue(player);
                    }

                    // Third Phase he gets faster
                    if (phase == 2 && maxSpeed != 7)
                    {
                        mass = 1.5f;
                        maxSpeed = 7.7f;
                    }


                    break;
                #endregion

                #region demons
                // Demons
                case enemyType.imp:
                case enemyType.darkimp:
                case enemyType.bloodDemon:
                    // Seek
                    ultimateForce += seek(player.transform.position);
                    ultimateForce += Seperation();
                    break;
                case enemyType.gargoyle:
                    ultimateForce += seek(player.transform.position);
                    ultimateForce += Seperation();
                    break;

                case enemyType.boneDemon:

                    break;

                case enemyType.corruptedDemon:

                    break;

                case enemyType.infernalDemon:

                    break;

                case enemyType.shadowDemon:

                    break;

                case enemyType.slasherDemon:

                    break;

                case enemyType.spikeDemon:

                    break;

                case enemyType.hellhound:

                    break;

                case enemyType.fury:

                    break;

                case enemyType.demonLord:

                    break;
                case enemyType.cerberus:

                    break;

                #endregion

                #region zombies
                case enemyType.crawlingHand:
                case enemyType.crawlingZombie:
                case enemyType.basicZombie:
                case enemyType.runnerZombie:
                case enemyType.spitterZombie:
                case enemyType.fatZombie:
                case enemyType.tankZombie:
                    ultimateForce += seek(player.transform.position);
                    ultimateForce += Seperation();
                    break;
                case enemyType.stalkerZombie:
                    ultimateForce += seek(player.transform.position);
                    ultimateForce += Seperation();

                    // Jumping Movement
                    if ((player.transform.position - transform.position).magnitude < 1.75f)
                    {
                        //maxSpeed = 5.5f;
                        if (readyToMove)
                        {
                            ultimateForce += seek(player.transform.position) * 75;
                            readyToMove = false;
                            resettingMovement = true;
                        }
                        else if (resettingMovement)
                        {
                            resettingMovement = false;
                            Invoke("ResetMoveBool", 3);
                        }
                    }
                    break;
                case enemyType.gasZombie:
                    ultimateForce += pursue(player);
                    ultimateForce += Seperation();

                    // Jumping Movement
                    if ((player.transform.position - transform.position).magnitude < 1.75f)
                    {
                        //maxSpeed = 5.5f;
                        if (readyToMove)
                        {
                            ultimateForce += seek(player.transform.position) * 50;
                            readyToMove = false;
                            resettingMovement = true;
                        }
                        else if (resettingMovement)
                        {
                            resettingMovement = false;
                            Invoke("ResetMoveBool", 3);

                        }
                    }
                    break;
                case enemyType.zombiehordeLeader:
                    ultimateForce += pursue(player);
                    break;




                #endregion

                #region skeletons
                case enemyType.basicSkeleton:
                case enemyType.skeleHand:
                case enemyType.eliteSkeleArcher:
                case enemyType.eliteSkeleMage:
                case enemyType.eliteSkeleWarrior:
                    ultimateForce += seek(player.transform.position);
                    break;

                case enemyType.archerSkeleton:

                    break;

                case enemyType.warriorSkeleton:
                    ultimateForce += seek(player.transform.position);
                    break;

                case enemyType.mageSkeleton:

                    break;

                case enemyType.giantSkeleton:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.skeletonDragon:
                    ultimateForce += seek(player.transform.position);
                    break;

                case enemyType.necromancer:

                    break;

                #endregion

                #region Muck
                case enemyType.oilSlime:
                case enemyType.bloodSlime:
                case enemyType.phlegmSlime:
                case enemyType.mucusSlime:
                    // Jumping Movement
                    if (readyToMove)
                    {
                        ultimateForce += seek(player.transform.position) * 50;
                        readyToMove = false;
                        resettingMovement = true;
                    }
                    else if (resettingMovement)
                    {
                        resettingMovement = false;
                        Invoke("ResetMoveBool", 1);
                    }
                    break;

                case enemyType.mucusMuck:
                case enemyType.phlegmMuck:
                case enemyType.bloodMuck:
                case enemyType.oilMuck:
                case enemyType.BeastoftheFourBiles:
                    ultimateForce += seek(player.transform.position);
                    break;

                case enemyType.ectoplasmMuck:
                    ultimateForce += seek(player.transform.position);
                    break;


                case enemyType.purpleSludgeMuck:
                    ultimateForce += seek(player.transform.position);
                    break;

                #endregion

                #region shadows
                case enemyType.shadeKnight:

                    break;

                case enemyType.shadow:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.shadowBehemoth:

                    break;
                #endregion

                #region Elementals
                case enemyType.infernalElemental:

                    break;

                case enemyType.blackFireElemental:

                    break;

                case enemyType.acidicElemental:

                    break;

                case enemyType.pyreLord:

                    break;
                case enemyType.dreorsProxy:
                    ultimateForce += pursue(player);
                    break;

                #endregion

                #region Beasts
                case enemyType.shadowBeast:

                    break;
                case enemyType.flameBeast:

                    break;
                case enemyType.boneBeast:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.bloodBeast:

                    break;

                #endregion

                #region bats
                case enemyType.basicBat:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.giantBat:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.bloodBat:
                    ultimateForce += seek(player.transform.position);
                    break;
                case enemyType.giantBloodBat:
                    ultimateForce += seek(player.transform.position);
                    break;

                #endregion

                #region other
                #endregion

                #region default monster
                default:

                    break;

                    #endregion

            }

        }
        else
        {
            //ultimateForce = wander();
        }
        // Apply Decelleration using ApplyFriction Force
        //ultimateForce += ApplyFriction(3.0f);

        //Debug.Log("Before Clamp: " + ultimateForce);
        // Clamp the ultimate force by the maximum force
        Vector3.ClampMagnitude(ultimateForce, maxForce);

        // Ensure that the enemies do not move in the z-axis
        ultimateForce.z = 0;

        //Debug.Log("After Clamp: " + ultimateForce);
        ApplyForce(ultimateForce);
    }
    #endregion

    #region Enemy Rotate
    /// <summary>
    /// Rotates the player based on the direction its facing
    /// </summary>
    protected override void Rotate()
    {
        Vector3 targetPosition = player.transform.position;
        Vector3 dir = targetPosition - transform.position;
        angleOfRotation = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90;
    }
    #endregion

    #region Advanced Movement Methods
    /// <summary>
    /// A delay for a movement method.
    /// Helps for rapid movement monsters
    /// </summary>
    void ResetMoveBool()
    {
        //Debug.Log("Reset");
        readyToMove = true;
        resettingMovement = false;
    }

    /// <summary>
    /// Seperation Method that returns a steering force to move an enemy away from another enemy if too close.
    /// </summary>
    /// <returns></returns>
    public Vector3 Seperation()
    {
        // Create a new steering force
        Vector3 steeringForce = Vector3.zero;

        // Find nearest neighbor
        foreach (GameObject enemy in LevelManager.enemies)
        {
            if ((transform.position - enemy.transform.position).magnitude < seperationBubble)
            {
                if ((transform.position - enemy.transform.position).magnitude != 0)
                {
                    // Step 1: Find Desired Velocity
                    // This is the vector pointing from my target to my myself
                    Vector3 desiredVelocity = position - enemy.transform.position;

                    // Step 2: Scale Desired to maximum speed
                    //         so I move as fast as possible
                    desiredVelocity.Normalize();
                    desiredVelocity *= seperationForce;

                    // Step 3: Calculate your Steering Force
                    steeringForce = desiredVelocity - velocity;
                }

            }
        }
        return steeringForce;
    }

    


    /// <summary>
    /// Path following method that returns a steering force based off of the current and future path point
    /// the character is going towards.
    /// </summary>
    /// <returns></returns>
    public Vector3 pathfollow()
    {
        // If the enemy unit gets close enough to the path point it advances the current path point
        if ((transform.position - pathPoints[currentPathPoint].transform.position).magnitude <= .1f)
        {
            // Get the next hole number and set it as the new currentPathPoint
            int nextHoleNum = currentPathPoint + 1;
            nextHoleNum %= pathPoints.Count;
            currentPathPoint = nextHoleNum;
            velocity = Vector3.zero;
        }

        // Step 3: Seek the new wandered position and add it to the ultimate force
        return seek(pathPoints[currentPathPoint].transform.position);
    }

    #endregion

    #region Revert Speed Method for Enemies
    /// <summary>
    /// Returns Speed to Max Speed
    /// </summary>
    protected override void RevertSpeed()
    {
        // Reset speed if you are slowed
        if (currentSpeed < maxSpeed && beingSlowed == false)
        {
            currentSpeed += .05f;
        }

        //Reset speed if on slippery surface
        if (currentSpeed > maxSpeed && beingSped == false)
        {
            currentSpeed -= .05f;
        }

        // Don't allow speed to be negative or 0
        if (currentSpeed < .25f)
        {
            currentSpeed = .25f;
        }

        // Don't allow speed to be too high
        if (currentSpeed > 6f)
        {
            currentSpeed = 6f;
        }
    }
    #endregion
}
