using System;

namespace Plane.PlaneData
{
    [Serializable]
    public class OilData: ICloneable
    {
        public float Amount;
        
        
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}