
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


// Code by Ron James Theron
namespace EditorUtilities
{
    public class MeshStatisticsUtility : MonoBehaviour
    {
#if (UNITY_EDITOR)


        private static float _lastMenuCallTimestamp = 0f;

        /// <summary>
        ///  Print the mesh statistics of the selected gameObjects which have a valid mesh attached in the console.
        ///  Can be called via the context menu after right-clicking in the hierachy.    
        /// </summary>
        [MenuItem("GameObject/Editor Utilities/Debug Selected Mesh Statistics", false, 0)]
        static void DebugSelectedMeshStatistics()
        {
            //Ensures time has passed since last menu call.
            //prevents function from being called for every selected gameObject.
            bool timePassedSinceLastMenuCall = !Time.unscaledTime.Equals(_lastMenuCallTimestamp);
            if (!timePassedSinceLastMenuCall)
            {
                return;
            }

            //Calculates the total vertex and triangle count on all meshfilters currently selected in hierachy.
            GameObject[] selectedGameObjects = Selection.gameObjects;
            int totalTriangleCount = 0;
            int totalVertexCount = 0;
            foreach (GameObject selected in selectedGameObjects)
            {
                //If the gameObject which is selected has a non-null mesh attatched to it
                //Add the triangle and vertex count's to the respective total variables.
                //Prints the gameObject's name followed by it's individual triangle and vertice count.
                MeshFilter meshFilter = null;
                selected.TryGetComponent<MeshFilter>(out meshFilter);
                bool hasMesh = ValidateMesh(selected);
                if (!hasMesh)
                {
                    continue;
                }
                else
                {
                    int selectedTriangleCount = (meshFilter.sharedMesh.triangles.Length / 3);
                    int selectedVertexCount = meshFilter.sharedMesh.vertexCount;

                    totalTriangleCount += selectedTriangleCount;
                    totalVertexCount += selectedVertexCount;

                    string debugObjectMessage = "Game Object " + selected.name
                    + " contains \n"
                    + selectedTriangleCount.ToString() + " Triangles"
                    + " and " + selectedVertexCount.ToString() + " Vertices";
                    Debug.Log(debugObjectMessage);
                }


            }

            string totalMessage = "Total Selected Triangle Count: " + totalTriangleCount.ToString()
            + "\n Total Selected Vertex Count: " + totalVertexCount.ToString();
            Debug.Log(totalMessage);

            //Sets the _lastMenuCallTimestamp variable to the current time stamp.
            _lastMenuCallTimestamp = Time.unscaledTime;
        }

        /// <summary>
        ///  Hides the DebugSelectedMeshStatistics() context menu item if there are no valid meshes selected in the hierachy.
        /// </summary>
        /// <returns>Returns true if there is a valid mesh selected in the hierachy, false otherwise.</returns>
        [MenuItem("GameObject/Editor Utilities/Debug Selected Mesh Statistics", true, 0)]
        static bool ValidateSelectedMeshStatistics()
        {
            GameObject[] selectedGameObjects = Selection.gameObjects;
            bool selectionContainsMesh = ContainsMeshFilter(selectedGameObjects);
            return selectionContainsMesh;
        }

        /// <summary>
        ///  Checks a single gameobject to see if it has an attached mesh.
        /// </summary>
        /// <param name="item">The game object to be validated</param>
        /// <returns>Returns true if there is a non-null mesh filter and mesh.</returns>
        public static bool ValidateMesh(GameObject item)
        {
            MeshFilter meshFilter;
            item.TryGetComponent<MeshFilter>(out meshFilter);
            if (meshFilter != null)
            {
                bool hasValidMesh = meshFilter.sharedMesh != null;
                return hasValidMesh;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        ///  Checks to see if any of the gameObjects in the passed in array have an attatched mesh component. 
        /// </summary>
        /// <param name="array">The array of gameobjects which you want to validate.</param>
        /// <returns>Returns true if one of the gameobjects in the array has an attatched mesh.</returns>
        public static bool ContainsMeshFilter(GameObject[] array)
        {
            foreach (GameObject item in array)
            {
                bool containsMesh = ValidateMesh(item);
                if (containsMesh)
                {
                    return true;
                }
            }
            return false;
        }
#endif
    }
}

