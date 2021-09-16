using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameEventsHandler : MonoBehaviour
{
    public static GameEventsHandler current;

    public const string PROJECTILE_DATA_PATH = "ProjectileData";

    public Dictionary<int, ShellParticleSystem> shellParticleDictionary = new Dictionary<int, ShellParticleSystem>();

    private void Awake()
    {
        current = Singleton<GameEventsHandler>.Instance;
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
        Debug.Log($"Shoot with ID : {e.projectileIndex}");
        var shellParticle = GetShellParticle(e.projectileIndex);

        shellParticle.SpawnShell(e.position, e.direction, 0f, Vector3.one, 0);
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
        var projectile = Resources.LoadAll<ProjectileData>(PROJECTILE_DATA_PATH).Where(p => p.item.id == index).FirstOrDefault();

        return projectile;
    }
}
