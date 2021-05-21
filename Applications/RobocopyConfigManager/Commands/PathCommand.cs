using CoreCmd.Attributes;
using NetCoreUtils.Text.Json;
using RobocopyConfigManager.Misc;
using System;

namespace RobocopyConfigManager.Commands
{
    //class PathCommand
    //{
    //    RobocopyConfig config = JsonConfigOperator<RobocopyConfig>.LoadCreate(RobocopyConfigParameters.fullConfigFilePath);

    //    [Help("List all available path aliases")]
    //    public void List()
    //    {
    //        foreach(var alias in config.Pathes)
    //        {
    //            Console.WriteLine($"{alias.Name}\t{alias.Path}");
    //        }
    //    }

    //    [Help("Add a path alias")]
    //    public void Add(string alias, string path)
    //    {
    //        config.Pathes.Add(new PathAlias { Name = alias, Path = path });
    //        JsonConfigOperator<RobocopyConfig>.Save(RobocopyConfigParameters.fullConfigFilePath, config);
    //    }

    //    [Help("Rename a path alias")]
    //    public void Rename(string currentName, string newName)
    //    {
    //        var alias = config.GetAlias(currentName);
    //        if(alias != null)
    //        {
    //            alias.Name = newName;
    //        }
    //        JsonConfigOperator<RobocopyConfig>.Save(RobocopyConfigParameters.fullConfigFilePath, config);
    //    }

    //    [Help("Remove a path alias")]
    //    public void Remove(string name)
    //    {
    //        config.RemoveAlias(name);
    //        JsonConfigOperator<RobocopyConfig>.Save(RobocopyConfigParameters.fullConfigFilePath, config);
    //    }
    //}
}
