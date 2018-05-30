using System;
using Xunit;

namespace Turbocharged.Toolbox.Tests
{
    public class ResultExtensionsTests
    {
        [Fact]
        public void UnwrapOrThrow_with_exception_types_works()
        {
            var expected = new Exception();
            Result<int, Exception> result = expected;

            var actual = Assert.Throws<Exception>(() => result.UnwrapOrThrow());
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Select_works_with_a_value_result()
        {
            Result<int, Exception> result = 10;
            var output = from r in result
                         select r;

            Assert.Equal(10, output.UnwrapOrThrow());
        }

        [Fact]
        public void Select_works_with_an_error_result()
        {
            var expected = new Exception();
            Result<int, Exception> result = expected;
            var output = from r in result
                         select r;

            Assert.True(output.TryGetError(out Exception actual));
            Assert.Equal(expected, actual);
        }
    }
}
