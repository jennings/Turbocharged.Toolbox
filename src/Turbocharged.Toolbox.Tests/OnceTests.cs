using System;
using Xunit;

namespace Turbocharged.Toolbox.Tests
{
    public class OnceTests
    {
        [Fact]
        public void Once_runs_an_action_the_first_time_through()
        {
            var executions = 0;
            var once = new Once(() => executions += 1);
            once.Execute();

            Assert.Equal(1, executions);
        }

        [Fact]
        public void Once_does_not_execute_actions_twice()
        {
            var executions = 0;
            var once = new Once(() => executions += 1);
            once.Execute();
            once.Execute();

            Assert.Equal(1, executions);
        }

        [Fact]
        public void Once_throws_if_the_action_throws()
        {
            var expected = new Exception();
            var once = new Once(() => { throw expected; });

            var thrown = Assert.Throws<PoisonException>(() => once.Execute());
            Assert.Same(thrown.InnerException, expected);
        }

        [Fact]
        public void Once_throws_if_the_first_invocation_threw()
        {
            var invoked = false;
            var expected = new Exception();
            var once = new Once(() => { invoked = true; throw expected; });

            Assert.Throws<PoisonException>(() => once.Execute());
            Assert.True(invoked);

            invoked = false;
            var thrown = Assert.Throws<PoisonException>(() => once.Execute());

            Assert.Same(thrown.InnerException, expected);
            Assert.False(invoked);
        }
    }
}
