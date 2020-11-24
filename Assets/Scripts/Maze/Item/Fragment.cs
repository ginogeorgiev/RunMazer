using UnityEngine;
using Random = UnityEngine.Random;

namespace Maze.Item
{
    /// <summary>
    /// if you collide with the item, the fragment score goes up and plate moves on the y axis
    /// goes back to old position once it exits the collision
    /// </summary>
    public class Fragment : MazeItem
    {
        private bool alreadyPressed = false;
        private Vector3 pos;
        private float yPos = -0.95f;
        private float plateMovingDiff;
        /// <summary>
        /// fragment gets random color
        /// -0.95 and -1.04 are random numbers that i thought would look good
        /// </summary>
        private void Awake()
        {
            Material mat = GetComponent<Renderer>().material;
            mat.color = new Color(Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
            plateMovingDiff = yPos + 1.04f;
            transform.position = new Vector3(transform.position.x,yPos,transform.position.z);
            pos = transform.position;
        }

        protected override void StayEffect()
        {
            transform.position = new Vector3(pos.x, yPos-plateMovingDiff, pos.z);
        }

        protected override void EnterEffect()
        {
            
            if (!alreadyPressed)
            {
                ScoreManager.Instance.AddFragmentScore();
                alreadyPressed = true;
            }
        }

        protected override void ExitEffect()
        {
            transform.position = pos;
        }

    }
}