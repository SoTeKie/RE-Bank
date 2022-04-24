using osu.Framework.Testing;

namespace rebank.Game.Tests.Visual
{
    public class rebankTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new rebankTestSceneTestRunner();

        private class rebankTestSceneTestRunner : rebankGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}
