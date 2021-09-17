using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshParticleSystem))]
public class ShellParticleSystem : MonoBehaviour
{
    [SerializeField]
    private MeshParticleSystem meshParticle;

    private List<Shell> shells = new List<Shell>();

    // Start is called before the first frame update
    void Awake()
    {
        if (meshParticle == null)
        {
            meshParticle = GetComponent<MeshParticleSystem>();
        }
    }

    public void SpawnShell(Vector3 position, Vector3 direction, float rotation, float speed, float slowDownFactor, Vector3 size, int uvIndex)
    {
        shells.Add(new Shell(position, direction, rotation, speed, slowDownFactor, size, uvIndex, meshParticle));
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < shells.Count; i++)
        {
            Shell shell = shells[i];

            shell.Update();

            if (shell.HasFinishedMovement())
            {
                shells.RemoveAt(i);
                i--;
            }
        }
    }

    public static ShellParticleSystem Instantiate(string name, MeshParticleData meshData)
    {
        GameObject shellSystemGo = new GameObject(name, typeof(ShellParticleSystem));

        Vector3 pos = new Vector3(0, 0, -5);

        shellSystemGo.transform.position = pos;

        var shellSystem = shellSystemGo.GetComponent<ShellParticleSystem>();

        shellSystem.meshParticle = shellSystemGo.GetComponent<MeshParticleSystem>();

        shellSystem.meshParticle.UpdateUV(meshData.material, meshData.uV_PixelsArray);

        return shellSystem;
    }

    public static ShellParticleSystem Instantiate(string name, MeshParticleData meshData, Vector3 position)
    {
        var shellSystem = Instantiate(name, meshData);

        shellSystem.gameObject.transform.position = position;

        return shellSystem;
    }

    private class Shell
    {
        private MeshParticleSystem meshParticleSystem;

        private Vector3 position;
        private Vector3 direction;
        private float rotation;
        private float speed;
        private int quadIndex;
        private Vector3 size;
        private int uvIndex;

        private float slowDownFactor;

        public Shell(Vector3 position, Vector3 direction, float rotation, float speed, float slowDownFactor, Vector3 size, int uvIndex, MeshParticleSystem meshParticleSystem)
        {
            this.position = position;
            this.direction = direction;
            this.rotation = rotation;
            this.speed = speed;
            this.slowDownFactor = slowDownFactor;
            this.size = size;
            this.meshParticleSystem = meshParticleSystem;
            this.uvIndex = uvIndex;
            CreateQuad();
        }

        private void CreateQuad()
        {
            quadIndex = meshParticleSystem.AddQuad(position, rotation, size, uvIndex);
        }

        public void Update()
        {
            position += direction * Time.deltaTime * speed;
            rotation += 360f * Time.deltaTime * speed;

            speed -= speed * slowDownFactor * Time.deltaTime;

            meshParticleSystem.UpdateQuad(quadIndex, position, rotation, size, uvIndex);
        }

        public bool HasFinishedMovement()
        {
            return speed < 0.1f;
        }
    }
}
