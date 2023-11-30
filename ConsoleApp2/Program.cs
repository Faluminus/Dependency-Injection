using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            

            var container2 = new DependencyContainer2();
            container2.Register<ServiceA>(()=>new ServiceA());
            container2.Register<ServiceB>(()=> new ServiceB(container2.Resolve<ServiceA>()));
            container2.Register<ServiceB>(c => new ServiceB(c.Resolve<ServiceA>));
       
            
            Console.ReadKey();
            
        }
    }
    public class DependencyContainer2
    {
        private readonly Dictionary<Type,Func<object>> _service = new Dictionary<Type, Func<object>>();

        public void Register<TInterface>(Func<TInterface> TImplementation)
        {
            _service[typeof(TInterface)] = () => TImplementation();
        }
        public void Register<TInterface, TImplementation>()
        {
            var implementationType = typeof(TImplementation);
            _service[typeof(TInterface)] = (_) => (TInterface)Activator.CreateInstance(implementationType));
        }
        public void Register<TInterface>(Func<DependencyContainer2, TInterface> builder)
        {
            _service[typeof(TInterface)] = (container)=>builder(container);
        }
        public TInterface Resolve<TInterface>()
        {
            if(_service.TryGetValue(typeof(TInterface), out var implementationType))
            {
                return (TInterface)implementationType();
            }

            throw new InvalidOperationException($"Service for interface {typeof(TInterface)} not registered");
        }
    }
    public class DependencyContainer
    {
        private readonly Dictionary<Type, Type> _service = new Dictionary<Type, Type>();

        public void Register<TInterface, TImplementation>()
        {
            _service[typeof(TInterface)] = typeof(TImplementation);
        }
        public TInterface Resolve<TInterface>()
        {
            if (_service.TryGetValue(typeof(TInterface), out Type implementationType))
            {
                return (TInterface)Activator.CreateInstance(implementationType);
            }

            throw new InvalidOperationException($"Service for interface {typeof(TInterface)} not registered");
        }
    }
    public class DependencyContainer3
    {
        private readonly Dictionary<Type, Func<object>> _service = new Dictionary<Type, Func<object>>();

        public void Register<TInterface>(Func<TInterface> TImplementation)
        {
            _service[typeof(TInterface)] = () => TImplementation();
        }
        public void Register<TInterface, TImplementation>()
        {
            _service[typeof(TInterface)] = () => (TInterface)Activator.CreateInstance(typeof(TImplementation));
        }
        public TInterface Resolve<TInterface>()
        {
            if (_service.TryGetValue(typeof(TInterface), out var implementationType))
            {
                return (TInterface)implementationType();
            }

            throw new InvalidOperationException($"Service for interface {typeof(TInterface)} not registered");
        }
    }

    public interface IService
    {
        void Run();
    }
    public class ServiceA : IService
    {
        public void Run()
        {
            Console.WriteLine("Im running as A");
        }
        public ServiceA(ServiceC c , ServiceD d)
        {

        }
    }
    public class ServiceB : IService
    {
        public void Run()
        {
            Console.WriteLine("Im runnig as B");
        }
    }
    public class ServiceC : IService
    {
        public void Run()
        {
            Console.WriteLine("Im runnig as C");
        }
    }
    public class ServiceD : IService
    {
        public void Run()
        {
            Console.WriteLine("Im runnig as D");
        }
        public ServiceD(ServiceF f, ServiceG g)
        {

        }
    }
    public class ServiceE : IService
    {
        public void Run()
        {
            Console.WriteLine("Im runnig as E");
        }
        public ServiceE(ServiceB b, ServiceH h)
        {

        }
    }
    public class ServiceF : IService
    {
        public void Run()
        {
            Console.WriteLine("Im runnig as F");
        }
    }
    public class ServiceG : IService
    {
        public void Run()
        {
            Console.WriteLine("Im runnig as G");
        }
    }
    public class ServiceH : IService
    {
        public void Run()
        {
            Console.WriteLine("Im runnig as H");
        }
    }
}
