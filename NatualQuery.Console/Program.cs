using NaturalQuery.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NaturalQuery
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Test();
        }


        public void Test()
        {
            using(StoreModelContext ctx = new StoreModelContext())
            {
                int pageSize = 30;
                int currentPage = 1;

                var list = ctx.Customers
                                    .OrderBy(c => c.CustomerID)
                                    .Take(pageSize)
                                    .Skip(currentPage * pageSize)
                                    .ToList();
                
            }

        }

        public void Run()
        {
            //DynamicSelectCliente("NaturalQuery.Model.Entities.Cliente", typeof(Context));

            string input = string.Empty;
            TokenChain chain = new TokenChain();

            chain.Add(new VerbToken());
            chain.Add(new ArticleToken());
            chain.Add(new EntityToken());

            chain.Source = MockSource();

            Console.WriteLine("Informe a pesquisa:");
            Console.WriteLine();
            while(input.ToLower() != "sair")
            {
                input = Console.ReadLine();
                var list = chain.Process(input);
                foreach (var item in list)
                {
                    Console.WriteLine("Item: {0} ({1})", item.Value, item.TokenType.Name);
                }

            }
        }

        public DataSource MockSource()
        {
            DataSource result = new DataSource();
            SourceEntity entity = new SourceEntity();
            entity.Name = "Cliente";

            result.Entities.Add(entity);

            return result;
        }

        //public void DynamicSelectCliente(string targetName, Type providerType)
        //{
        //    //Type targetType = Assembly.GetExecutingAssembly().GetType(targetName);
        //    Type targetType = typeof(NaturalQuery.Model.Entities.Cliente);

        //    Dictionary<string, PropertyInfo> dataProviders = providerType.GetProperties().Where(p => p.PropertyType.GetInterface(typeof(IQueryable).FullName) != null).ToDictionary(k => k.Name, v => v);


        //    var ctx = Activator.CreateInstance(providerType);

        //    IQueryable<dynamic> source = (IQueryable<dynamic>)dataProviders[targetType.Name.Pluralize()].GetValue(ctx);


        //    var result = Queryable.Select(source, SelectHelper.DynamicSelect(targetType, "ClienteId", "Nome"));
        //    var lresult = result.Take(20);

        //    foreach (var item in result)
        //    {
        //        Console.WriteLine(item.Nome);
        //    }

        //    (ctx as IDisposable).Dispose();
        //}

        

    }
}
