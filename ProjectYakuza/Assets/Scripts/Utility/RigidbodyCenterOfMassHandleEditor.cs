#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace CS4455.Utility
{
    [CustomEditor(typeof(RigidbodyCenterOfMass))]//, CanEditMultipleObjects]
    public class RigidbodyCenterOfMassHandleEditor : Editor
    {

        // Do we want to draw positioning handles for editing CoM position?
        protected bool editEnabled = false;

        protected virtual void OnSceneGUI()
        {

            // Reference to component we are editing
            RigidbodyCenterOfMass component = (RigidbodyCenterOfMass)target;

            // Map local coords to world
            Vector4 comv4 = new Vector4(component.centerOfMass.x, component.centerOfMass.y, component.centerOfMass.z, 1f);
            Vector4 wcomv4 = component.transform.localToWorldMatrix * comv4;
            Vector3 handlePos = new Vector3(wcomv4.x, wcomv4.y, wcomv4.z);

            if (editEnabled)
            {

                EditorGUI.BeginChangeCheck();

                // Draw handles
                Vector3 newHandlePos = Handles.PositionHandle(handlePos, component.transform.rotation);

                //Debug.Log("localTOWorld: " + component.transform.localToWorldMatrix);
                //Debug.Log("Handle Position: " + handlePos);
                //Debug.Log("New target pos: " + newHandlePos);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(component, "Change Center of Mass Position");

                    // Go back to local coords
                    Vector4 nhpv4 = new Vector4(newHandlePos.x, newHandlePos.y, newHandlePos.z, 1f);
                    Vector4 new_com = component.transform.worldToLocalMatrix * nhpv4;

                    //Debug.Log("new_com: " + new_com);

                    component.centerOfMass = new Vector3(new_com.x, new_com.y, new_com.z);

                    //component.Update();

                    if(EditorApplication.isPlaying)
                    {
                        component.AssignCenterOfMass();
                    }
                }

            }
            //else
            //{
            //    Handles.SphereHandleCap(0, handlePos, component.transform.rotation, 0.1f, EventType.MouseUp);
            //}
        }


        void OnSelectionChange()
        {
            this.editEnabled = false;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Edit the center of mass or specify a GameObject to serve as COM", MessageType.Info);

            RigidbodyCenterOfMass myScript = (RigidbodyCenterOfMass)target;

            myScript.centerOfMassMarker = (GameObject)EditorGUILayout.ObjectField(new GUIContent("CoM Marker Object", 
                                                                                    "An optional marker GameObject used to identify where the center of mass is. If assigned non-null, it will be used to determine the centerOfMass"), 
                                                                                    myScript.centerOfMassMarker, typeof(GameObject), true);

            if (myScript.centerOfMassMarker == null)
            {
                string btnTxt = !this.editEnabled ? "Edit Center of Mass Position" : "Stop Editing";

                if (GUILayout.Button(btnTxt))
                {

                    this.editEnabled = !this.editEnabled;
                    //this.Repaint();
                    EditorUtility.SetDirty(target);
                }

                myScript.centerOfMass = EditorGUILayout.Vector3Field(new GUIContent("Center of Mass", 
                                                                        "The relative position defining the center of mass that will be used in physics simulation. Will be overridden by centerOfMassMarker if non-null."), 
                                                                        myScript.centerOfMass);
            }

            myScript.continuousUpdating = EditorGUILayout.Toggle(new GUIContent("Continuous Updating", "Whether the centerOfMass is re-evaluated every FixedUpdate()"), myScript.continuousUpdating);

            //DrawDefaultInspector();


        }

        [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
        //[DrawGizmo(GizmoType.Pickable | GizmoType.InSelectionHierarchy)]
        static void DrawGizmoRigidbodyCenterOfMass(RigidbodyCenterOfMass obj, GizmoType gizmoType)
        {
        
            // Draw a sphere at the transform's position
            Gizmos.color = Color.cyan;
            Gizmos.matrix = obj.transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(obj.centerOfMass, 0.1f);
        }

    }
}

#endif