using Common.Context.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Common.Context
{
    public abstract class BasicModel
    {
        public int Id { get; set; }

        [OnDeleteCascade]
        public IEnumerable<ScheduledJob> Jobs { get; set; }
    }

    public abstract class BasicModel<T> : BasicModel, IEquatable<BasicModel<T>>
    {
        public override bool Equals(object obj)
        {
            var other = obj as BasicModel<T>;
            if (other == null) return false;

            return Equals(other);
        }

        public bool Equals([AllowNull] BasicModel<T> other)
        {
            return Id == other.Id;
        }
    }
}
