using Svelto.Context;
using UnityEngine;

namespace Code
{
    public class MainContext : UnityContext<MainCompositionRoot>
    {
        
        
//        public Material mat;
//        public void CreateLineMaterial()
//        {
//            if (!mat)
//            {
//                Debug.Log("CREATEMATERIAL");
//                Shader shader = Shader.Find ("Sprites/Default");
//                mat = new Material (shader);
//            }
//        }
//
//        void OnRenderObject()
//        {
//                //Debug.Log("test");
//                CreateLineMaterial();
//                GL.PushMatrix();
//                mat.SetPass(0);
//                GL.LoadOrtho();
//                GL.Color(Color.red);
//                GL.Begin(GL.TRIANGLES);
//                GL.Vertex3(0.25F, 0.1351F, 0);
//                GL.Vertex3(0.25F, 0.3F, 0);
//                GL.Vertex3(0.5F, 0.3F, 0);
//                GL.End();
//                GL.Color(Color.yellow);
//                GL.Begin(GL.TRIANGLES);
//                GL.Vertex3(0.5F, 0.25F, -1);
//                GL.Vertex3(0.5F, 0.1351F, -1);
//                GL.Vertex3(0.1F, 0.25F, -1);
//                GL.End();
//                GL.PopMatrix();
//        }
    }
}