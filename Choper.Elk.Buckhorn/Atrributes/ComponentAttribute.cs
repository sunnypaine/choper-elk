using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Buckhorn.Atrributes
{
    public class ComponentAttribute : Attribute
    {
        private string name;
        public string Name
        {
            get { return this.name; }
        }


        public ComponentAttribute()
        { }

        public ComponentAttribute(string name)
        {
            this.name = name;
        }
    }
}
