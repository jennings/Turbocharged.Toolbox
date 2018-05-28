using System;
using Xunit;

namespace Turbocharged.Toolbox.Tests
{
    public class MaybeTests
    {
        [Fact]
        public void Maybe_with_a_value_inside_works()
        {
            Maybe<int> maybe = new Maybe<int>(1);

            int innerValue;
            Assert.True(maybe.HasValue);
            Assert.False(maybe.IsNull);
            Assert.True(maybe.TryGetValue(out innerValue));
            Assert.Equal(1, innerValue);
        }

        [Fact]
        public void Maybe_with_no_value_works()
        {
            Maybe<int> maybe = new Maybe<int>();

            int innerValue;
            Assert.False(maybe.HasValue);
            Assert.True(maybe.IsNull);
            Assert.False(maybe.TryGetValue(out innerValue));
        }

        [Fact]
        public void Maybe_can_match_a_value_with_a_function()
        {
            Maybe<int> maybe = 1;

            Maybe<int> incremented = maybe.Match(val => val + 1);

            int newValue;
            Assert.True(incremented.TryGetValue(out newValue));
            Assert.Equal(2, newValue);
        }

        [Fact]
        public void Maybe_can_match_a_null_with_a_function()
        {
            Maybe<int> maybe = new Maybe<int>();

            Maybe<int> incremented = maybe.Match(val => val + 1);

            int newValue;
            Assert.False(incremented.TryGetValue(out newValue));
        }

        [Fact]
        public void Maybe_can_choose_the_onValue_action_when_matching()
        {
            Maybe<int> maybe = 1;
            bool onValueCalled = false;
            bool onNullCalled = false;
            maybe.Match(
                    onValue: v => onValueCalled = true,
                    onNull: () => onNullCalled = true);
            Assert.True(onValueCalled);
            Assert.False(onNullCalled);
        }

        [Fact]
        public void Maybe_can_choose_the_onNull_action_when_matching()
        {
            Maybe<int> maybe = new Maybe<int>();
            bool onValueCalled = false;
            bool onNullCalled = false;
            maybe.Match(
                    onValue: v => onValueCalled = true,
                    onNull: () => onNullCalled = true);
            Assert.False(onValueCalled);
            Assert.True(onNullCalled);
        }

        [Fact]
        public void Maybe_can_unwrap_when_it_contains_a_value()
        {
            Maybe<int> maybe = 1;
            int unwrapped = maybe.Unwrap(2);
            Assert.Equal(1, unwrapped);
        }

        [Fact]
        public void Maybe_unwraps_using_the_provided_default_value()
        {
            Maybe<int> maybe = new Maybe<int>();
            int unwrapped = maybe.Unwrap(2);
            Assert.Equal(2, unwrapped);
        }

        [Fact]
        public void Maybe_unwraps_using_the_provided_default_value_factory()
        {
            Maybe<int> maybe = new Maybe<int>();
            int unwrapped = maybe.Unwrap(() => 2);
            Assert.Equal(2, unwrapped);
        }
    }
}
