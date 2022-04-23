using osu.Framework.Testing;

namespace REBankOsu.Game.Tests.Visual
{
    public class REBankOsuTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new REBankOsuTestSceneTestRunner();

        private class REBankOsuTestSceneTestRunner : REBankOsuGameBase, ITestSceneTestRunner
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
