﻿using System;

namespace Ryocatusn.Games.Stages
{
    public class StageId : IEquatable<StageId>
    {
        private string value { get; }

        public StageId(string value)
        {
            this.value = value;
        }

        public bool Equals(StageId other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return Equals(other.value, value);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(obj, this)) return true;
            if (GetType() != obj.GetType()) return false;
            return Equals((StageId)obj);
        }
        public override int GetHashCode()
        {
            return value != null ? value.GetHashCode() : 0;
        }
    }
}