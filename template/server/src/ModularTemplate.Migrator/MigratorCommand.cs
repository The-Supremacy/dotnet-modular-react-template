using ModularTemplate.Identity.Access;

namespace ModularTemplate.Migrator;

public sealed record MigratorCommand(InitialAdminOptions? InitialAdmin)
{
    public static bool TryParse(
        string[] args,
        out MigratorCommand command,
        out string? error)
    {
        command = new MigratorCommand((InitialAdminOptions?)null);
        error = null;

        if (args is [] or ["migrate"])
        {
            return true;
        }

        if (args is not ["identity", "grant-admin", ..])
        {
            error = "Usage: migrator [migrate] | identity grant-admin --provider <issuer> --subject <subject> [--force]";
            return false;
        }

        string? provider = null;
        string? subject = null;
        bool force = false;

        for (int index = 2; index < args.Length; index++)
        {
            string arg = args[index];
            switch (arg)
            {
                case "--provider":
                    if (!TryReadValue(args, ref index, out provider))
                    {
                        error = "--provider requires a value.";
                        return false;
                    }

                    break;
                case "--subject":
                    if (!TryReadValue(args, ref index, out subject))
                    {
                        error = "--subject requires a value.";
                        return false;
                    }

                    break;
                case "--force":
                    force = true;
                    break;
                default:
                    error = $"Unknown argument '{arg}'.";
                    return false;
            }
        }

        if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(subject))
        {
            error = "Both --provider and --subject are required.";
            return false;
        }

        command = new MigratorCommand(
            new InitialAdminOptions
            {
                Provider = provider,
                Subject = subject,
                Force = force
            });
        return true;
    }

    private static bool TryReadValue(string[] args, ref int index, out string? value)
    {
        value = null;
        int valueIndex = index + 1;
        if (valueIndex >= args.Length || args[valueIndex].StartsWith("--", StringComparison.Ordinal))
        {
            return false;
        }

        value = args[valueIndex];
        index = valueIndex;
        return true;
    }
}
