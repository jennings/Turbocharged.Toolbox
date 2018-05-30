using System;
using System.Threading.Tasks;
using Xunit;

namespace Turbocharged.Toolbox.Tests
{
    public class AsyncOnceTests
    {
        [Fact]
        public async Task AsyncOnce_runs_an_action_the_first_time_through()
        {
            var executions = 0;
            var once = new AsyncOnce(() => { executions += 1; return Task.CompletedTask; });
            await once.ExecuteAsync();

            Assert.Equal(1, executions);
        }

        [Fact]
        public async Task AsyncOnce_does_not_execute_actions_twice()
        {
            var executions = 0;
            var once = new AsyncOnce(() => { executions += 1; return Task.CompletedTask; });
            await once.ExecuteAsync();
            await once.ExecuteAsync();

            Assert.Equal(1, executions);
        }

        [Fact]
        public async Task AsyncOnce_throws_if_the_action_throws()
        {
            var expected = new Exception();
            var once = new AsyncOnce(async () => { await Task.Yield(); throw expected; });

            var thrown = await Assert.ThrowsAsync<PoisonException>(
                async () => await once.ExecuteAsync());
            Assert.Same(thrown.InnerException, expected);
        }

        [Fact]
        public async Task AsyncOnce_throws_if_the_first_invocation_threw()
        {
            var invoked = false;
            var expected = new Exception();
            var once = new AsyncOnce(async () => { await Task.Yield(); invoked = true; throw expected; });

            await Assert.ThrowsAsync<PoisonException>(async () => await once.ExecuteAsync());
            Assert.True(invoked);

            invoked = false;
            var thrown = await Assert.ThrowsAsync<PoisonException>(
                async () => await once.ExecuteAsync());

            Assert.Same(thrown.InnerException, expected);
            Assert.False(invoked);
        }

        [Fact]
        public async Task AsyncOnce_indicates_not_poisoned_for_success()
        {
            var once = new AsyncOnce(async () => { await Task.Yield(); });

            Assert.False(once.Poisoned);
            await once.ExecuteAsync();
            Assert.False(once.Poisoned);
        }

        [Fact]
        public async Task AsyncOnce_indicates_poisoned_after_a_failure()
        {
            var once = new AsyncOnce(async () => { await Task.Yield(); throw new Exception(); });

            Assert.False(once.Poisoned);
            await Assert.ThrowsAsync<PoisonException>(() => once.ExecuteAsync());
            Assert.True(once.Poisoned);
        }
    }
}
