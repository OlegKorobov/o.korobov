using UnityEngine;

using UnityEditor;
using UnityEngine.AI;
using Unity.AI.Navigation;

namespace Unity.Tutorials
{
    /// <summary>
    /// Implement your Tutorial callbacks here.
    /// </summary>
    public class TutorialCallbacks : ScriptableObject
    {
        public Unity.Tutorials.Editor.FutureObjectReference futureRoomInstance = default;
        public Unity.Tutorials.Editor.FutureObjectReference futureBotInstance = default;
        NavMeshSurface navMeshSurface = default;

        public bool NavMeshIsBuilt()
        {
            return navMeshSurface.navMeshData != null;
        }

        public void ClearAllNavMeshes()
        {
            if (!navMeshSurface)
            {
                navMeshSurface = FindAnyObjectByType<NavMeshSurface>();
            }
            
            NavMesh.RemoveAllNavMeshData();
            navMeshSurface.navMeshData = null;
        }

        /// <summary>
        /// Keeps the Room selected during a tutorial. 
        /// </summary>
        public void KeepRoomSelected()
        {
            SelectSpawnedGameObject(futureRoomInstance);
        }

        /// <summary>
        /// Keeps the Room selected during a tutorial. 
        /// </summary>
        public void KeepBotSelected()
        {
            SelectSpawnedGameObject(futureBotInstance);
        }


        /// <summary>
        /// Selects a GameObject in the scene, marking it as the active object for selection
        /// </summary>
        /// <param name="futureObjectReference"></param>
        public void SelectSpawnedGameObject(Unity.Tutorials.Editor.FutureObjectReference futureObjectReference)
        {
            if (futureObjectReference.SceneObjectReference == null) { return; }
            Selection.activeObject = futureObjectReference.SceneObjectReference.ReferencedObjectAsGameObject;
        }

        public void SelectMoveTool()
        {
            Tools.current = Tool.Move;
        }

        public void SelectRotateTool()
        {
            Tools.current = Tool.Rotate;
        }

        public void StartTutorial(Unity.Tutorials.Editor.Tutorial tutorial)
        {
            Unity.Tutorials.Editor.TutorialWindowUtils.StartTutorial(tutorial);
        }
    }
}