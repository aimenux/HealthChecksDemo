using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApi
{
    public class Examples
    {
        private const Ids Current = Ids.Example10;

        private static readonly Dictionary<Type, string> StartupTypes = new()
        {
            {typeof(Example01.Startup), null},                                  // EXAMPLE 01 with url checkers (one endpoint)
            {typeof(Example02.Startup), null},                                  // EXAMPLE 02 with custom checkers (one endpoint)
            {typeof(Example03.Startup), null},                                  // EXAMPLE 03 with custom options (four endpoints)
            {typeof(Example04.Startup), null},                                  // EXAMPLE 04 with ui (fluent configuration, one endpoint)
            {typeof(Example05.Startup), null},                                  // EXAMPLE 05 with ui (fluent configuration, two endpoints)
            {typeof(Example06.Startup), "Example06/healthchecks.json"},         // EXAMPLE 06 with ui (json configuration, two endpoints, absolute uris, memory storage)
            {typeof(Example07.Startup), "Example07/healthchecks.json"},         // EXAMPLE 07 with ui (json configuration, two endpoints, relative uris, memory storage)
            {typeof(Example08.Startup), "Example08/healthchecks.json"},         // EXAMPLE 08 with ui (json configuration, two endpoints, relative uris, sql lite database storage)
            {typeof(Example09.Startup), "Example09/healthchecks.json"},         // EXAMPLE 09 with ui (json configuration, two endpoints, relative uris, sql server database storage)
            {typeof(Example10.Startup), "Example10/healthchecks.json"},         // EXAMPLE 10 with ui (json configuration, two endpoints, relative uris, memory storage, sqlServer checks)
        };

        public static (Type, string) GetStartupConfiguration()
        {
            const int index = (int) Current - 1;
            var (startupType, jsonFile) = StartupTypes.ElementAt(index);
            return (startupType, jsonFile);
        }

        public enum Ids
        {
            Example1 = 1,
            Example2,
            Example3,
            Example4,
            Example5,
            Example6,
            Example7,
            Example8,
            Example9,
            Example10,
        }
    }
}