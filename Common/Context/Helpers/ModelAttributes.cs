using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Context.Helpers
{

    [AttributeUsage(AttributeTargets.Property)]
    public class OnDeleteCascadeAttribute : Attribute
    {
        public OnDeleteCascadeAttribute()
        {

        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class DbModelAttribute : Attribute
    {
        public DbModelAttribute()
        {

        }
    }
}
