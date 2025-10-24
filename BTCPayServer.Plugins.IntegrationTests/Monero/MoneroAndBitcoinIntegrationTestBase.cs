using BTCPayServer.Tests;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

using Xunit.Abstractions;

namespace BTCPayServer.Plugins.IntegrationTests.Monero;

public class MoneroAndBitcoinIntegrationTestBase : UnitTestBase
{
    public IContainer WebWalletContainer;

    public MoneroAndBitcoinIntegrationTestBase(ITestOutputHelper helper) : base(helper)
    {
        SetDefaultEnv("BTCPAY_XMR_DAEMON_URI", "http://127.0.0.1:18081");
        SetDefaultEnv("BTCPAY_XMR_WALLET_DAEMON_URI", "http://127.0.0.1:18082");
        SetDefaultEnv("BTCPAY_XMR_WALLET_DAEMON_WALLETDIR", "/wallet");

        WebWalletContainer = new ContainerBuilder()
            .WithImage("btcpayserver/monero:0.18.4.2")
            .WithName("xmr-wallet")
            .WithCommand(
                "monero-wallet-rpc",
                "--log-level", "2",
                "--allow-mismatched-daemon-version",
                "--rpc-bind-ip=0.0.0.0",
                "--disable-rpc-login",
                "--confirm-external-bind",
                "--rpc-bind-port=18082",
                "--non-interactive",
                "--trusted-daemon",
                "--daemon-address", $"{ Environment.GetEnvironmentVariable("BTCPAY_XMR_DAEMON_URI") }:18081",
                "--wallet-dir", "/wallet",
                "--tx-notify", "/bin/sh ./scripts/notifier.sh -k -X GET https://127.0.0.1:14142/monerolikedaemoncallback/tx?cryptoCode=xmr&hash=%s"
            )
            .WithPortBinding(18082, true)
            .WithVolumeMount("xmr_wallet", "/wallet")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilExternalTcpPortIsAvailable(18082))
            .Build();
    }

    private static void SetDefaultEnv(string key, string defaultValue)
    {
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(key)))
        {
            Environment.SetEnvironmentVariable(key, defaultValue);
        }
    }
}