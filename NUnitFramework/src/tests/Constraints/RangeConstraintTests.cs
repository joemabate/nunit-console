﻿// ***********************************************************************
// Copyright (c) 2008 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace NUnit.Framework.Constraints.Tests
{
    [TestFixture]
    public class RangeConstraintTest : ConstraintTestBaseWithArgumentException
    {
        RangeConstraint rangeConstraint;

        [SetUp]
        public void SetUp()
        {
            theConstraint = rangeConstraint = new RangeConstraint(5, 42);
            expectedDescription = "in range (5,42)";
            stringRepresentation = "<range 5 42>";
        }

        object[] SuccessData = new object[] { 5, 23, 42 };

        object[] FailureData = new object[] { new object[] { 4, "4" }, new object[] { 43, "43" } };

        object[] InvalidData = new object[] { null, "xxx" };

        [Test]
        public void UsesProvidedIComparer()
        {
            MyComparer comparer = new MyComparer();
            Assert.That(rangeConstraint.Using(comparer).Matches(19).HasSucceeded);
            Assert.That(comparer.Called, "Comparer was not called");
        }

        class MyComparer : IComparer
        {
            public bool Called;

            public int Compare(object x, object y)
            {
                Called = true;
                return Comparer.Default.Compare(x, y);
            }
        }

        [Test]
        public void UsesProvidedComparerOfT()
        {
            MyComparer<int> comparer = new MyComparer<int>();
            Assert.That(rangeConstraint.Using(comparer).Matches(19).HasSucceeded);
            Assert.That(comparer.Called, "Comparer was not called");
        }

        class MyComparer<T> : IComparer<T>
        {
            public bool Called;

            public int Compare(T x, T y)
            {
                Called = true;
                return Comparer<T>.Default.Compare(x, y);
            }
        }

        [Test]
        public void UsesProvidedComparisonOfT()
        {
            MyComparison<int> comparer = new MyComparison<int>();
            Assert.That(rangeConstraint.Using(new Comparison<int>(comparer.Compare)).Matches(19).HasSucceeded);
            Assert.That(comparer.Called, "Comparer was not called");
        }

        class MyComparison<T>
        {
            public bool Called;

            public int Compare(T x, T y)
            {
                Called = true;
                return Comparer<T>.Default.Compare(x, y);
            }
        }

#if CS_3_0 || CS_4_0
        [Test]
        public void UsesProvidedLambda()
        {
            Comparison<int> comparer = (x, y) => x.CompareTo(y);
            Assert.That(rangeConstraint.Using(comparer).Matches(19).HasSucceeded);
        }
#endif
    }
}