using CoreCmd.Attributes;
using NetCoreUtils.Text.Json;
using RobocopyConfigManager.Misc;

namespace RobocopyConfigManager.Commands
{
    class PathCommand
    {
        RobocopyConfig config = JsonConfigOperator<RobocopyConfig>.LoadCreate(RobocopyConfigParameters.fullConfigFilePath);

        [Help("Add a path alias")]
        public void Add(string alias, string path)
        {

        }

        [Help("Rename a path alias")]
        public void Rename(string currentName, string newName)
        {

        }

        [Help("Remove a path alias")]
        public void Remove(string name)
        {

        }
    }
}
