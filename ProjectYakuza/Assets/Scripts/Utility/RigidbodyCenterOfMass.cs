using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CS4455.Utility {


    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyCenterOfMass : MonoBehaviour {

		[Tooltip("An optional marker GameObject used to identify where the center of mass is. If assigned non-null, it will be used to determine the centerOfMass")]
		public GameObject centerOfMassMarker = null;

		public Vector3 centerOfMass { get {return m_centerOfMass; } set { m_centerOfMass = value; } }

        [SerializeField]
        public Vector3 m_centerOfMass = Vector3.zero;

		[Tooltip("Whether the centerOfMass is re-evaluated every Update()")]
		public bool continuousUpdating = false;

        protected Rigidbody rb;

        void Awake() {
            rb = GetComponent<Rigidbody>();
        }

    	
    	void Start () {

            AssignCenterOfMass();
           
    	}

        void Update() {

            if (continuousUpdating)
                AssignCenterOfMass();
        }


        public void AssignCenterOfMass() {

            if (centerOfMassMarker != null)
            {
                centerOfMass = this.transform.InverseTransformPoint(centerOfMassMarker.transform.position);
            }
           
            if(rb.centerOfMass != centerOfMass)
                rb.centerOfMass = centerOfMass;
        }




        //void OnDrawGizmosSelected()
        //{
        //    // Draw a yellow sphere at the transform's position
        //    Gizmos.color = Color.yellow;
        //    Gizmos.matrix = transform.localToWorldMatrix;
        //    Gizmos.DrawWireSphere(this.centerOfMass, 0.1f);
        //}


    }

}
