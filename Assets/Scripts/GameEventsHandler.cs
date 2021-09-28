using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameEventsHandler : MonoBehaviour
{
    public static GameEventsHandler current;

    public const string PROJECTILE_DATA_PATH = "ProjectileData";

    public float feedbackTest = 20;
    public GameObject playerGo;

    public Dictionary<int, ShellParticleSystem> shellParticleDictionary = new Dictionary<int, ShellParticleSystem>();

    public event EventHandler<EventArgs> onLevelGenerated;

    private void Awake()
    {
        current = Singleton<GameEventsHandler>.Instance;

        Level.current.onGenerated += OnLevelGenerated;

        if (Level.current.IsGenerated())
        {
            OnLevelGenerated(this, new EventArgs());
        }
    }

    private void OnLevelGenerated(object sender, System.EventArgs e)
    {
        //TODO Instantiate player
        var coord = Level.current.GetRandomPositionInsideRoom();
        var pos = new Vector3(coord.x, coord.y);
        playerGo.transform.position = pos;

        onLevelGenerated?.Invoke(this, null);
    }


    public void SubcribeToShoot(ShootComponent shootComponent)
    {
        shootComponent.onShoot += OnShootEvent;
    }

    public void UnSubscribeToShoot(ShootComponent shootComponent)
    {
        shootComponent.onShoot -= OnShootEvent;
    }

    private void OnShootEvent(object sender, OnShootEventArgs e)
    {
        var shellParticle = GetShellParticle(e.projectileIndex);
        var projectileData = GetProjectileDataByIndex(e.projectileIndex);

        shellParticle.SpawnShell(e.shellPosition.position,
            e.shellPosition.up,
            Random.Range(0f, 360f),
            Random.Range(projectileData.shellParticleData.speed * 0.5f, projectileData.shellParticleData.speed),
            projectileData.shellParticleData.slowDown,
            new Vector3(0.5f, 0.25f) * 0.5f,
            0);

        SoundManager.PlaySound(projectileData.shotSound);

        //AddForceFeedback(-e.direction, e.damage * feedbackTest);
    }

    private void AddForceFeedback(Vector3 direction, float forceMagnitude)
    {
        var movement = playerGo.GetComponent<Movement>();

        Vector2 directionForce = direction.normalized * forceMagnitude;

        movement.AddForce(directionForce, ForceMode2D.Force);
    }

    private ShellParticleSystem GetShellParticle(int projectileId)
    {
        var success = shellParticleDictionary.TryGetValue(projectileId, out ShellParticleSystem shellParticleSystem);

        if (!success)
        {
            var projectileData = GetProjectileDataByIndex(projectileId);
            shellParticleSystem = ShellParticleSystem.Instantiate($"{projectileId}_ShellParticle", projectileData.shellParticleData);
            shellParticleDictionary.Add(projectileId, shellParticleSystem);
        }

        return shellParticleSystem;
    }

    public ProjectileData GetProjectileDataByIndex(int index)
    {
        var projectile = Resources.LoadAll<ProjectileData>(PROJECTILE_DATA_PATH).Where(p => p.GetId() == index).FirstOrDefault();

        return projectile;
    }
}
