using System.IO;
using NUnit.Framework;
using Unity.Android.Logcat;
using Unity.PerformanceTesting;

class AndroidLogcatPerformanceTests
{
    private string m_LogMessageByPs = string.Empty;
    private string m_LogMessageByDumpsys = string.Empty;

    [SetUp]
    public void SetupLogMessages()
    {
        m_LogMessageByPs = File.ReadAllText("../../com.unity.mobile.android-logcat/Tests/Editor/LogMessageByShellPS.txt");
        m_LogMessageByDumpsys = File.ReadAllText("../../com.unity.mobile.android-logcat/Tests/Editor/LogMessageByShellDumpsys.txt");
    }

    // Test parsing messages produced by "adb shell ps".
#if (UNITY_2019_1 || UNITY_2018_3)
    [PerformanceTest]
#else
    [Test, Performance]
#endif
    public void ParsePIDByPackageName()
    {
        const int kLoopTime = 20;
        const int expectedPid = 26812;

        for (int i = 0; i < kLoopTime; ++i)
        {
            var pid = AndroidLogcatConsoleWindow.ParsePIDInfo("com.samsung.android.app.memo", m_LogMessageByPs);
            Assert.IsTrue(pid == expectedPid);
        }
    }

    // Test parsing messages produced by "adb shell "dumpsys activity"".
#if (UNITY_2019_1 || UNITY_2018_3)
    [PerformanceTest]
#else
    [Test, Performance]
#endif
    public void ParseTopActivity()
    {
        const int kLoopTime = 20;
        const int expectedPid = 4332;
        const string expectedPackageName = "com.sec.android.app.launcher";

        for (int i = 0; i < kLoopTime; ++i)
        {
            string packageName;
            var pid = AndroidLogcatConsoleWindow.ParseTopActivityPackageInfo(m_LogMessageByDumpsys, out packageName);
            Assert.IsTrue(pid == expectedPid);
            Assert.IsTrue(packageName == expectedPackageName);
        }
    }
}
