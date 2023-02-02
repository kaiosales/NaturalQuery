using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Metadata.Edm;
using NaturalQuery.QueryTreeSyntax;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Threading.Tasks;
using NaturalQuery.EntityFramework.Test.Helpers;

namespace NaturalQuery.EntityFramework.Test
{
    [TestClass]
    public class TestQueryTreeExecution : AbstractTestQueryTree
    {
        [TestCleanup]
        public void Finalizer()
        {
            if (Given != null && Expected != null && Actual != null)
            {
                Debug.WriteLine("Given the following tree:");
                Debug.WriteLine((object)Given);
                Debug.WriteLine(string.Empty);
                Debug.WriteLine("Expected query:");
                Debug.WriteLine((object)Expected);
                Debug.WriteLine(string.Empty);
                Debug.WriteLine("Actual query:");
                Debug.WriteLine((object)Actual);
            }
        }


        [TestMethod]
        [TestCategory("QueryTree To SQL Query")]
        public void Test_build_successfully()
        {
            Given = new QueryTree(
                QueryNode.Select(
                    QueryNode.Field("ContactName"),
                    QueryNode.Entity(StoreModelContext.Customers)
                )
            );

            var query = Given.ToQueryable();

            Assert.IsNotNull(query);
        }

        [TestMethod]
        [TestCategory("QueryTree To SQL Query")]
        public void Mostre_a_descricao_dos_clientes()
        {
            Expected =
                StoreModelContext.Customers
                .Select(c => c.ContactName);

            Given = new QueryTree(
                QueryNode.Select(
                    QueryNode.Field("ContactName"),
                    QueryNode.Entity(StoreModelContext.Customers)
                )
            );


            Actual = Given.ToQueryable();

            DataAssert.AreEquals(Expected, Actual);
        }

        [TestMethod]
        [TestCategory("QueryTree To SQL Query")]
        public void Mostre_a_descricao_dos_clientes_que_tenham_pedidos_ordenado_pelo_maior_total_do_pedido_descendente()
        {
            Expected =
                StoreModelContext.Customers
                .Join(StoreModelContext.Orders, c => c.CustomerID, o => o.CustomerID, (c, o) => new { Customer = c, Order = o })
                .GroupBy(x => x.Customer.ContactName)
                .Select(g => new
                {
                    g = g,
                    max = g.Max(x => x.Order.OrderTotal)
                })
                .OrderByDescending(x => x.max)
                .Select(x => x.g.Key);

            Given = new QueryTree(
                QueryNode.Select(
                    QueryNode.Field("ContactName"),
                    QueryNode.OrderDescending(
                        QueryNode.Select(
                            QueryNode.Projection(
                                QueryNode.GroupField("ContactName"),
                                QueryNode.Aggregation(
                                    QueryNode.Value("Max"),
                                    QueryNode.Field("Orders.OrderTotal")
                                )
                            ),
                            QueryNode.Group(
                                QueryNode.Join(
                                    QueryNode.Entity(StoreModelContext.Customers),
                                    QueryNode.Field("CustomerID"),
                                    QueryNode.Entity(StoreModelContext.Orders),
                                    QueryNode.Field("CustomerID")
                                ),
                                QueryNode.Field("Customers.ContactName")
                            )
                        ),
                        QueryNode.Field("OrderTotal")
                    )
                )
            );

            Actual = Given.ToQueryable();

            DataAssert.AreEquals(Expected, Actual);

        }

        [TestMethod]
        [TestCategory("QueryTree To SQL Query")]
        public void Mostre_a_descricao_dos_clientes_com_o_nome_contendo_Joao()
        {
            Expected =
                StoreModelContext.Customers
                .Where(c => c.ContactName.Contains("João"))
                .Select(c => c.ContactName);

            Given = new QueryTree(
                QueryNode.Select(
                    QueryNode.Field("ContactName"),
                    QueryNode.Where(
                        QueryNode.Entity(StoreModelContext.Customers),
                        QueryNode.Contains(
                            QueryNode.Field("ContactName"),
                            QueryNode.Value("João")
                        )
                    )
                )
            );

            Actual = Given.ToQueryable();

            DataAssert.AreEquals(Expected, Actual);
        }

        [TestMethod]
        [TestCategory("QueryTree To SQL Query")]
        public void Mostre_o_nome_da_cidade_e_a_quantidade_de_clientes_agrupados_por_cidade()
        {
            Expected =
                StoreModelContext.Customers
                .GroupBy(c => c.City)
                .Select(g => new
                {
                    City = g.Key,
                    Count = g.Count()
                });

            Given = new QueryTree(
                QueryNode.Select(
                    QueryNode.Projection(
                        QueryNode.GroupField("City"),
                        QueryNode.Aggregation(new ValueQueryNode("Count"), new EmptyQueryNode())
                    ),
                    QueryNode.Group(
                        QueryNode.Entity(StoreModelContext.Customers),
                        QueryNode.Field("City")
                    )
                )
            );

            Actual = Given.ToQueryable();

            DataAssert.AreEquals(Expected, Actual);
        }

        [TestMethod]
        [TestCategory("QueryTree To SQL Query")]
        public void Mostre_o_nome_e_a_cidade_dos_clientes_que_pedidos_que_tenham_produtos_da_categoria_A()
        {
            Expected =
                StoreModelContext.Customers.Join(
                    StoreModelContext.Orders.Join(
                        StoreModelContext.OrderDetails.Join(
                            StoreModelContext.Products.Join(
                                StoreModelContext.Categories,
                                p => p.CategoryID,
                                cat => cat.CategoryID,
                                (p, cat) => new { Product = p, Category = cat }
                            ),
                            d => d.ProductID,
                            p => p.Product.ProductID,
                            (d, pc) => new { OrderDetail = d, Product_Category = pc }
                        ),
                        o => o.OrderID,
                        dpc => dpc.OrderDetail.OrderID,
                        (o, dpc) => new { Order = o, OrderDetail_Product_Category = dpc }
                    ),
                    c => c.CustomerID,
                    odpc => odpc.Order.CustomerID,
                    (c, odpc) => new { Customer = c, Order_OrderDetail_Product_Category = odpc }
                )
                .Where(x => x.Order_OrderDetail_Product_Category.OrderDetail_Product_Category.Product_Category.Category.CategoryName == "A")
                .Select(x => new
                {
                    ContactName = x.Customer.ContactName,
                    City = x.Customer.City
                });


            Given = new QueryTree(
                QueryNode.Select(
                    QueryNode.Projection(
                        QueryNode.Field("Customers.ContactName"),
                        QueryNode.Field("Customers.City")
                    ),
                    QueryNode.Where(
                        QueryNode.Join(
                            QueryNode.Entity(StoreModelContext.Customers),
                            QueryNode.Field("CustomerID"),
                            QueryNode.Join(
                                QueryNode.Entity(StoreModelContext.Orders),
                                QueryNode.Field("OrderID"),
                                QueryNode.Join(
                                    QueryNode.Entity(StoreModelContext.OrderDetails),
                                    QueryNode.Field("ProductID"),
                                    QueryNode.Join(
                                        QueryNode.Entity(StoreModelContext.Products),
                                        QueryNode.Field("CategoryID"),
                                        QueryNode.Entity(StoreModelContext.Categories),
                                        QueryNode.Field("CategoryID")
                                    ),
                                    QueryNode.Field("Products.ProductID")
                                ),
                                QueryNode.Field("OrderDetails.OrderID")
                            ),
                            QueryNode.Field("Orders.CustomerID")
                        ),
                        QueryNode.IsEquals(
                            QueryNode.Field("Categories.CategoryName"),
                            QueryNode.Value("A")
                        )
                    )
                )
            );

            Actual = Given.ToQueryable();

            DataAssert.AreEquals(Expected, Actual);
        }

        [TestMethod]
        [TestCategory("QueryTree To SQL Query")]
        public void Mostre_a_descricao_dos_clientes_que_tenham_pedidos_com_total_maior_que_2_ordenados_descendente_pela_data_do_pedido()
        {
            Expected =
                StoreModelContext.Customers
                .Join(StoreModelContext.Orders, c => c.CustomerID, o => o.CustomerID, (c, o) => new { Customer = c, Order = o })
                .Where(x => x.Order.OrderTotal > 2)
                .OrderByDescending(x => x.Order.OrderDate)
                .Select(x => x.Customer.ContactName);

            Given = new QueryTree(
                QueryNode.Select(
                    QueryNode.Field("Customers.ContactName"),
                    QueryNode.OrderDescending(
                        QueryNode.Where(
                            QueryNode.Join(
                                QueryNode.Entity(StoreModelContext.Customers),
                                QueryNode.Field("CustomerID"),
                                QueryNode.Entity(StoreModelContext.Orders),
                                QueryNode.Field("CustomerID")
                            ),
                            QueryNode.GreaterThan(
                                QueryNode.Field("Orders.OrderTotal"),
                                QueryNode.Value(2)
                            )
                        ),
                        QueryNode.Field("Orders.OrderDate")
                    )
                )
            );

            Actual = Given.ToQueryable();

            DataAssert.AreEquals(Expected, Actual);
        }

        [TestMethod]
        [TestCategory("QueryTree To SQL Query")]
        public void Mostre_a_descricao_dos_clientes_ordenado_descendente_por_ID()
        {
            Expected =
                StoreModelContext.Customers
                .OrderByDescending(c => c.CustomerID)
                .Select(c => c.ContactName);

            Given = new QueryTree(
                QueryNode.Select(
                    QueryNode.Field("ContactName"),
                    QueryNode.OrderDescending(
                        QueryNode.Entity(StoreModelContext.Customers),
                        QueryNode.Field("CustomerID")
                    )
                )
            );

            Actual = Given.ToQueryable();

            DataAssert.AreEquals(Expected, Actual);
        }
    }
}
