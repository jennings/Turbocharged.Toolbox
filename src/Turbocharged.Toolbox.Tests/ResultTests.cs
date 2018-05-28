using System;
using Xunit;

namespace Turbocharged.Toolbox.Tests
{
    public class ResultTests
    {
        const int IntValue = 10;
        const string StringValue = "Hello";
        const string NullString = null;

        [Fact]
        public void Result_can_contain_a_value()
        {
            Result<int, string> result = IntValue;

            Assert.True(result.HasValue);
            Assert.False(result.HasError);

            int actualValue;
            Assert.True(result.TryGetValue(out actualValue));
            Assert.Equal(IntValue, actualValue);

            string actualError;
            Assert.False(result.TryGetError(out actualError));
        }

        [Fact]
        public void Result_can_contain_a_error()
        {
            Result<int, string> result = StringValue;

            Assert.False(result.HasValue);
            Assert.True(result.HasError);

            int actualValue;
            Assert.False(result.TryGetValue(out actualValue));

            string actualError;
            Assert.True(result.TryGetError(out actualError));
            Assert.Equal(StringValue, actualError);
        }

        [Fact]
        public void Result_unwraps_values_successfully()
        {
            Result<int, string> result = IntValue;
            var unwrapped = result.Unwrap(0);
            Assert.Equal(IntValue, unwrapped);
        }

        [Fact]
        public void Result_unwraps_errors_successfully()
        {
            Result<int, string> result = StringValue;
            var unwrapped = result.Unwrap(str => 2);
            Assert.Equal(2, unwrapped);
        }

        [Fact]
        public void Result_unwraps_and_does_not_throw_when_it_contains_a_value()
        {
            Result<int, string> result = IntValue;
            var unwrapped = result.UnwrapOrThrow(str => new InvalidOperationException());
            Assert.Equal(IntValue, unwrapped);
        }

        [Fact]
        public void Result_unwraps_and_throws_when_it_contains_an_error()
        {
            Result<int, string> result = StringValue;
        }
    }
}
