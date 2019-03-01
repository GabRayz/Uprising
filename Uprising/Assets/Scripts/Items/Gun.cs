using System;

namespace Uprising.Item
{
    public class DefaultGun : Weapon
    {

        public DefaultGun(int durability)
        {
            this.durability = durability;
        }

        public override void Aim()
        {
            throw new System.NotImplementedException();
        }
    }
}
