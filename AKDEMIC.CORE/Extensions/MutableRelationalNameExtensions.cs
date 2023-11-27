namespace AKDEMIC.CORE.Extensions
{
    public static class MutableRelationalNameExtensions
    {
        public class MutableRelationalName
        {
            readonly string value;

            public MutableRelationalName(string value)
            {
                this.value = value;
            }

            public static implicit operator string(MutableRelationalName mutableRelationalName)
            {
                return mutableRelationalName.value;
            }

            public static implicit operator MutableRelationalName(string value)
            {
                return new MutableRelationalName(value);
            }
        }

        public static string NormalizeRelationalName(this MutableRelationalName mutableRelationalName)
        {
            var mutableRelationalNameSplit = ((string)mutableRelationalName).Split("_");
            var tmpMutableRelationalName = "";

            for (var i = 0; i < mutableRelationalNameSplit.Length; i++)
            {
                var mutableRelationalNameSplitValue = mutableRelationalNameSplit[i];

                if (tmpMutableRelationalName != "")
                {
                    tmpMutableRelationalName += "_";
                }

                if (mutableRelationalNameSplitValue.Contains("."))
                {
                    var mutableRelationalNameSplitValueSplit = mutableRelationalNameSplitValue.Split(".");
                    tmpMutableRelationalName += mutableRelationalNameSplitValueSplit[1];
                }
                else
                {
                    tmpMutableRelationalName += mutableRelationalNameSplitValue;
                }
            }

            return tmpMutableRelationalName;
        }
    }
}
