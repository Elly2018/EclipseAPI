using UnityEngine;
using Eclipse.Base;

namespace Eclipse.Components.Character
{
    public class GroundDetection : ComponentBase
    {
        public bool IsCollider = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag != "Player")
                IsCollider = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag != "Player")
                IsCollider = false;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.tag != "Player")
                IsCollider = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player")
                IsCollider = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag != "Player")
                IsCollider = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag != "Player")
                IsCollider = true;
        }
    }
}
