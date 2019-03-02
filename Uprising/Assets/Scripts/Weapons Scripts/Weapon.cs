
using UnityEngine;
namespace Uprising.Item
{
    public abstract class Weapon : Item
    {
        public GameObject belette;
        public GameObject weapon;

        public float accuracy;
        public float range;
        public float firerate;
        public float knockback;

        public abstract void Aim(); //Aim

        void Update()
        {
            if (this.durability <= 0)
            {
                this.StopUsing();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0)) //Left-Click
            {
                Use();
                durability--;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1)) //Right-Click
            {
                Aim();
            }
        }

        protected override void StopUsing()
        {
            // Destroy(this);
        }
    }
}