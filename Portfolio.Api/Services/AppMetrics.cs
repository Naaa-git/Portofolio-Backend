using Prometheus;

namespace Portfolio.Api.Services;

/// <summary>
/// Custom business metrics, on top of the generic HTTP metrics prometheus-net
/// already exposes automatically. These are what make the Grafana dashboard
/// tell a story specific to THIS app (login attacks, lockouts) instead of just
/// generic "requests per second" that any API would have.
/// </summary>
public static class AppMetrics
{
    public static readonly Counter FailedLoginAttempts = Metrics.CreateCounter(
        "portfolio_failed_login_attempts_total",
        "Number of failed login attempts, broken down by which step failed.",
        new CounterConfiguration { LabelNames = ["stage"] });

    public static readonly Counter AccountLockouts = Metrics.CreateCounter(
        "portfolio_account_lockouts_total",
        "Number of times an account got locked out after too many failed attempts.");

    /// <summary>
    /// Forces every series to exist with an initial value of 0 from boot,
    /// instead of only appearing the first time something actually increments
    /// them. Without this, Prometheus has no earlier sample to diff against
    /// the FIRST time a real event happens — increase() needs at least two
    /// points, so the very first lockout/failure after each restart would
    /// silently fail to trigger its alert.
    /// </summary>
    public static void EnsureRegistered()
    {
        AccountLockouts.Inc(0);
        FailedLoginAttempts.WithLabels("password").Inc(0);
        FailedLoginAttempts.WithLabels("totp").Inc(0);
        FailedLoginAttempts.WithLabels("email_otp").Inc(0);
    }
}
