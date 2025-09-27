using Autofac;
using Core.Services;

namespace Api.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StudentService>().As<IStudentService>().InstancePerLifetimeScope();
            builder.RegisterType<CourseService>().As<ICourseService>().InstancePerLifetimeScope();
        }
    }
}
