using System.ComponentModel;
using System.Text.RegularExpressions;

namespace ODSphereRouter.Models
{
    [Flags]
    public enum GalacticPower
    {
        [Description("No Power")]
        NoPower = 0,
        [Description("Aisling Duval")]
        AislingDuval = 1 << 0,
        [Description("Archon Delaine")]
        ArchonDelaine = 1 << 1,
        [Description("Arissa Lavigny-Duval")]
        ArissaLavignyDuval = 1 << 2,
        [Description("Denton Patreus")]
        DentonPatreus = 1 << 3,
        [Description("Edmund Mahon")]
        EdmundMahon = 1 << 4,
        [Description("Felicia Winters")]
        FeliciaWinters = 1 << 5,
        [Description("Li Yong-Rui")]
        LiYongRui = 1 << 6,
        [Description("Pranav Antal")]
        PranavAntal = 1 << 7,
        [Description("Yuri Grom")]
        YuriGrom = 1 << 8,
        [Description("Zachary Hudson")]
        ZacharyHudson = 1 << 9,
        [Description("Zemina Torval")]
        ZeminaTorval = 1 << 10
    }

    public enum PowerPlayState
    {
        None,
        Contested,
        Controlled,
        Expansion,
        Exploited,
        [Description("Home System")]
        HomeSystem,
        [Description("In Prepare Radius")]
        InPrepareRadius,
        Prepared,
        Turmoil
    }

    public enum Government
    {
        None,
        Anarchy,
        Communism,
        Confederacy,
        Cooperative,
        Corporate,
        Democracy,
        Dictatorship,
        Feudal,
        Patronage,
        Prison,
        [Description("Prison Colony")]
        PrisonColony,
        Theocracy,
        [Description(" Workshop (Engineer)")]
        WorkshopEngineer,
    }

    [Flags]
    public enum FactionsState
    {
        None = 0,
        Blight = 1 << 0,
        Boom = 1 << 1,
        Bust = 1 << 2,
        [Description("Civil Liberty")]
        CivilLiberty = 1 << 3,
        [Description("Civil Unrest")]
        CivilUnrest = 1 << 4,
        [Description("Civil War")]
        CivilWar = 1 << 5,
        Drought = 1 << 6,
        Election = 1 << 7,
        Expansion = 1 << 8,
        Famine = 1 << 9,
        [Description("Infrastructure Failure")]
        InfrastructureFailure = 1 << 10,
        Investment = 1 << 11,
        Lockdown = 1 << 12,
        [Description("Natural Disaster")]
        NaturalDistaster = 1 << 13,
        Outbreak = 1 << 14,
        [Description("Pirate Attack")]
        PirateAttack = 1 << 15,
        [Description("Public Holiday")]
        PublicHoliday = 1 << 16,
        Retreat = 1 << 17,
        [Description("Terrorist Attack")]
        TerroristAttack = 1 << 18,
        War = 1 << 19,
        [Description("Terrorism")]
        Terrorism = 1 << 20
    }

    public enum Allegiance
    {
        Alliance,
        Empire,
        Federation,
        Independent,
        None,
        [Description("Pilots Federation")]
        PilotsFederation,
    }

    public static partial class Enums
    {
        [GeneratedRegex(@"\s+")]
        private static partial Regex PowerStateRegex();

        public static GalacticPower StringToPowers(IEnumerable<string>? powers)
        {
            var ret = GalacticPower.NoPower;

            if (powers is not null && powers.Any())
            {
                foreach (var power in powers)
                {
                    ret |= StringToEnum<GalacticPower>(power);
                }
            }
            return ret;
        }

        public static PowerPlayState StringToPowerState(string? state)
        {
            if (state is null)
            {
                return PowerPlayState.None;
            }

            var powerState = PowerStateRegex().Replace(state, "");
            return StringToEnum<PowerPlayState>(powerState);
        }

        public static FactionsState StringToFactionsStates(IEnumerable<string>? states)
        {
            FactionsState ret = FactionsState.None;

            if (states is not null && states.Any())
            {
                foreach (var state in states)
                {
                    ret |= StringToEnum<FactionsState>(state);
                }
            }
            return ret;
        }

        public static Government StringToGovernment(string? input)
        {
            return StringToEnum<Government>(input);
        }

        public static Allegiance StringToAllegiance(string? input)
        {
            return StringToEnum<Allegiance>(input);
        }

        public static TEnum StringToEnum<TEnum>(string? input) where TEnum : struct
        {
            if (input == null)
            {
                Enum.TryParse("", true, out TEnum InputType);

                return InputType;
            }
            var name = input.Where(c => !char.IsWhiteSpace(c)).Where(c => c != '-')
                .Where(c => c != '(').Where(c => c != ')').ToArray();

            Enum.TryParse(name, true, out TEnum resultInputType);

            return resultInputType;
        }
    }
}
