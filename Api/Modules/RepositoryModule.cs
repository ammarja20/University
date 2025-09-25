using Autofac;
using Data.Repositories;

namespace Api.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StudentRepository>().As<IStudentRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CourseRepository>().As<ICourseRepository>().InstancePerLifetimeScope();
        }
    }
}
