using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Camus.Utilities
{
    public sealed class Range : IEnumerable<int>, IEnumerable
    {
        public readonly int Begin;

        public readonly int End;

        public Range(int begin, int end)
        {
            if (Begin > End)
            {
                throw new Exception("Range Begin must greater than End!");
            }

            Begin = begin;
            End = end;
        }

        #region Create Methods

        /// <summary>
        /// Open interval: (A, B) = {x | A < x < B}.
        /// </summary>
        /// <param name="begin">Range begin value, not included.</param>
        /// <param name="end">Range end value, not included.</param>
        /// <returns></returns>
        public static Range CreateOpenRange(int begin, int end) => new Range(begin + 1, end);

        /// <summary>
        /// Closed interval: [A, B] = {x | A <= x <= B}.
        /// This is the default Range
        /// </summary>
        /// <param name="begin">Range begin value, included.</param>
        /// <param name="end">Range end value, included.</param>
        /// <returns></returns>
        public static Range CreateCloseRange(int begin, int end) => new Range(begin, end + 1);

        /// <summary>
        /// Half-open interval:  (A, B] = {x | A < x <= B}.
        /// </summary>
        /// <param name="begin">Range begin value, not included.</param>
        /// <param name="end">Range end value, included.</param>
        /// <returns></returns>
        public static Range CreateOpenCloseRange(int begin, int end) => new Range(begin + 1, end + 1);

        /// <summary>
        /// Half-open interval:  [A, B) = {x | A <= x < B}.
        /// </summary>
        /// <param name="begin">Range begin value, included.</param>
        /// <param name="end">Range end value, not included.</param>
        /// <returns></returns>
        public static Range CreateCloseOpenRange(int begin, int end) => new Range(begin, end);

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new RangeEnumerator(this);
        }

        #endregion

        #region IEnumerator

        private class RangeEnumerator : IEnumerator<int>, IEnumerator
        {
            private readonly Range Range;

            private int Offset;

            public RangeEnumerator(Range range)
            {
                Range = range;
                Reset();
            }

            object IEnumerator.Current
            {
                get => this.Current;
            }

            public int Current
            {
                get => Range.Begin + Offset;
            }

            bool IEnumerator.MoveNext() => this.MoveNext();

            public bool MoveNext()
            {
                if (Current >= Range.End)
                {
                    return false;
                }

                ++Offset;
                return true;
            }

            void IEnumerator.Reset() => this.Reset();

            public void Reset() => Offset = -1;

            void IDisposable.Dispose() {}
        }

        #endregion
    }
}
