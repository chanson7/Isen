using UnityEngine;
using HexMapTools;

namespace HexMapToolsExamples
{

    public enum CellColor { White = 0, Blue, Red }

    [RequireComponent(typeof(Animator))]
    public class Cell : MonoBehaviour
    {
        [SerializeField]
        private CellColor color = CellColor.White;
        private bool isHighlighted = false;
        private bool isSelected = false;
        private Animator animator;

        public CellColor Color
        {
            get { return color; }
            set
            {
                if (color == value)
                    return;

                color = value;

                animator.SetInteger("State", (int)color);
            }
        }

        public bool IsHighlighted
        {
            get { return isHighlighted; }
            set
            {
                if (isHighlighted == value)
                    return;

                isHighlighted = value;

                animator.SetBool("IsHighlighted", isHighlighted);
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected == value)
                    return;

                isSelected = value;
                animator.SetBool("IsSelected", isSelected);
            }
        }


        public HexCoordinates Coords
        {
            get;
            private set;
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
            animator.SetInteger("State", (int)color);
        }

        public void Init(HexCoordinates coords)
        {
            Coords = coords;
        }

        private void OnDrawGizmos()
        {
            if (Color == CellColor.White)
                return;

            if (Color == CellColor.Red)
                Gizmos.color = UnityEngine.Color.red;
            else
                Gizmos.color = UnityEngine.Color.blue;

            Gizmos.DrawWireSphere(transform.position, 0.433f);
        }
    }



}
