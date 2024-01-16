using CodeChallenge.Config;
using System;

public class Program
{

    public static void Main(string[] args)
    {
        // This doesn't matter on Windows at all, the Pyroscope thing only works on Linux but maybe I can fix that.
        Environment.SetEnvironmentVariable("CORECLR_ENABLE_PROFILING", "1");
        Environment.SetEnvironmentVariable("CORECLR_PROFILER", "{BD1A650D-AC5D-4896-B64F-D6FA25D6B26A}");
        Environment.SetEnvironmentVariable("CORECLR_PROFILER_PATH", "/dotnet/Pyroscope.Profiler.Native.so");
        Environment.SetEnvironmentVariable("LD_PRELOAD", "/dotnet/Pyroscope.Linux.ApiWrapper.x64.so");
        Environment.SetEnvironmentVariable("PYROSCOPE_PROFILING_ENABLED", "1");
        Environment.SetEnvironmentVariable("PYROSCOPE_APPLICATION_NAME", "CodeChallenge");
        Environment.SetEnvironmentVariable("PYROSCOPE_SERVER_ADDRESS", "https://profiles-prod-001.grafana.net");
        Environment.SetEnvironmentVariable("PYROSCOPE_BASIC_AUTH_USER", "828812");
        Environment.SetEnvironmentVariable("PYROSCOPE_BASIC_AUTH_PASSWORD", "glc_eyJvIjoiMTAyNzE1NCIsIm4iOiJzdGFjay04Mjg4MTItaHAtd3JpdGUtcHlyb3Njb3BlLXRva2VuIiwiayI6Im0wODVrekM5cnoxd2NVMHNFMDUybTZzZSIsIm0iOnsiciI6InByb2QtdXMtZWFzdC0wIn19");

        new App().Configure(args).Run();
    }
}