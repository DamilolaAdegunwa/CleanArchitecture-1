using Autofac;
using TodoApp.Core.Interfaces;
using TodoApp.Core.Services;

namespace TodoApp.Core;

public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<ToDoItemSearchService>()
        .As<IToDoItemSearchService>().InstancePerLifetimeScope();
  }
}
