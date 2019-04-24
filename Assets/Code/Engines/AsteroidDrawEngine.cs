using System.Collections;
using Code.Structs;
using Code.Views;
using Svelto.ECS;
using Svelto.Tasks.ExtraLean;
using UnityEngine;

namespace Code.Engines
{
    public class AsteroidDrawEngine : IQueryingEntitiesEngine
    {
        public IEntitiesDB entitiesDB { get; set; }
        public void Ready()
        {
            Render().RunOn(StandardSchedulers.renderScheduler);
        }


        private Material mat;

        private void CreateLineMaterial()
        {
            if (mat) return;
            var shader = Shader.Find ("Sprites/Default");
            mat = new Material (shader);
        }

        
        IEnumerator Render()
        {
            while (true)
            {
                CreateLineMaterial();
                GL.PushMatrix();
                mat.SetPass(0);
                var ast = entitiesDB.QueryEntities<AsteroidInfoEntityStruct, TransformEntityViewStruct>(ECSGroups.Asteroid, out var count);
                for (int i = 0; i < count; i++)
                {
                    var asteroidPoints = ast.Item1[i].Points;
                    var pos = ast.Item2[i].TransformComponent.position;
                    var rot = ast.Item2[i].TransformComponent.rotation;
                    GL.MultMatrix(ast.Item2[i].TransformComponent.matrix);
                    GL.Begin(GL.LINE_STRIP);
                    foreach (var point in asteroidPoints)
                    {
                        GL.Vertex(point);
                    }
                    GL.Vertex(asteroidPoints[0]);
                    GL.End();
                }
                GL.PopMatrix();
                yield return null;
            }
        }
    }
}