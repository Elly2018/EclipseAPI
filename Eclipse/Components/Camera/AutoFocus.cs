using UnityEngine.Rendering.PostProcessing;
using Eclipse.Base;
using UnityEngine;

namespace Eclipse.Components.Camera
{
    [AddComponentMenu("Eclipse/Components/Camera/Auto Focus")]
    public class AutoFocus : ComponentBase 
    {
        
        [SerializeField] private PostProcessVolume PPP;

        private float velocity;

        private void Start()
        {
            PPP = GameObject.FindObjectOfType<PostProcessVolume>();
        }

        private void Update()
        {
            if (PPP)
            {
                Ray ray = GetComponent<UnityEngine.Camera>().ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, 1000.0f))
                {
                    Debug.Log("Hit");
                    // Get distance from camera and target
                    float dist = Vector3.Distance(transform.position, hit.point);
                    DepthOfField e = PPP.profile.GetSetting<DepthOfField>();
                    e.focusDistance.value = Mathf.SmoothDamp(e.focusDistance.value, dist, ref velocity, 0.5f);
                }
            }
        }
    }
}
